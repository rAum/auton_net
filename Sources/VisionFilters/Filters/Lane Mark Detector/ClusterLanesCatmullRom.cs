using System;
using System.Collections.Generic;
using Auton.CarVision.Video;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using RANSAC.Functions;


namespace VisionFilters.Filters.Lane_Mark_Detector
{
    /// <summary>
    /// Road modeling using parabolic equation
    /// </summary>
    public class ClusterLanes_CatmullRom : ThreadSupplier<List<Point>, SimpleRoadModel>
    {
        private Supplier<List<Point>> supplier;
        private double roadCenterDistAvg = 200; // estimated relative road distance [half of width]

        const double ROAD_CENTER_MIN = 175;
        const double ROAD_CENTER_MAX = 230;
        const int CENTER_PROBE_OFFSET = 10;
        const int MIN_POINTS_FOR_EACH = 280;
        const int MIN_POINTS_FOR_ONLY_ONE = 300;

        const int RANSAC_ITERATIONS = 400;
        const int RANSAC_MODEL_SIZE = 3;
        const int RANSAC_MAX_ERROR = 10;
        const double RANSAC_INLINERS = 0.75;

        int imgWidth = CamModel.Width;
        int imgHeight = CamModel.Height;
        int centerProbePoint,
            carCenter;

        private void ObtainSimpleModel(List<Point> lanes)
        {
            CatmullRom leftLane = null;
            CatmullRom rightLane = null;
            CatmullRom roadCenter = null;

            if (lanes.Count > MIN_POINTS_FOR_ONLY_ONE)
                roadCenter = RANSAC.RANSAC.fitCatmullRom(RANSAC_ITERATIONS, RANSAC_MODEL_SIZE, (int)(lanes.Count * RANSAC_INLINERS), RANSAC_MAX_ERROR, lanes);

            if (roadCenter != null)
                create_model_from_single_line(ref roadCenter, ref leftLane, ref rightLane);
            else if (lanes.Count > MIN_POINTS_FOR_EACH)// no one line mark can be matched. trying to find left and right and then again trying to find model.
            {
                // try to cluster data to distinguish left and right lane
                List<Point> first = new List<Point>(2048);
                List<Point> second = new List<Point>(2048);

                VisionToolkit.Two_Means_Clustering(lanes, ref first, ref second);

                ////////////////////////////////////////////////////////////////

                if (first.Count > MIN_POINTS_FOR_EACH)
                    leftLane = RANSAC.RANSAC.fitCatmullRom(RANSAC_ITERATIONS, RANSAC_MODEL_SIZE, (int)(first.Count * RANSAC_INLINERS), RANSAC_MAX_ERROR, first);

                if (second.Count > MIN_POINTS_FOR_EACH)
                    rightLane = RANSAC.RANSAC.fitCatmullRom(RANSAC_ITERATIONS, RANSAC_MODEL_SIZE, (int)(second.Count * RANSAC_INLINERS), RANSAC_MAX_ERROR, second);

                create_model_from_two_lanes(ref leftLane, ref rightLane, ref roadCenter);
            }

            LastResult = new SimpleRoadModel(roadCenter, leftLane, rightLane);
            PostComplete();
        }

        private void create_model_from_two_lanes(ref CatmullRom leftLane, ref CatmullRom rightLane, ref CatmullRom roadCenter)
        {
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
                roadCenter = CatmullRom.merge(leftLane, rightLane);

                // reestimate road center
                // TODO: probe in some places and take average.
                //double new_road_width = ((rightLane.c - roadCenter.c) + (roadCenter.c - leftLane.c)) * 0.5 * 0.05 + roadCenterDistAvg * 0.95;
                //roadCenterDistAvg = Math.Max(Math.Min(new_road_width, ROAD_CENTER_MAX), ROAD_CENTER_MAX);
            }
            else if (leftLane != null) // check if this is really a left lane
            {
                if (leftLane.at(centerProbePoint) > carCenter) // this is right lane!!
                {
                    rightLane = leftLane;
                    leftLane = null;
                    roadCenter = CatmullRom.Moved(rightLane, -roadCenterDistAvg);
                }
                else roadCenter = CatmullRom.Moved(leftLane, roadCenterDistAvg);
            }
            else if (rightLane != null) // check if this is really a left lane
            {
                if (rightLane.at(centerProbePoint) <= carCenter) // this is left lane!!
                {
                    leftLane = rightLane;
                    rightLane = null;
                    roadCenter = CatmullRom.Moved(leftLane, roadCenterDistAvg);
                }
                else roadCenter = CatmullRom.Moved(rightLane, -roadCenterDistAvg);
            }
        }

        private void create_model_from_single_line(ref CatmullRom roadCenter, ref CatmullRom leftLane, ref CatmullRom rightLane)
        {
            double x = roadCenter.at(imgHeight - CENTER_PROBE_OFFSET);
            if (x < carCenter)
            {
                leftLane = roadCenter;
                roadCenter = CatmullRom.Moved(leftLane, roadCenterDistAvg);
                rightLane = CatmullRom.Moved(roadCenter, roadCenterDistAvg);
            }
            else
            {
                rightLane = roadCenter;
                roadCenter = CatmullRom.Moved(rightLane, -roadCenterDistAvg);
                leftLane = CatmullRom.Moved(roadCenter, -roadCenterDistAvg);
            }
        }


        public ClusterLanes_CatmullRom(Supplier<List<Point>> supplier_)
        {
            supplier = supplier_;
            centerProbePoint = imgHeight - CENTER_PROBE_OFFSET;
            carCenter = imgWidth / 2;

            supplier.ResultReady += MaterialReady;
            Process += ObtainSimpleModel;
        }
    }
}
