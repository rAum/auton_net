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
        public double MaxAngle;

        public POIDetector() 
        { 
            // set default parameters
            GradientMagnitudeThreshold = 2.0;
            MaxAngle = Math.PI / 4;
        }


        public List<POI> FindPOI(Image<Gray, float> gray) 
        { 
            int w = gray.Width;
            int h = gray.Height;
            gray = gray.SmoothGaussian(9);
            Image<Gray, float> gx = gray.Sobel(1, 0, 3);
            Image<Gray, float> gy = gray.Sobel(0, 1, 3);


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
                            int direction = (best_x > 0) ? 1 : 0;
                            if (direction != last_direction)
                            {
                                last_direction = direction;
                                points.Add(new POI(best_c, r, best_x, best_y, best_angle));
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
