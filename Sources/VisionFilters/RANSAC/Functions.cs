using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using VisionFilters;

namespace RANSAC.Functions
{
    public interface Function
    {
        double at(double y);
        string ToString();
        void moveHorizontal(double offset);
    }

    public class Bezier : Function
    {
        Vec2[] points;
        double minY, maxY, rangeY;

        public Bezier()
        {
        }

        public Bezier(Vec2[] points_)
        {
            points = points_.ToArray();
            findRange(this);
        }

        public static Bezier fit(List<Point> points_, int count)
        {
            Bezier b = new Bezier();

            b.points = new Vec2[count];
            b.minY = 10000;
            b.maxY = 0;
            for (int i = 0; i < count; ++i)
            {
                b.points[i] = new Vec2(points_[i]);
                //if (points_[i].Y < b.minY) b.minY = points_[i].Y;
                //else if (points_[i].Y > b.maxY) b.maxY = points_[i].Y;
            }
            //b.rangeY = b.maxY - b.minY;

            findRange(b);

            return b;
        }

        public void moveHorizontal(double offset)
        {
            for (int i = 0; i < points.Length; ++i)
                points[i].X += offset;
        }

        public static Bezier Moved(Bezier old, double offset)
        {
            Bezier b = new Bezier(old.points);
            b.moveHorizontal(offset);
            return b;
        }

        private static void findRange(Bezier b)
        {
            b.minY = b.points[0].Y;
            b.maxY = b.points[b.points.Length - 1].Y;
            b.rangeY = b.maxY - b.minY;
        }

        public double at(double y)
        {
            double t = (y - minY) / rangeY;
            return castelijou(t).X;
        }

        public int Count { get { return points.Length; } }

        /// <summary>
        /// a.Count == b.Count !!!!!!!
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Bezier merge(Bezier a, Bezier b, double t = 0.5)
        {
            Vec2[] points = new Vec2[a.Count];
            for (int i = 0; i < a.Count; ++i)
                points[i] = t * (a.points[i] + b.points[i]);
            return new Bezier(points);
        }

        private Vec2 castelijou(double t)
        {
            Vec2[] c = points.ToArray();
            int n = c.Length - 1;
            for (int i = 0; i < c.Length; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    c[j] = c[j] + t * (c[j + 1] - c[j]);
                }
                --n;
            }

            return c[0];
        }

