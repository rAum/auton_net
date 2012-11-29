using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

using Auton.CarVision.Video;

namespace Auton.CarVision.Video
{
    public class VideoSource : Supplier<Image<Gray, Byte>>
    {
        public override Image<Gray, Byte> LastResult
        {
            get 
            {
                var frame = capture.RetrieveGrayFrame();
                while (frame == null)
                {
                    Load();
                    frame = capture.RetrieveGrayFrame();
                }

                System.Threading.Thread.Sleep(30);

                return frame;
            }
        }

        private Capture capture;
        private string file;

        public VideoSource(string _file = "")
        {
            file = _file;
            Load();
        }

        private void Load()
        {
            if (file == "")
                capture = new Capture();
            else
                capture = new Capture(file);
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
