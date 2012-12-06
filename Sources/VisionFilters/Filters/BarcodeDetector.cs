using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV;
using Emgu.CV.Structure;

using Auton.CarVision.Video;
using System.Drawing;

using Auton.CarVision.Video.Utils;

namespace Auton.CarVision.Video.Filters
{
    public class BarcodeDetector : ThreadSupplier<Image<Gray, float>, Image<Bgr, float>>
    {
        public double GradientMagnitudeThreshold;

        public int SobelRadius;
        public int SmoothRadius;

        public int MeanRadius;
        public int AveragingMultipiler;
        public double MaxAngle;

        private double[] MeanMagnitude;
        private double[] AdaptiveThreshold;
        private bool[] bwLine;

        private List<int> candidates;

        int cols, rows;

        public BarcodeDetector(Supplier<Image<Gray, float>> supplier, double thr) 
        {
            supplier.ResultReady += MaterialReady;

            // set default parameters
            GradientMagnitudeThreshold = thr;
            SobelRadius = 1;
            SmoothRadius = 4;
            MeanRadius = 5;
            AveragingMultipiler = 2;
            MaxAngle = Math.PI / 4;

            Process += ProcessImage;
        }


        private void ProcessImage(Image<Gray, float> source)
        {
            if (source != null)
            {
                Image<Gray, float> gx, gy;
                Image<Bgr, float> display = source.Convert<Bgr, float>();

                PreprocessImage(source, out gx, out gy);
                FindLineColors(source, gx, gy);
                DrawGraphs(display, gx, gy);

                LastResult = display;

                PostComplete();
            }
            else
                PostFailed();
        }


        private void DrawGraphs(Image<Bgr, float> frame, Image<Gray, float> gx, Image<Gray, float> gy)
        {
            double[] abs = new double[cols];
            for (int i = 0; i < cols; i++)
                abs[i] = Math.Abs(MeanMagnitude[i]);

          
            Drawing.DrawFunction(frame, abs, new Bgr(Color.Green));
            Drawing.DrawFunction(frame, AdaptiveThreshold, new Bgr(Color.Yellow));  

            foreach(int c in candidates)
                Drawing.DrawCircle(frame, c, rows/ 2, 3, Color.Red);

            for (int i = 0; i < cols; i++)
            {
                Color color = bwLine[i] ? Color.White : Color.Black;
                frame[rows / 2 + 5, i] = new Bgr(color);
            }
        }


        private void PreprocessImage(Image<Gray, float> gray, out Image<Gray, float> gx, out Image<Gray, float> gy)
        {
            cols = gray.Width;
            MeanMagnitude = new double[cols];
            AdaptiveThreshold = new double[cols];
            bwLine = new bool[cols];
            rows = gray.Height;

            gray = gray.SmoothGaussian(2 * SmoothRadius + 1);
            gx = gray.Sobel(1, 0, 2 * SobelRadius + 1);
            gy = gray.Sobel(0, 1, 2 * SobelRadius + 1);
        }


        private void CalcMeans(double[] partialSums, double[] dst, int r)
        {
            for (int i = 0; i < partialSums.Length; i++)
            {
                if (i - r - 1 < 0 || i + r >= partialSums.Length)
                {
                    dst[i] = 0;
                }
                else
                {
                    dst[i] = partialSums[r + i] - partialSums[i - r - 1];
                    dst[i] /= 2 * r + 1;
                }
            }
        }

        private void FindLineColors(Image<Gray, float> gray, Image<Gray, float> gx, Image<Gray, float> gy) 
        {
            int centralRow = rows / 2;
            double thr = GradientMagnitudeThreshold;

            // compute partial sums of scaled angles
            double[] meanGradientPartial = new double[cols];
            double[] meanAbsGradientPartial = new double[cols];
            double prevMean = 0;
            double prevAbsMean = 0;
            for (int c = 0; c < cols; c++)
            {
                double x = gx[centralRow, c].Intensity;
                double y = gy[centralRow, c].Intensity;
                double mag = Math.Sqrt(x * x + y * y);
                double sign = (x > 0) ? 1 : -1;

                meanGradientPartial[c] = prevMean + mag * sign;
                meanAbsGradientPartial[c] = prevAbsMean + mag;
                prevMean = meanGradientPartial[c];
                prevAbsMean = meanAbsGradientPartial[c];
            }


            // compute mean magnitude
            CalcMeans(meanGradientPartial, MeanMagnitude, MeanRadius);
            CalcMeans(meanAbsGradientPartial, AdaptiveThreshold, MeanRadius * AveragingMultipiler);


            // find poi candidates
            candidates = new List<int>();
            for (int c = 1; c < cols - 1; c++) {
                if (Math.Abs(MeanMagnitude[c]) <= AdaptiveThreshold[c] + 3)
                    continue;
                if(Math.Abs(MeanMagnitude[c]) <= Math.Max(Math.Abs(MeanMagnitude[c-1]), Math.Abs(MeanMagnitude[c+1])))
                    continue;
                candidates.Add(c);
            }


            // calculate line partial sums
            double[] intensityPartial = new double[cols];
            intensityPartial[0] = gray[centralRow, 0].Intensity;
            for (int c = 1; c < cols; c++)
                intensityPartial[c] = gray[centralRow, c].Intensity + intensityPartial[c - 1];


            // calculate line mean
            double lineMean = intensityPartial[cols - 1] / cols;
            int lastPoint = 0;
            foreach (int x in candidates) { 
                double segmentMean = (intensityPartial[x] - intensityPartial[lastPoint]) / (x - lastPoint);
                bool color = segmentMean > lineMean;
                for (int i = lastPoint; i < x; i++)
                    bwLine[i] = color;
                lastPoint = x;
            }
        }
    }
}