        public override string ToString()
        {
            StringBuilder bd = new StringBuilder();
            bd.Append('[');
            foreach (var p in points)
                bd.AppendFormat(" ({0}, {1}),", p.X, p.Y);
            bd.Append("]\n");
            return bd.ToString();
        }
    }

    public class Parabola : Function
    {
        public double a { get; set; }
        public double b { get; set; }
        public double c { get; set; }

        public Parabola(double A = 0, double B = 0, double C = 0)
        {
            a = A;
            b = B;
            c = C;
        }

        public override string ToString()
        {
            return String.Format("{0}x^2 + {1}x + {2}", a, b, c);
        }

        /// <summary>
        /// Returns average distance sampled from 4 points
        /// </summary>
        /// <param name="a">first parabola</param>
        /// <param name="b">second parabola</param>
        /// <returns>average distance</returns>
        public static double distance(Parabola a, Parabola b)
        {
            double sum = 0;
            for (double x = 10; x <= 90; x += 20)
                sum += a.at(x) - b.at(x);
            return sum * 0.25;
        }

        public double deriv(double x)
        {
            return 2.0 * x * a + b;
        }

        public Parabola move_this(double up)
        {
            double bb = -2 * a * up + b;
            double cc = a * up * up - b * up + c;
            b = bb;
            c = cc;
            return this;
        }

        public static Parabola merge(Parabola a, Parabola b, double t = 0.5)
        {
            return new Parabola((a.a + b.a) * t,
                                (a.b + b.b) * t,
                                (a.c + b.c) * t);
        }

        public Parabola move(double up)
        {
            double bb = -2 * a * up + b;
            double cc = a * up * up - b * up + c;
            return new Parabola(a, bb, cc);
        }

        public void moveHorizontal(double offset)
        {
            c += offset;
        }

        public static Parabola Moved(Parabola old, double offset)
        {
            Parabola b = new Parabola(old.a, old.b, old.c);
            b.moveHorizontal(offset);
            return b;
        }

        /// <summary>
        /// Using Least Squares for fitting quadratic curve.
        /// http://www.efunda.com/math/leastsquares/lstsqr2dcurve.cfm
        /// </summary>
        /// <param name="input">input sample points, must be >= 3</param>
        /// <returns>Best approximation for parabola</returns>
        public static Parabola fit(List<Point> input, int count)
        {
            // Sjk = sum of p.x^j * p.y^k
            decimal
                s00 = count,
                s01 = 0,
                s10 = 0,
                s11 = 0,
                s21 = 0,
                s20 = 0,
                s30 = 0,
                s40 = 0;
            decimal x, xx, y;

            Point p;
            for (int i = 0; i < count;  ++i)
            {
                p = input[i];
                x = (decimal)p.Y;
                y = (decimal)p.X;
                xx = x * x;
                //s00 += 1; //s00 = input.Count
                s10 += x;
                s20 += xx;
                s30 += xx * x;
                s40 += xx * xx;

                s21 += xx * y;
                s11 += x * y;
                s01 += y;
            }

            // Cramer method:    
            decimal s20s00_s10s10 = s20 * s00 - s10 * s10;
            decimal s30s00_s10s20 = s30 * s00 - s10 * s20;
            decimal s30s10_s20s20 = s30 * s10 - s20 * s20;

            decimal D = s40 * s20s00_s10s10 - s30 * s30s00_s10s20 + s20 * s30s10_s20s20;
            if (D == 0)
                return null;
            decimal Da = s21 * s20s00_s10s10 - s11 * s30s00_s10s20 + s01 * s30s10_s20s20; // <- blad!!!!
            decimal Db = s40 * (s11 * s00 - s01 * s10) - s30 * (s21 * s00 - s01 * s20) + s20 * (s21 * s10 - s11 * s20);
            decimal Dc = s40 * (s20 * s01 - s10 * s11) - s30 * (s30 * s01 - s10 * s21) + s20 * (s30 * s11 - s20 * s21);

            return new Parabola((double)(Da / D), (double)(Db / D), (double)(Dc / D));
        }

        public double at(double x)
        {
            return value(x);
        }
        /// <summary>
        /// Returns value at point x using Horner method.
        /// </summary>
        /// <param name="x">value of x</param>
        /// <returns>f(x)</returns>
        public double value(double x)
        {
            return ((a * x) + b) * x + c;
        }
    }


    /// <summary>
    /// CatmullRom Spline 
    /// Points must be sorted [bigger value y first] and equidistant
    /// </summary>
    public class CatmullRom : Function
    {
        Vec2[] points;
        double[] ylen;
        int n;

        public CatmullRom(Vec2[] points_)
        {
            points = points_;
            Array.Sort(points);

            n = points.Length;
            ylen = new double[n];
            for (int i = 0; i < n; ++i)
            {
                ylen[i] = points[i].Y / CamModel.Height; // TODO: optimalize [look at "at"]
            }
        }

        public Vec2 get(int i)
        {
            if (i < 0) i = 0;
            else if (i >= n) i = n-1;

            return points[i];
        }

        /// <summary>
        /// TODO: Optimalize 
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public double at(double y)
        {
            y /= CamModel.Height;
            int seg = 0;
            while (seg < (n-1) && ylen[seg + 1] < y) 
                ++seg;
            double t;
            if (seg == n - 1)
                t = (y - points[seg].Y / 480);
            else t = (y - ylen[seg]) / (ylen[Math.Min(seg+1,n-1)] - ylen[seg]);

            return value(t, get(seg - 1), points[seg], get(seg + 1), get(seg + 2)).X;
        }

        public static CatmullRom merge(CatmullRom a, CatmullRom b, double t = 0.5)
        {
            int jump = CamModel.Height / 8;
            Vec2[] p = new Vec2[8];
            for (int i = 0; i < 8; ++i)
            {
                double j = (i+1) * jump;
                p[i] = new Vec2((a.at(j) + b.at(j)) * t, j);
            }

            return new CatmullRom(p);
        }

        public void moveHorizontal(double off)
        {
            for (int i = 0; i < n; ++i)
                points[i].X += off;
        }

        public static CatmullRom Moved(CatmullRom cm, double offset)
        {
            CatmullRom cr = new CatmullRom(cm.points.ToArray());
            cr.moveHorizontal(offset);
            return cr;
        }


        public static CatmullRom fit(List<Point> points_, int count)
        {
            Vec2[] pt = new Vec2[count];
            for (int i = 0; i < count; ++i)
                pt[i] = new Vec2(points_[i]);
            return new CatmullRom(pt);
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
        public static Vec2 value(double t, Vec2 a, Vec2 b, Vec2 c, Vec2 d)
        {
            double c0 =  t * ((2.0 - t) * t - 1.0);
            double c1 = (t * t * (3.0 * t - 5.0) + 2.0);
            double c2 =  t * ((4.0 - 3.0 * t) * t + 1.0);
            double c3 = (t - 1.0) * t * t;
            return 0.5 * (c0 * a + c1 * b + c2 * c + c3 * d);
        }
    }
}
