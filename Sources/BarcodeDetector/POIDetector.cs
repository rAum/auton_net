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
            for (int c = 0; c < w; c++) 
            {
                double x = gx[r, c].Intensity;
                double y = gy[r, c].Intensity;
                
                // check if gradient magnitude is big enough
                double magnitude = x * x + y * y;
                if (magnitude < thr)
                    continue;

                // check if angle beetween scanline and gradient isn't too close to 90* 
                double angle = Math.Atan2(y, x);
                double a = Math.Abs(angle);
                if (Math.Abs(a - Math.PI / 2) < Math.PI / 2 - MaxAngle)
                    continue;

                // check if this gradient direction differs from last one
                int direction = (x > 0) ? 1 : 0;
                if (direction == last_direction)
                    continue;

                last_direction = direction;
                points.Add(new POI(c, r, x, y, angle));
            }

            return points;
        }
    }
}
