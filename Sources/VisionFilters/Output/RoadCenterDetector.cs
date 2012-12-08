using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RANSAC.Functions;
using System.Drawing;

namespace VisionFilters.Output
{
    /// <summary>
    /// This class provides detection of road center.
    /// </summary>
    public class RoadCenterDetector
    {
        private int[] samplePoints; // in pixels
        private VisionPerceptor perceptor;

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

        public RoadCenterDetector(GrayVideoSource<byte> videoSource)
        {
            float[] samplePointsDistance = new float[] // in meters
            {
                0.5f, 
                1.0f, 
                1.5f
            };

            samplePoints = samplePointsDistance.Select(p => { return CamModel.ToPixels(p); }).ToArray();

            perceptor = new VisionPerceptor(videoSource);
            perceptor.ActualRoadModel += NewRoadModel;
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

            System.Console.Write("New measurements: ");
            foreach (var p in samples)
            {
                System.Console.Out.Write(p + " ");
            }
            System.Console.Out.WriteLine();

            if (RoadCenterSupply == null)
            {
                //Helpers.Logger.Log(this, "Nobody is waiting for road center.", 5);
                return;
            }
            RoadCenterSupply.Invoke(this, new RoadCenterEvent(samples));
        }
    }
}
