using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RANSAC.Functions;
using System.Drawing;
using Auton.CarVision.Video;
using Emgu.CV;
using Emgu.CV.Structure;

namespace VisionFilters.Output
{
    /// <summary>
    /// This class provides detection of road center.
    /// 
    /// Also serves as a supplier for Kalman-filtered sample points.
    /// </summary>
    public class RoadCenterDetector: Supplier<List<Point>>
    {
        private int[] samplePoints; // in pixels
        private VisionPerceptor perceptor;
        private KalmanFilter[] kalmanFilters;

        // for dbg purpose
        public VisionPerceptor Perceptor
        {
            get
            {
                return perceptor;
            }
        }

        /// <summary>
        /// event raised when road center is founded.
        /// </summary>
        public event RoadCenterHandler RoadCenterSupply;

        public RoadCenterDetector(Supplier<Image<Gray, byte>> input)
        {
            Setup();

            perceptor = new VisionPerceptor(input);
            perceptor.ActualRoadModel += NewRoadModel;
        }

        private void Setup()
        {
            float[] samplePointsDistance = new float[] // in meters
            {
                0.5f, 
                1.0f,
                1.5f
            };

            samplePoints = samplePointsDistance.Select(p => { return CamModel.ToPixels(p); }).ToArray();
            kalmanFilters = new KalmanFilter[samplePoints.Length];
            for (int i = 0; i < kalmanFilters.Length; ++i)
                kalmanFilters[i] = new KalmanFilter();
        }

        private void NewRoadModel(object sender, RoadModelEvent e)
        {
            RoadCenterFounded(e.model.center);
        }

        /// <summary>
        /// Sample road center and raises event.
        /// </summary>
        /// <param name="roadModel">road center model</param>   
        private void RoadCenterFounded(Function roadModel)
        {
            PointF[] samples = samplePoints.Select(p => { return new PointF((float)roadModel.at(p), (float)p); }).ToArray();

            for (int i = 0; i < kalmanFilters.Length; ++i)
                samples[i] = kalmanFilters[i].FeedPoint(samples[i]);

            List<Point> points = samples.Select(p => { return new Point((int)p.X, (int)p.Y); }).ToList();
            OnResultReady(new ResultReadyEventArgs<List<Point>>(points));

            if (RoadCenterSupply == null)
                return;

            RoadCenterSupply.Invoke(this, new RoadCenterEvent(samples));
        }
    }
}
