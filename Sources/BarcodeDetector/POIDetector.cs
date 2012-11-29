using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV;
using Emgu.CV.Structure;


namespace BarcodeDetector
{
    class POIDetector
    {
        public double GradientMagnitudeThreshold;
        public int SobelRadius;
        public int SmoothRadius;
        public double MaxAngle;

        Image<Gray, float> gx, gy;
        Image<Gray, float> gray;

        int w, h;

        public POIDetector(double thr) 
        {
            // set default parameters
            GradientMagnitudeThreshold = thr;
            SobelRadius = 1;
            SmoothRadius = 4;
            MaxAngle = Math.PI / 4;
        }

        public void LoadImage(Image<Gray, float> gray)
        {
            w = gray.Width;
            h = gray.Height;

            this.gray = gray.SmoothGaussian(2*SmoothRadius + 1);
            gx = this.gray.Sobel(1, 0, 2*SobelRadius + 1);
            gy = this.gray.Sobel(0, 1, 2*SobelRadius + 1);
        }

        public double GradientMagnitude(int c, int r)
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

        public List<POI> FindPOI() 
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
}
