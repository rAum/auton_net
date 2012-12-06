using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;


namespace Auton.CarVision.Video.Utils
{
    public class Drawing
    {
        public static void DrawFunction(Image<Bgr, float> frame, double[] ys, Bgr color)
        {
            Point previous = new Point(0, (int)ys[0] + frame.Height / 2);

            for (int i = 1; i < frame.Width; ++i)
            {
                Point current = new Point(i, frame.Height / 2 - (int)ys[i]);
                frame.Draw(new LineSegment2D(previous, current), color, 1);
                previous = current;
            }
        }


        public static void DrawCircle(Image<Bgr, float> frame, int x, int y, float radius, Color color)
        {
            CircleF circle = new CircleF(new PointF(x, y), radius);
            frame.Draw(circle, new Bgr(color), 2);
        }
    }
}
