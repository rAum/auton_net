using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace VisionFilters
{
    /// <summary>
    /// Set of some handy functions which share nothing in common
    /// yet it's good to have them.
    /// </summary>
    public class VisionToolkit
    {
        /// <summary>
        /// CatmullRom spline.
        /// It's passing through b and c points.
        /// Point a and d is been used to determine tangents.
        /// </summary>
        /// <param name="t">[0,1]</param>
        /// <param name="a">first control (tangent) point</param>
        /// <param name="b">first point [interpolated]</param>
        /// <param name="c">second point [interpolated]</param>
        /// <param name="d">second control (tangent) point</param>
        /// <returns></returns>
        public static PointF CatmullRom(float t, PointF a, PointF b, PointF c, PointF d)
        {
            float c0 = t * ((2.0f - t) * t - 1.0f);
            float c1 = (t * t * (3.0f * t - 5.0f) + 2.0f);
            float c2 = t * ((4.0f - 3.0f * t) * t + 1.0f);
            float c3 = (t - 1.0f) * t * t;
            return new PointF(
                0.5f * (c0 * a.X + c1 * b.X + c2 * c.X + c3 * d.X),
                0.5f * (c0 * a.Y + c1 * b.Y + c2 * c.Y + c3 * d.Y)
                );
        }

        /// <summary>
        /// Converts line in angle/distance [theta/rho] representation to vector (line segment) form which can be drawn.
        /// </summary>
        /// <param name="p">line </param>
        /// <param name="offseting">half of vector size</param>
        /// <returns>segment of line</returns>
        public static LineSegment2D ToLineSegment2D(PointF p, float offseting = 1000)
        {
            float rho = p.X;
            float theta = p.Y;
            double a = Math.Cos(theta), b = Math.Sin(theta);
            double x0 = a * rho, y0 = b * rho;
            return new LineSegment2D(
                new Point((int)Math.Round(x0 + offseting * (-b)), (int)Math.Round(y0 + offseting * (a))),
                new Point((int)Math.Round(x0 - offseting * (-b)), (int)Math.Round(y0 - offseting * (a)))
                );
        }

        /// <summary>
        /// Hough Line Transform, as in OpenCV (EmguCv does not wrap this function as it should be)
        /// </summary>
        /// <param name="img">Binary image</param>
        /// <param name="type">type of hough transform</param>
        /// <param name="threshold">how many votes is needed to accept line</param>
        /// <returns>Lines in theta/rho format</returns>
        public static PointF[] HoughLineTransform(Image<Gray, byte> img, Emgu.CV.CvEnum.HOUGH_TYPE type, int threshold)
        {
            using (MemStorage stor = new MemStorage())
            {
                IntPtr linePtr = CvInvoke.cvHoughLines2(img, stor.Ptr, type, 5, Math.PI / 180 * 15, threshold, 0, 0);
                Seq<PointF> seq = new Seq<PointF>(linePtr, stor);
                return seq.ToArray(); ;
            }
        }

        /// <summary>
        /// Running k-means algorithm with k-means++ initial algorithm for k = 2.
        /// Can be used to find right and left lane boundary.
        /// </summary>
        /// <param name="input">Input points</param>
        /// <param name="a">First lane</param>
        /// <param name="b">Second lane</param>
        public static void Two_Means_Clustering(List<Point> input, ref List<Point> first, ref List<Point> second, int attempts = 3)
        {
            if (input.Count < 7)
                return;

            // formatting input data
            float[,] samples = new float[input.Count, 2];
            int i = 0;
            foreach (var p in input)
            {
                samples[i, 0] = p.X;
                samples[i, 1] = p.Y;
                ++i;
            }

            MCvTermCriteria term = new MCvTermCriteria();

            Matrix<float> samplesMatrix = new Matrix<float>(samples);
            Matrix<Int32> labels = new Matrix<Int32>(input.Count, 1);

            CvInvoke.cvKMeans2(samplesMatrix, 2, labels, term, attempts, IntPtr.Zero, KMeansInitType.RandomCenters, IntPtr.Zero, IntPtr.Zero);

            first.Clear();
            second.Clear();
            for (i = 0; i < input.Count; ++i)
            {
                if (labels[i, 0] == 0)
                    first.Add(input[i]);
                else
                    second.Add(input[i]);
            }
        }
    }
}
