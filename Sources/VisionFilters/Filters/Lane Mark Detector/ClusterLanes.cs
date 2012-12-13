﻿using System;
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
    public class ClusterLanes : ThreadSupplier<Image<Gray, Byte>, SimpleRoadModel> 
    {
        private Supplier<Image<Gray, Byte>> supplier;
        private double roadCenterDistAvg = 180;

        private void ObtainSimpleModel(Image<Gray, Byte> img)
        {
            List<Point> lanes = new List<Point>(200);

            // find pixels which can be on lane mark
            byte[,,] raw = img.Data;
            for (int y = 0; y < img.Height; ++y)
            {
                for (int x = 0; x < img.Width; ++x)
                {
                    if (raw[y,x,0] == 255)
                    {
                        lanes.Add(new Point(x, y));
                    }
                }
            }

            //////////////////////////////////////////////////////////////
            // trying to estimate which points belong to which lane.

            // try to cluster data to distinguish left and right lane
            List<Point> first = new List<Point>(100);
            List<Point> second = new List<Point>(100);
            VisionToolkit.Two_Means_Clustering(lanes, ref first, ref second);

            ////////////////////////////////////////////////////////////

            // if there are enough points, try to find lanes.

            Parabola leftLane   = null;
            Parabola rightLane  = null;
            Parabola roadCenter = null;

            if (first.Count > 8)
                leftLane = RANSAC.RANSAC.fit(1100, 8, (int)(first.Count * 0.7), 5, first);

            if (second.Count > 8)
                rightLane = RANSAC.RANSAC.fit(1100, 8, (int)(second.Count * 0.7), 5, second);

            if (leftLane != null && rightLane != null)
            {
                // swap lanes if necessary
                if (leftLane.value(img.Height) > rightLane.value(img.Height))
                {
                    var t     = leftLane;
                    leftLane  = rightLane;
                    rightLane = leftLane;
                }

                if (Math.Abs(leftLane.c - rightLane.c) < roadCenterDistAvg * 0.3) // two lines are too near...
                {
                    first.AddRange(second);
                    leftLane = RANSAC.RANSAC.fit(800, 8, (int)(first.Count * 0.65), 6, first);
                    System.Console.Out.WriteLine("Two lines are too near!");
                    roadCenter = new Parabola(leftLane.a, leftLane.b, leftLane.c + (leftLane.c > CamModel.Width? -roadCenterDistAvg : roadCenterDistAvg));
                }
                else
                {
                    // center is between left and right lane
                    roadCenter = new Parabola(
                        0.5 * (leftLane.a + rightLane.a),
                        0.5 * (leftLane.b + rightLane.b),
                        0.5 * (leftLane.c + rightLane.c)
                        );

                    roadCenterDistAvg = (rightLane.c - roadCenter.c) * 0.75 + roadCenterDistAvg * 0.25;
                }

            }
            else if (leftLane != null)
            {
                // check if this is really a left lane
                if (leftLane.value(img.Height - 8) > img.Width - 5) // this is right lane!!
                {
                    rightLane = leftLane;
                    leftLane = null;
                    roadCenter = new Parabola(rightLane.a, rightLane.b, rightLane.c - roadCenterDistAvg);
                }
                else
                    roadCenter = new Parabola(leftLane.a, leftLane.b,  leftLane.c + roadCenterDistAvg);
            }
            else if (rightLane != null)
            {
                // check if this is really a left lane
                if (rightLane.value(img.Height - 8) < img.Width + 5) // this is left lane!!
                {
                    leftLane = rightLane;
                    rightLane = null;
                    roadCenter = new Parabola(leftLane.a, leftLane.b, leftLane.c + roadCenterDistAvg);
                }
                else
                    roadCenter = new Parabola(rightLane.a, rightLane.b, rightLane.c + roadCenterDistAvg);
            }

            LastResult = new SimpleRoadModel(roadCenter, leftLane, rightLane);
            PostComplete();
        }

        public ClusterLanes(Supplier<Image<Gray, Byte>> supplier_)
        {
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            Process += ObtainSimpleModel;
        }
    }
}
