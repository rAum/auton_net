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
        public double GradientMagnitudeThreshold;

        public int SobelRadius;
        public int SmoothRadius;

        public int MeanRadius;
        public int AveragingMultipiler;
        public double MaxAngle;
        private int NumHistBins;

        public double[] MeanMagnitude;
        public double[] AdaptiveThreshold;
        public double[] AbsMeanMagnitude;

        public List<POI> POIs { get; private set; }

        int cols, rows;

        public POIDetector(Supplier<Image<Gray, float>> supplier, double thr) 
        {
            supplier.ResultReady += MaterialReady;

            // set default parameters
            GradientMagnitudeThreshold = thr;
            SobelRadius = 1;
            SmoothRadius = 4;
            MeanRadius = 5;
            NumHistBins = 50;
            AveragingMultipiler = 2;
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
            int r = rows / 2;
            double[] grad_mags = new double[cols];
            for (int c = 0; c < cols; ++c)
            {
                double x = gx[r, c].Intensity;
                double y = gy[r, c].Intensity;

                double magnitude = Math.Sqrt(x * x + y * y);
                int direction = (x > 0.0) ? 1 : -1;
                grad_mags[c] = magnitude * direction;
            }

            LineSegment2D line = new LineSegment2D(new Point(0, frame.Height / 2), new Point(frame.Width, frame.Height / 2));
            frame.Draw(line, new Bgr(Color.Red), 2);

            DrawFunction(frame, grad_mags, new Bgr(Color.LightGreen));


            foreach (POI p in POIs)
            {
                Point begin = new Point(p.X, p.Y);
                Point end = new Point((int)(p.X + p.GX), (int)(p.Y + p.GY));
                LineSegment2D arrow = new LineSegment2D(begin, end);
                CircleF circle = new CircleF(begin, 5);
                frame.Draw(circle, new Bgr(Color.Blue), 2);
                frame.Draw(arrow, new Bgr(Color.Blue), 1);
            }

            DrawFunction(frame, AbsMeanMagnitude, new Bgr(Color.Green));
            DrawFunction(frame, AdaptiveThreshold, new Bgr(Color.Yellow));  
            
        }

        private void PreprocessImage(Image<Gray, float> gray, out Image<Gray, float> gx, out Image<Gray, float> gy)
        {
            cols = gray.Width;
            MeanMagnitude = new double[cols];
            AdaptiveThreshold = new double[cols];
            AbsMeanMagnitude = new double[cols];
            rows = gray.Height;

            gray = gray.SmoothGaussian(2 * SmoothRadius + 1);
            gx = gray.Sobel(1, 0, 2 * SobelRadius + 1);
            gy = gray.Sobel(0, 1, 2 * SobelRadius + 1);
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

        private List<POI> FindPOI(Image<Gray, float> gx, Image<Gray, float> gy) 
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

                meanWindow[c] = prevMean + mag * sign;
                absMeanWindow[c] = prevAbsMean + mag;
                prevMean = meanWindow[c];
                prevAbsMean = absMeanWindow[c];
            }

            // compute mean magnitude
            CalcMeans(meanWindow, MeanMagnitude, MeanRadius);
            CalcMeans(absMeanWindow, AdaptiveThreshold, MeanRadius * AveragingMultipiler);

            for (int i = 0; i < cols; i++)
                AbsMeanMagnitude[i] = Math.Abs(MeanMagnitude[i]);


            // 

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
