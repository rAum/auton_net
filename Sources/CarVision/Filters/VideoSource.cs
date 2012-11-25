using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
//using Emgu.Util;

namespace CarVision.Filters
{
    class VideoSource : Supplier<Image<Gray, Byte>>
    {
        public override Image<Gray, Byte> LastResult
        {
            get { return capture.RetrieveGrayFrame(); }
        }

        private Capture capture;

        public VideoSource()
        {
            capture = new Capture();
            capture.ImageGrabbed += 
                (sender, e) => { OnResultReady(new ResultReadyEventArgs(LastResult)); };
            Start();
        }

        public void Start()
        {
            capture.Start();
        }

        public void Stop()
        {
            capture.Stop();
        }
    }
}
