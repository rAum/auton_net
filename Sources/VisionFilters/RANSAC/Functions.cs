using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RANSAC.Functions
{
    public class Vec2
    {
        double[] p = { 0, 0 };

        public double X { get { return p[0]; } set { p[0] = value; } }
        public double Y { get { return p[1]; } set { p[1] = value; } }

        public Vec2()
        {
        }

        public Vec2(double x, double y)
        {
            p[0] = x; p[1] = y;
        }

        public Vec2(int x, int y)
        {
            p[0] = x; p[1] = y;
        }

        public Vec2(Point pt)
        {
            p[0] = pt.X;
            p[1] = pt.Y;
        }

        static Vec2 operator -(Vec2 a, Vec2 b)
        {
            return new Vec2(a.p[0] - b.p[0], a.p[1] - b.p[1]);
        }

        static Vec2 operator +(Vec2 a, Vec2 b)
        {
            return new Vec2(a.p[0] + b.p[0], a.p[1] + b.p[1]);
        }

        static Vec2 operator *(double t, Vec2 a)
        {
            return new Vec2(t * a.p[0], t * a.p[1]);
        }

        public Point ToPoint()
        {
            return new Point((int)Math.Round(p[0]), (int)Math.Round(p[1]));
        }
    }

    public class Bezier
    {
        Vec2[] points;
        double minY, maxY, rangeY;

        public Bezier()
        {
        }

        public static Bezier fit(List<Point> points_)
        {
            Bezier b = new Bezier();
            
            b.points = (from p in points_
                        orderby p.Y // sorting unneeded?
                        select new Vec2(p)).ToArray();
            
            b.minY = b.points[0].Y;
            b.maxY = b.points[b.points.Length - 1].Y;
            b.rangeY = b.maxY - b.minY;

            return b;
        }

        public Vec2 at(int y)
        {
            double t = (y - minY) / rangeY;
            return castelijou(t);
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

        public string ToString()
        {
            StringBuilder bd = new StringBuilder();
            bd.Append('[');
            foreach (var p in points)
                bd.AppendFormat(" ({0}, {1}),", p.X, p.Y);
            bd.Append("]\n");
            return bd.ToString();
        }
    }

    public class Parabola
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

        public Parabola move(double up)
        {
            double bb = -2 * a * up + b;
            double cc = a * up * up - b * up + c;
            return new Parabola(a, bb, cc);
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
}
