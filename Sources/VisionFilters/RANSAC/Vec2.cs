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

        public static Vec2 operator -(Vec2 a, Vec2 b)
        {
            return new Vec2(a.p[0] - b.p[0], a.p[1] - b.p[1]);
        }

        public static Vec2 operator +(Vec2 a, Vec2 b)
        {
            return new Vec2(a.p[0] + b.p[0], a.p[1] + b.p[1]);
        }

        public static Vec2 operator *(double t, Vec2 a)
        {
            return new Vec2(t * a.p[0], t * a.p[1]);
        }

        public Point ToPoint()
        {
            return new Point((int)Math.Round(p[0]), (int)Math.Round(p[1]));
        }
    }
}
