using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VisionUtils
{
    /// <summary>
    /// CatmullRom Spline 
    /// Points must be sorted [bigger value y first] and equidistant
    /// </summary>
    public class CatmullRom
    {
        PointF[] points;
        int last;
        float range;

        /// <summary>
        /// assuming that points are sorted.
        /// </summary>
        /// <param name="points_"></param>
        public CatmullRom(PointF[] points_)
        {
            points = new PointF[points_.Length];

            points[0] = points_[0];
            for (int i = 0; i < points_.Length; ++i)
            {
                points[i] = points_[i];
            }

            last = points.Length - 1;
            range = points[0].Y - points.Last().Y;
        }

        public PointF p(int i)
        {
            if (i < 0) return points[0];
            else if (i >= points.Length) return points.Last();
            return points[i];
        }

        /// <summary>
        /// y in pixels
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public PointF at(float y)
        {
            float t = (points[0].Y - y) / range;
            if (y < points[last].Y)
                return value(t, points[last - 2], points[last - 1], points[last], points[last]);
            else if (y > points[0].Y)
                return value(t, points[0], points[0], points[1], points[2]);
            //TODO: points must be sorted and equal distance
            t = t * points.Length;
            int s = (int)Math.Floor(t);
            float d = t - s;

            return value(d, p(s - 1), p(s), p(s + 1), p(s + 2));
        }

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
        public static PointF value(float t, PointF a, PointF b, PointF c, PointF d)
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
    }
}
