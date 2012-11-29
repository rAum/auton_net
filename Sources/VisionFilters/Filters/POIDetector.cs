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

        private double[] MeanMagnitude;
        private double[] AdaptiveThreshold;

        public List<POI> POIs { get; private set; }
        private List<int> candidates;

        int cols, rows;
        public int PerpendicularCheckDepth;

        public POIDetector(Supplier<Image<Gray, float>> supplier, double thr) 
        {
            supplier.ResultReady += MaterialReady;

            // set default parameters
            GradientMagnitudeThreshold = thr;
            SobelRadius = 1;
            SmoothRadius = 4;
            MeanRadius = 5;
            AveragingMultipiler = 2;
            MaxAngle = Math.PI / 4;
            PerpendicularCheckDepth = 20;

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


        private void DrawCircle(Image<Bgr, float> frame, int c, float radius, Color color)
        {
            int r = rows / 2;

            CircleF circle = new CircleF(new PointF(c, r), radius);

            frame.Draw(circle, new Bgr(color), 2);
        }


        private void DrawGraphs(Image<Bgr, float> frame, Image<Gray, float> gx, Image<Gray, float> gy)
        {
            double[] abs = new double[cols];
            for (int i = 0; i < cols; i++)
                abs[i] = Math.Abs(MeanMagnitude[i]);
            
            DrawFunction(frame, abs, new Bgr(Color.Green));
            DrawFunction(frame, AdaptiveThreshold, new Bgr(Color.Yellow));  

            foreach(int c in candidates)
                DrawCircle(frame, c, 3, Color.Red);

            foreach (POI p in POIs)
                DrawCircle(frame, p.X, 3, Color.Magenta);
        }


        private void PreprocessImage(Image<Gray, float> gray, out Image<Gray, float> gx, out Image<Gray, float> gy)
        {
            cols = gray.Width;
            MeanMagnitude = new double[cols];
            AdaptiveThreshold = new double[cols];
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


            // find poi candidates
            candidates = new List<int>();
            for (int c = 1; c < cols - 1; c++) {
                if (Math.Abs(MeanMagnitude[c]) <= AdaptiveThreshold[c] + 3)
                    continue;
                if(Math.Abs(MeanMagnitude[c]) <= Math.Max(Math.Abs(MeanMagnitude[c-1]), Math.Abs(MeanMagnitude[c+1])))
                    continue;
                candidates.Add(c);
            }

            // check each one 
            POIs = new List<POI>();
            foreach(int c in candidates){
                double x = gx[centralRow, c].Intensity;
                double y = gy[centralRow, c].Intensity;
                double len = Math.Abs(MeanMagnitude[c]);

                double perp_x = y / len;
                double perp_y = -x / len;
                
                // average x along this line
                double mean_gx = 0;
                double mean_gy = 0;
                int n = 0;
                for (int i = 0; i < PerpendicularCheckDepth; i++) {
                    int prow = (int) perp_y * i + centralRow;
                    int pcol = (int) perp_x * i + c; 
                    if(prow >= 0 && prow < rows && pcol > 0 && pcol < cols)
                    {
                        n++;
                        mean_gx += gx[prow, pcol].Intensity;
                        mean_gy += gy[prow, pcol].Intensity;
                    }
                }
                mean_gx /= n;
                mean_gx /= n;

                if(Math.Max(Math.Abs(mean_gx - x), Math.Abs(mean_gy - y)) < 400){
                    POIs.Add(new POI(c, centralRow, x, y, Math.Atan2(y, x)));
                }
            }

            return POIs;
        }
    }




    public class POI
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public double GX { get; private set; }
        public double GY { get; private set; }
        public double Angle { get; private set; }

        public bool IntoDark
        {
            get
            {
                return X < 0;
            }
        }

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
