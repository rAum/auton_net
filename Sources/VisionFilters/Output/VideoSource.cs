using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

using Auton.CarVision.Video;

namespace VisionFilters.Output
{
    public class GrayVideoSource<PixelType> : Supplier<Image<Gray, PixelType>> where PixelType : new()
    {
        public Boolean Runs { get; private set; }

        public override Image<Gray, PixelType> LastResult
        {
            get
            {
                var frame = capture.RetrieveGrayFrame();
                while (frame == null)
                {
                    Load();
                    frame = capture.RetrieveGrayFrame();
                }

                System.Threading.Thread.Sleep(20);

                return frame.Convert<Gray, PixelType>().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            }
        }

        private Capture capture;
        private string file;

        public GrayVideoSource(string _file = "")
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
                (sender, e) => { OnResultReady(new ResultReadyEventArgs<Image<Gray, PixelType>>(LastResult)); };
        }

        public void Start()
        {
            Runs = true;
            capture.Start();
        }

        public void Stop()
        {
            Runs = false;
            capture.Stop();
        }

        public void Pause()
        {
            Runs = false;
            capture.Pause();
        }
    }

    public class ColorVideoSource : Supplier<Image<Bgr, float>>
    {
        public override Image<Bgr, float> LastResult
        {
            get 
            {
                var frame = capture.RetrieveBgrFrame();
                while (frame == null)
                {
                    Load();
                    frame = capture.RetrieveBgrFrame();
                }

                System.Threading.Thread.Sleep(10);

                return frame.Convert<Bgr, float>();
            }
        }

        private Capture capture;
        private string file;

        public ColorVideoSource(string _file = "")
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
                (sender, e) => { OnResultReady(new ResultReadyEventArgs<Image<Bgr, float>>(LastResult)); };
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
