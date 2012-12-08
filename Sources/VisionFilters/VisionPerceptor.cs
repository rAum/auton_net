using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Auton.CarVision.Video;
using Auton.CarVision.Video.Filters;
using VisionFilters.Filters.Lane_Mark_Detector;
using System.Drawing;
using VisionFilters.Output;
using Emgu.CV;
using Emgu.CV.Structure;

namespace VisionFilters
{
    /// <summary>
    /// Event passed when new road model is obtained from measurements.
    /// </summary>
    public class RoadModelEvent : EventArgs
    {
        public SimpleRoadModel model;

        public RoadModelEvent(SimpleRoadModel model_)
        {
            model = model_;
        }
    }

    public delegate void RoadModelEventHandler(object sender, RoadModelEvent e);

    /// <summary>
    /// Main class for doing vision
    /// </summary>
    public class VisionPerceptor
    {
        // this public is for dbg only!!
        public LaneMarkDetector laneDetector;
        public PerspectiveCorrection perspectiveTransform;
        public ClusterLanes roadDetector;

        public PointF[] src;
        public PointF[] dst;

        public event RoadModelEventHandler ActualRoadModel;

        public VisionPerceptor(Supplier<Image<Gray, byte>> input)
        {
            // construct perspective transformation
            src = new PointF[] { 
                    new PointF(116,      108), 
                    new PointF(116 + 88, 108),                                 
                    new PointF(320,     217), 
                    new PointF(0,       217), 
                };
            int offset = 320 / 4;
            dst = new PointF[] { 
                    new PointF(offset,       0), 
                    new PointF(320 - offset, 0), 
                    new PointF(src[2].X - offset, src[2].Y + 33), 
                    new PointF(src[3].X + offset, src[3].Y + 33) 
                };

            ///////////////////////////////////

            perspectiveTransform = new PerspectiveCorrection(input, src, dst);
            laneDetector = new LaneMarkDetector(perspectiveTransform);
            roadDetector = new ClusterLanes(laneDetector);
            roadDetector.ResultReady += PassRoadModel;
        }

        ~VisionPerceptor()
        {
        }

        private void PassRoadModel(object sender, ResultReadyEventArgs<SimpleRoadModel> e)
        {
            SimpleRoadModel model = e.Result as SimpleRoadModel;
            if (model.center != null)
                ActualRoadModel.Invoke(this, new RoadModelEvent(model));
        }

    }
}
