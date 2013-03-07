using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Auton.CarVision.Video;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using RANSAC.Functions;
using System.Diagnostics;

namespace VisionFilters.Filters.Lane_Mark_Detector
{
    public class ClusterLanes : ThreadSupplier<List<Point>, SimpleRoadModel> 
    {
        private Supplier<List<Point>> supplier;
        private double roadCenterDistAvg = 200; // estimated relative road distance [half of width]
        
        const double ROAD_CENTER_MIN = 175;
        const double ROAD_CENTER_MAX = 230;
        const int CENTER_PROBE_OFFSET = 10;
        const int MIN_POINTS_FOR_ONLY_ONE = 300;
        const int MIN_POINTS_FOR_EACH    = 280;
        int imgWidth  = CamModel.Width;
        int imgHeight = CamModel.Height;
        int centerProbePoint,
            carCenter;
        
        private void ObtainSimpleModel(List<Point> lanes)
        {
            Parabola leftLane   = null;
            Parabola rightLane  = null;
            Parabola roadCenter = null;

            if (lanes.Count > MIN_POINTS_FOR_ONLY_ONE)
                roadCenter = RANSAC.RANSAC.fitParabola(700, 6, (int)(lanes.Count * 0.75), 5, lanes);

            if (roadCenter != null) 
            {
                double x = roadCenter.at(imgHeight - CENTER_PROBE_OFFSET);
                if (x < carCenter)
                {
                    leftLane   = roadCenter;
                    roadCenter = Parabola.Moved(leftLane,   roadCenterDistAvg);
                    rightLane  = Parabola.Moved(roadCenter, roadCenterDistAvg);
                }
                else
                {
                    rightLane  = roadCenter;
                    roadCenter = Parabola.Moved(rightLane,  -roadCenterDistAvg);
                    leftLane   = Parabola.Moved(roadCenter, -roadCenterDistAvg);
                }
            }
            else if (lanes.Count > MIN_POINTS_FOR_EACH)// no one line mark can be matched. trying to find left and right and then again trying to find model.
            {
                // try to cluster data to distinguish left and right lane
                List<Point> first = new List<Point>(2048);
                List<Point> second = new List<Point>(2048);

                VisionToolkit.Two_Means_Clustering(lanes, ref first, ref second);

                ////////////////////////////////////////////////////////////////

                if (first.Count > MIN_POINTS_FOR_EACH)
                    leftLane = RANSAC.RANSAC.fitParabola(700, 6, (int)(first.Count * 0.75), 5, first);

                if (second.Count > MIN_POINTS_FOR_EACH)
                    rightLane = RANSAC.RANSAC.fitParabola(700, 6, (int)(second.Count * 0.75), 5, second);

                if (leftLane != null && rightLane != null)
                {
                    // swap lanes if necessary
                    if (leftLane.at(centerProbePoint) > rightLane.at(centerProbePoint))
                    {
                        var t = leftLane;
                        leftLane = rightLane;
                        rightLane = leftLane;
                    }

                    // center is between left and right lane
                    roadCenter = Parabola.merge(leftLane, rightLane);

                    // reestimate road center
                    double new_road_width = ((rightLane.c - roadCenter.c) + (roadCenter.c - leftLane.c))* 0.5*0.05 + roadCenterDistAvg * 0.95;
                    roadCenterDistAvg     =  Math.Max(Math.Min(new_road_width, ROAD_CENTER_MAX), ROAD_CENTER_MAX );
                }
                else if (leftLane != null) // check if this is really a left lane
                {
                    if (leftLane.at(centerProbePoint) > carCenter) // this is right lane!!
                    {
                        rightLane  = leftLane;
                        leftLane   = null;
                        roadCenter = Parabola.Moved(rightLane, -roadCenterDistAvg);
                    }
                    else
                        roadCenter = Parabola.Moved(leftLane,   roadCenterDistAvg);
                }
                else if (rightLane != null) // check if this is really a left lane
                {
                    if (rightLane.at(centerProbePoint) <= carCenter) // this is left lane!!
                    {
                        leftLane = rightLane;
                        rightLane = null;
                        roadCenter = Parabola.Moved(leftLane,   roadCenterDistAvg);
                    }
                    else
                        roadCenter = Parabola.Moved(rightLane, -roadCenterDistAvg);
                }
            }

            LastResult = new SimpleRoadModel(roadCenter, leftLane, rightLane);
            PostComplete();
        }

        

        public ClusterLanes(Supplier<List<Point>> supplier_)
        {
            supplier = supplier_;
            centerProbePoint = imgHeight - CENTER_PROBE_OFFSET;
            carCenter = imgWidth / 2;

            supplier.ResultReady += MaterialReady;
            Process += ObtainSimpleModel;
        }
    }
}
