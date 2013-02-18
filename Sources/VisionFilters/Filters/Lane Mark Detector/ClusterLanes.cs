using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Auton.CarVision.Video;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using RANSAC.Functions;

namespace VisionFilters.Filters.Lane_Mark_Detector
{
    public class ClusterLanes : ThreadSupplier<List<Point>, SimpleRoadModel> 
    {
        private Supplier<List<Point>> supplier;
        private double roadCenterDistAvg = 190; // estimated relative road distance [half of width]

        const int MinPointsForOnlyOne = 50;
        const int MinPointsForEach    = 30;
        int imgWidth  = CamModel.Width;
        int imgHeight = CamModel.Height;
        Parabola last = null;

        private void ObtainSimpleModel(List<Point> lanes)
        {
            //SimpleRoadModel road;

            //return;

            Parabola leftLane   = null;
            Parabola rightLane  = null;
            Parabola roadCenter = null;

            if (lanes.Count > MinPointsForOnlyOne)
                roadCenter = RANSAC.RANSAC.fit(900, 8, (int)(lanes.Count * 0.8), 6, lanes);

            if (roadCenter != null) 
            {
                double x = roadCenter.value(imgHeight - 50);
                if (x < imgWidth / 2) // WARNING !!! MAY BE FUCKUP @@@
                {
                    leftLane = roadCenter;
                    roadCenter = new Parabola(leftLane.a, leftLane.b, leftLane.c + roadCenterDistAvg);
                    rightLane = new Parabola(roadCenter.a, roadCenter.b, roadCenter.c + roadCenterDistAvg);
                }
                else
                {
                    rightLane = roadCenter;
                    roadCenter = new Parabola(rightLane.a, rightLane.b, rightLane.c - roadCenterDistAvg);
                    leftLane = new Parabola(rightLane.a, rightLane.b, rightLane.c - 2 * roadCenterDistAvg);
                }
            }
            else // no one line mark can be matched. trying to find left and right and then again trying to find model.
            {
                // try to cluster data to distinguish left and right lane
                List<Point> first = new List<Point>(1024);
                List<Point> second = new List<Point>(1024);
                VisionToolkit.Two_Means_Clustering(lanes, ref first, ref second);

                //////////////////////////////////////////////////////////////

                if (first.Count > MinPointsForEach)
                    leftLane = RANSAC.RANSAC.fit(900, 8, (int)(first.Count * 0.8), 6, first);

                if (second.Count > MinPointsForEach)
                    rightLane = RANSAC.RANSAC.fit(900, 8, (int)(second.Count * 0.8), 6, second);

                if (leftLane != null && rightLane != null)
                {
                    // swap lanes if necessary
                    if (leftLane.value(imgHeight) > rightLane.value(imgHeight))
                    {
                        var t = leftLane;
                        leftLane = rightLane;
                        rightLane = leftLane;
                    }

                    // center is between left and right lane
                    roadCenter = new Parabola(
                        0.5 * (leftLane.a + rightLane.a),
                        0.5 * (leftLane.b + rightLane.b),
                        0.5 * (leftLane.c + rightLane.c)
                        );

                    //roadCenterDistAvg = (rightLane.c - roadCenter.c) * 0.1 + (roadCenter.c - leftLane.c) * 0.1 + roadCenterDistAvg * 0.8; // reestimate road center
                }
                else if (leftLane != null)
                {
                    // check if this is really a left lane
                    if (leftLane.value(imgHeight - 8) > imgWidth - 5) // this is right lane!!
                    {
                        rightLane = leftLane;
                        leftLane = null;
                        roadCenter = new Parabola(rightLane.a, rightLane.b, rightLane.c - roadCenterDistAvg);
                    }
                    else
                        roadCenter = new Parabola(leftLane.a, leftLane.b, leftLane.c + roadCenterDistAvg);
                    }
                else if (rightLane != null)
                {
                    // check if this is really a left lane
                    if (rightLane.value(imgHeight - 8) < imgWidth + 5) // this is left lane!!
                    {
                        leftLane = rightLane;
                        rightLane = null;
                        roadCenter = new Parabola(leftLane.a, leftLane.b, leftLane.c + roadCenterDistAvg);
                    }
                    else
                        roadCenter = new Parabola(rightLane.a, rightLane.b, rightLane.c + roadCenterDistAvg);
                }
            }

            if (leftLane == null || rightLane == null)
            {
                if (last != null)
                {
                    double d = roadCenter.at(imgHeight - 80) - last.at(imgHeight - 80);
                    System.Console.WriteLine("Distance: " + d.ToString());
                    if (Math.Abs(d) > 50) // wtf...
                    {
                        if (leftLane != null)
                        {
                            rightLane = leftLane;
                            leftLane = null;
                            roadCenter = new Parabola(rightLane.a, rightLane.b, rightLane.c + roadCenterDistAvg);
                        }
                        else if (rightLane != null)
                        {
                            leftLane = rightLane;
                            rightLane = null;
                            roadCenter = new Parabola(leftLane.a, leftLane.b, leftLane.c + roadCenterDistAvg);
                        }
                    }
                        
                }
            }
            if (last != null)
                last = roadCenter;
            LastResult = new SimpleRoadModel(roadCenter, leftLane, rightLane);
            PostComplete();
        }

        public ClusterLanes(Supplier<List<Point>> supplier_)
        {
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            Process += ObtainSimpleModel;
        }
    }
}
