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

        public int MeanRadius;
        public int AveragingMultipiler;
        public double MaxAngle;
        private int NumHistBins;

        public double[] MeanMagnitude;
        public double[] AdaptiveThreshold;

        Image<Gray, float> gx, gy;
        Image<Gray, float> gray;

        int cols, rows;

        public POIDetector(double thr)
        {
            // set default parameters
            GradientMagnitudeThreshold = thr;
            SobelRadius = 1;
            SmoothRadius = 4;
            MeanRadius = 5;
            NumHistBins = 50;
            AveragingMultipiler = 30;
            MaxAngle = Math.PI / 4;
        }

        public void LoadImage(Image<Gray, float> gray)
        {
            cols = gray.Width;
            MeanMagnitude = new double[cols];
            AdaptiveThreshold = new double[cols];
            rows = gray.Height;

            this.gray = gray.SmoothGaussian(2 * SmoothRadius + 1);
            gx = this.gray.Sobel(1, 0, 2 * SobelRadius + 1);
            gy = this.gray.Sobel(0, 1, 2 * SobelRadius + 1);
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


        private double AngleToRight(double angle) 
        {
            if (angle > Math.PI / 2)
                return angle - Math.PI;
            if (angle < -Math.PI / 2)
                return angle + Math.PI;
            return angle;
        }


        private void CalcMeans(double[] partialSums, double [] dst, int r) 
        {
            for (int i = 0; i < partialSums.Length; i++) 
            {
                if (i - r - 1 < 0 || i + r >= partialSums.Length)
                {
                    dst[i] = 0;
                }
                else { 
                    dst[i] = partialSums[r + i] - partialSums[i - r - 1];
                    dst[i] /= 2 * r + 1;
                }
            }
        }


        public List<POI> FindPOI()
        {
            FoundWB = 0;
            FoundBW = 0;

            List<POI> points = new List<POI>();
            int centralRow = rows / 2;
            double thr = GradientMagnitudeThreshold;

            // compute partial sums of scaled angles
            double[] meanWindow = new double[cols];
            double[] absMeanWindow = new double[cols];
            double prevMean = 0;
            double prevAbsMean = 0;
            for (int c = 0; c < cols; c++)
            {
                double x = gx[centralRow, c].Intensity;
                double y = gy[centralRow, c].Intensity;
                double mag = Math.Sqrt(x * x + y * y);
                double sign = (x > 0) ? 1 : -1;

                meanWindow[c] = prevMean + mag*sign;
                prevMean = meanWindow[c];
            }

            // compute mean magnitude
            CalcMeans(meanWindow, MeanMagnitude, MeanRadius);
            CalcMeans(meanWindow, AdaptiveThreshold, MeanRadius * 3);


            return points;
        }
    }
}
