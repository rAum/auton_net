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
    public class ClusterLanes : ThreadSupplier<Image<Gray, Byte>, SimpleRoadModel> 
    {
        private Supplier<Image<Gray, Byte>> supplier;

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

            // try to cluster data to distinguish left and right lane
            List<Point> first = new List<Point>(100);
            List<Point> second = new List<Point>(100);
            VisionToolkit.Two_Means_Clustering(lanes, ref first, ref second);

            Parabola leftLane   = null;
            Parabola rightLane  = null;
            Parabola roadCenter = null;

            // if there are enough points, try to find lanes.
            if (first.Count > 8 && second.Count > 8)
            {
                leftLane  = RANSAC.RANSAC.fit(1100, 8, (int)(first.Count * 0.8), 3, first);
                rightLane = RANSAC.RANSAC.fit(1100, 8, (int)(second.Count * 0.8), 3, second);

                if (leftLane != null && rightLane != null)
                {
                    // swap lanes if necessary
                    if (leftLane.value(img.Height) > rightLane.value(img.Height))
                    {
                        var t     = leftLane;
                        leftLane  = rightLane;
                        rightLane = leftLane;
                    }

                    // center is between left and right lane
                    roadCenter = new Parabola(
                        0.5 * (leftLane.a + rightLane.a),
                        0.5 * (leftLane.b + rightLane.b),
                        0.5 * (leftLane.c + rightLane.c)
                        );
                }
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
