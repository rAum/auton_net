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

        //Kalman2D kalman;

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

            //kalman = new Kalman2D();
        }

        private void NewRoadModel(object sender, RoadModelEvent e)
        {
            RoadCenterFounded(e.model.center);
        }

        /// <summary>
        /// Sample road center and raises event.
        /// </summary>
        /// <param name="roadModel">road center model</param>
        private void RoadCenterFounded(Parabola roadModel)
        {
            PointF[] samples = samplePoints.Select(p => { return new PointF((float)roadModel.value(p), (float)p); }).ToArray();

            //kalman.Update();
            //kalman.Measurment(samples[1]);

            //km.Measurment(samples[1].Y, 1.0f);

            //Console.Out.WriteLine(String.Format("Kalman: meas: {0} pred: {1} estimation: {2}"), samples[1].Y, km.Prediction, km.Estimation);

            if (RoadCenterSupply == null)
                return;

            RoadCenterSupply.Invoke(this, new RoadCenterEvent(samples));
        }
    }
}
