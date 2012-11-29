using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV;
using Emgu.CV.Structure;

using Auton.CarVision.Video;
using System.Drawing;

namespace Auton.CarVision.Video.Filters
{
    public class POIDetector : ThreadSupplier<Image<Gray, float>, Image<Bgr, float>>
    {
        public double GradientMagnitudeThreshold { get; set; }
        public int SobelRadius { get; set; }
        public int SmoothRadius { get; set; }
        public int ScanlineWidth { get; set; }
        public double MaxAngle { get; set; }

        public List<POI> POIs { get; private set; }

        int w, h;

        public POIDetector(Supplier<Image<Gray, float>> supplier, double thr) 
        {
            supplier.ResultReady += MaterialReady;
            // set default parameters
            GradientMagnitudeThreshold = thr;
            SobelRadius = 1;
            SmoothRadius = 4;
            ScanlineWidth = 10;
            MaxAngle = Math.PI / 4;

            Process += ProcessImage;
        }

        private void ProcessImage(Image<Gray, float> source)
        {
            Image<Gray, float> gx, gy;

            Image<Bgr, float> display = source.Convert<Bgr, float>();

            PreprocessImage(source, out gx, out gy);
            POIs = FindPOI(gx, gy);
            DrawGraphs(display, gx, gy);

            LastResult = display;
            PostComplete();
        }

        private void DrawFunction(Image<Bgr, float> frame, double[] ys, Bgr color)
        {
            Point previous = new Point(0, (int)ys[0] + frame.Height / 2);
            
            for (int i = 1; i < frame.Width; ++i)
            {
                Point current = new Point(i, frame.Height / 2 - (int)ys[i]);
                frame.Draw(new LineSegment2D(previous, current), color, 1);
                previous = current;
            }
        }

        private void DrawGraphs(Image<Bgr, float> frame, Image<Gray, float> gx, Image<Gray, float> gy)
        {
            int r = h / 2;
            double[] grad_mags = new double[w];
            for (int c = 0; c < w; ++c)
            {
                double x = gx[r, c].Intensity;
                double y = gy[r, c].Intensity;

                double magnitude = Math.Sqrt(x * x + y * y);
                int direction = (x > 0.0) ? 1 : -1;
                grad_mags[c] = magnitude * direction;
            }

            DrawFunction(frame, grad_mags, new Bgr(Color.LightGreen));
        }

        private void PreprocessImage(Image<Gray, float> gray, out Image<Gray, float> gx, out Image<Gray, float> gy)
        {
            w = gray.Width;
            h = gray.Height;

            gray = gray.SmoothGaussian(2*SmoothRadius + 1);
            gx = gray.Sobel(1, 0, 2*SobelRadius + 1);
            gy = gray.Sobel(0, 1, 2*SobelRadius + 1);
        }

        private double GradientMagnitude(Image<Gray, float> gx, Image<Gray, float> gy, int c, int r)
        {
            int direction = (gx[r, c].Intensity >= 0) ? 1 : -1;
            double x = gx[r, c].Intensity;
            double y = gy[r, c].Intensity;
            double len = Math.Sqrt(x * x + y * y);

            return direction * len;
        }

        public int FoundWB { get; private set; }
        public int FoundBW { get; private set; }
        public int Found { get { return FoundWB + FoundBW; } }

        private List<POI> FindPOI(Image<Gray, float> gx, Image<Gray, float> gy) 
        {
            FoundWB = 0;
            FoundBW = 0;

            List<POI> points = new List<POI>();
            int r = h / 2;
            double thr = GradientMagnitudeThreshold;
            thr = thr * thr;
            int last_direction = 0;

            int best_c = 0;
            double best_c_mag = 0;
            for (int c = 0; c < w; c++) 
            {
                double x = gx[r, c].Intensity;
                double y = gy[r, c].Intensity;
                
                // check if gradient magnitude is big enough
                
                double magnitude = x * x + y * y;
                
                if (magnitude > best_c_mag)
                {
                    best_c_mag = magnitude;
                    best_c = c;
                }

                if (magnitude < thr)
                {
                    if (best_c_mag >= thr)
                    {
                        double best_x = gx[r, best_c].Intensity;
                        double best_y = gy[r, best_c].Intensity;
                        double best_angle = Math.Atan2(y, x);
                        double best_a = Math.Abs(best_angle);
                        if (Math.Abs(best_a - Math.PI / 2) >= Math.PI / 2 - MaxAngle)
                        {
                            int direction = (best_x > 0) ? 1 : -1;
                            if (direction != last_direction)
                            {
                                last_direction = direction;
                                points.Add(new POI(best_c, r, best_x, best_y, best_angle));
                                if (direction > 0)
                                    FoundBW++;
                                else
                                    FoundWB++;
                            }

                        }
                    }

                    best_c_mag = 0;
                }
                
            }

            return points;
        }
    }

    public class POI
    {
        public int X;
        public int Y;
        public double GX;
        public double GY;
        public double Angle;

        public POI(int x, int y, double gx, double gy, double Angle)
        {
            this.X = x;
            this.Y = y;
            this.Angle = Angle;
            this.GY = gy;
            this.GX = gx;
        }
    }
}
