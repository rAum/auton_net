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
        private double[] samplePoints;
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
            samplePoints = new double[]
            {
                0.9, 0.8, 0.7
            };

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
            if (RoadCenterSupply == null)
            {
                System.Console.Out.WriteLine("Nobody is waiting for road center.");
                //Helpers.Logger.Log(this, "Nobody is waiting for road center.", 5);
                return;
            }

            RoadCenterSupply.Invoke(this, new RoadCenterEvent(
                samplePoints.Select( sample => 
                { 
                    return new PointF((float)roadModel.value(sample), (float)sample); 
                }).ToArray())
            );
        }
    }
}
