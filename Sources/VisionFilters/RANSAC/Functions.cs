using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RANSAC.Functions
{
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

        /// <summary>
        /// Using Least Squares for fitting quadratic curve.
        /// http://www.efunda.com/math/leastsquares/lstsqr2dcurve.cfm
        /// </summary>
        /// <param name="input">input sample points, must be >= 3</param>
        /// <returns>Best approximation for parabola</returns>
        public static Parabola fit(List<Point> input, int count)
        {
            // Sjk = sum of p.x^j * p.y^k
            float
                s00 = count,
                s01 = 0,
                s10 = 0,
                s11 = 0,
                s21 = 0,
                s20 = 0,
                s30 = 0,
                s40 = 0;
            float x, xx, y;

            Point p;
            for (int i = 0; i < count;  ++i)
            {
                p = input[i];
                x = (float)p.Y;
                y = (float)p.X;
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
            float s20s00_s10s10 = s20 * s00 - s10 * s10;
            float s30s00_s10s20 = s30 * s00 - s10 * s20;
            float s30s10_s20s20 = s30 * s10 - s20 * s20;

            float D = s40 * s20s00_s10s10 - s30 * s30s00_s10s20 + s20 * s30s10_s20s20;
            if (D == 0)
                return null;
            float Da = s21 * s20s00_s10s10 - s11 * s30s00_s10s20 + s01 * s30s10_s20s20; // <- blad!!!!
            float Db = s40 * (s11 * s00 - s01 * s10) - s30 * (s21 * s00 - s01 * s20) + s20 * (s21 * s10 - s11 * s20);
            float Dc = s40 * (s20 * s01 - s10 * s11) - s30 * (s30 * s01 - s10 * s21) + s20 * (s30 * s11 - s20 * s21);

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
