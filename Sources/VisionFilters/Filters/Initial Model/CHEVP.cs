using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Auton.CarVision.Video;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu;

namespace VisionFilters.InitialModel
{
    ///TODO: implement to estimate horizon and automate finding perspective transform.
    /// <summary>
    /// The CHEVP algorithm - Canny/Hough Estimation of Vanishing Points
    /// ref: "Lane detection and tracking using B-Snake" Yue Wang et al
    /// </summary>
    public class CHEVP : ThreadSupplier<Image<Gray, Byte>, Image<Gray, Byte>>
    {
        private Supplier<Image<Gray, Byte>> supplier;
        private float[] sectionsRatio;
        private int sectionsCount;

        public Gray CannyThreshold { get; set; }
        public Gray CannyThresholdLinking { get; set; }
        public int Threshold { get; set; }

        private void FindRoadInitialEstimate(Image<Gray, Byte> input)
        {
            int[] sections = new int[sectionsCount];
            List<LineSegment2D[]> lines = new List<LineSegment2D[]>(sectionsCount);
            Gray white = new Gray(200);
            var hough = new Image<Gray, byte>(input.Width, input.Height);

            MakeSections(input, sections);
            var img = input.Copy();

            PointF[][] linesInSection = new PointF[sectionsCount][];
            PointF[] gravityCenter = new PointF[sectionsCount];
            int segStart = 0;
            int segEnd;
            for (int i = 0; i < sectionsCount; ++i)
            {
                segEnd = sections[i];
                img.ROI = new Rectangle(0, segStart, img.Width, segEnd - segStart);
                hough.ROI = new Rectangle(0, segStart, hough.Width, segEnd - segStart);

                linesInSection[i] = VisionToolkit.HoughLineTransform(img, Emgu.CV.CvEnum.HOUGH_TYPE.CV_HOUGH_STANDARD, Threshold);
                
                //var mom = img.GetMoments(true);
                //gravityCenter[i].X = (int)mom.GravityCenter.x;
                //gravityCenter[i].Y = (int)mom.GravityCenter.y;
                //hough.Draw(new CircleF(gravityCenter[i], 3), new Gray(180), 0);

                #region bajzel
                PointF avg = new PointF(0,0);
                foreach (PointF line in linesInSection[i])
                {
                    avg.X += line.X;
                    avg.Y += line.Y;

                    hough.Draw(VisionToolkit.ToLineSegment2D(line), new Gray(100), 1);
                }

                if (linesInSection[i].Count() != 0)
                {
                    avg.X /= linesInSection[i].Count();
                    avg.Y /= linesInSection[i].Count();
                    hough.Draw(VisionToolkit.ToLineSegment2D(avg), new Gray(200), 2);
                }


                //System.Console.Out.WriteLine(String.Format("sec #{2}. Line avg theta:{0} rho:{1}", avg.X, avg.Y, i));

                //hough.Draw(ToLineSegment2D(new Point((int)avg.X, (int)avg.Y)), new Gray(100), 2);
                //var ls = ToLineSegment2D(avg);
                #endregion bajzel

                img.ROI = Rectangle.Empty;
                hough.ROI = Rectangle.Empty;

                segStart = segEnd;
            }

            //List<Point> crspline = new List<Point>();
            //Point center = new Point(img.Width / 2, img.Height);
            ////Point center2 = new Point(img.Width / 2, 10);
            //hough.Draw(new CircleF(center, 5), new Gray(255), 0);
            ////hough.Draw(new CircleF(center2, 5), new Gray(255), 0);
            //for (float t = -1.0f; t <= 4f; t += 0.01f)
            //{
            //    crspline.Add(new Point(
            //                catmullrom(center, gravityCenter[7], gravityCenter[0], gravityCenter[0], t),//gravityCenter[5], gravityCenter[1], center2, t),
            //                catmullrom2(center, gravityCenter[7], gravityCenter[0], gravityCenter[0], t)
            //                )
            //            );
            //}
            //hough.DrawPolyline(crspline.ToArray(), false, white, 2);

                #region old
                // for each segment find lines using Hough Transform after Canny
                //int segStart = 0;
                //foreach (int end in sections)
                //{
                //    img.ROI = new Rectangle(0, segStart, img.Width, end-segStart);
                //    lines.Add(
                //        Hough(img, Emgu.CV.CvEnum.HOUGH_TYPE.CV_HOUGH_STANDARD, Threshold).Select(p => ToLineSegment2D(p)).ToArray()
                //    );

                //    segStart = end;
                //    img.ROI = Rectangle.Empty;
                //}


                //segStart = 0;
                //for (int i = 0; i < sectionsCount; ++i)
                //{
                //    segEnd = sections[i];
                //    hough.ROI = new Rectangle(0, segStart, img.Width, segEnd - segStart);
                //    Point origin = new Point(170, 15);
                //    float ang = 0;
                //    foreach (var line in lines[i])
                //    {
                //        PointF dir = line.Direction;
                //        Gray c = new Gray(100);
                //        if (line.GetExteriorAngleDegree(new LineSegment2D(new Point(0,0), new Point(0,100))) < (Math.PI * 0.25))
                //        {
                //            c = new Gray(200);
                //        }

                //        hough.Draw(new LineSegment2D(origin, new Point((int)dir.X * 100 + origin.X, (int)dir.Y * 100 + origin.Y) ), c, 3);
                //        hough.Draw(line, c , 2);
                //    }
                //    segStart = segEnd;
                //    hough.ROI = Rectangle.Empty;
                //}
                #endregion

            LastResult = hough;// +img;
            PostComplete();
        }

        private void MakeSections(Image<Gray, Byte> img2, int[] sections)
        {
            sections[0] = (int)Math.Ceiling(sectionsRatio[0] * img2.Height);
            for (int i = 1; i < sectionsCount; ++i)
            {
                sections[i] = (int)Math.Ceiling(sectionsRatio[i] * img2.Height) + sections[i - 1];
            }
        }

        public CHEVP(Supplier<Image<Gray, Byte>> supplier_)
        {
            supplier = supplier_;
            supplier.ResultReady += MaterialReady;

            Process += FindRoadInitialEstimate;

            sectionsRatio = new float[]
            {
                0.4f,
                0.2f,
                0.1f,
                0.1f,
                0.1f,
                0.1f
            };
            sectionsCount = sectionsRatio.Count();
        }
    }
}
