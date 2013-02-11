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

        public event RoadModelEventHandler ActualRoadModel;

        public VisionPerceptor(Supplier<Image<Gray, byte>> input)
        {
            perspectiveTransform = new PerspectiveCorrection(input, CamModel.srcPerspective, CamModel.dstPerspective);
            laneDetector = new LaneMarkDetector(perspectiveTransform);
            //roadDetector = new ClusterLanes(perspectiveTransform);
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
