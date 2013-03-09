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
    /// </summary>
    public class RoadCenterDetector
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
                1.5f, 
                2.0f,
                2.5f
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

            if (RoadCenterSupply == null)
                return;

            for (int i = 0; i < kalmanFilters.Length; ++i)
                samples[i] = kalmanFilters[i].FeedPoint(samples[i]);

            RoadCenterSupply.Invoke(this, new RoadCenterEvent(samples));
        }
    }
}
