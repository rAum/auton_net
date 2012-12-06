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
    public class ClusterLanes : ThreadSupplier<Image<Gray, Byte>, Image<Rgb, Byte>>
    {
        private Supplier<Image<Gray, Byte>> supplier;
        private Image<Rgb, Byte> output;

        private void DrawCluster(Image<Gray, Byte> img)
        {
            output = new Image<Rgb, Byte>(img.Width, img.Height);

            List<Point> lanes = new List<Point>(100);

            byte[,,] raw = img.Data;
            byte[,,] oraw = output.Data;

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

            List<Point> first = new List<Point>(100);
            List<Point> second = new List<Point>(100);
            VisionToolkit.Two_Means_Clustering(lanes, ref first, ref second);

            foreach (var p in first)
            {
                oraw[p.Y, p.X, 0] = 255;
            }

            foreach (var p in second)
            {
                oraw[p.Y, p.X, 1] = 255;
            }

            const int hoffset = 110;
            if (first.Count > 10 && second.Count > 10)
            {
                var one = RANSAC.RANSAC.fit(1000, 11, (int)(first.Count * 0.75), 120, first);
                var two = RANSAC.RANSAC.fit(1000, 11, (int)(second.Count * 0.75), 120, second);
                if (one != null && two != null)
                {
                    if (one.value(img.Height) > two.value(img.Height))
                    {
                        var t = one;
                        one = two;
                        two = one;

                        var tt = first;
                        first = second;
                        second = first;
                    }

                    if (one != null)
                    {
                        for (int y = hoffset; y < img.Height; y += 16)
                        {
                            output.Draw(
                                new CircleF(new PointF((float)one.value(y), (float)y), 3.0f)
                                , new Rgb(210, 140, 183),
                                0);
                        }
                    }

                    if (two != null)
                    {
                        for (int y = hoffset; y < img.Height; y += 16)
                        {
                            output.Draw(
                                new CircleF(new PointF((float)two.value(y), (float)y), 3.0f)
                                , new Rgb(170, 210, 143),
                                0);
                        }
                    }

                    //if (one != null && two != null)
                    {
                        var p = new Parabola(0.5 * (one.a + two.a), 0.5 * (one.b + two.b), 0.5 * (one.c + two.c));
                        for (int y = 0; y < img.Height; y += 8)
                        {
                            output.Draw(
                                new CircleF(new PointF((float)p.value(y), (float)y), 1.0f)
                                , new Rgb(230, 230, 230),
                                0);
                        }
                    }
                }
            }

            LastResult = output;
            PostComplete();
        }
        public ClusterLanes(Supplier<Image<Gray, Byte>> supplier_)
        {
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            Process += DrawCluster;
        }
    }
}
