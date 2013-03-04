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
        int sleepTime = 0;

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

                System.Threading.Thread.Sleep(sleepTime);

                return frame.Convert<Gray, PixelType>().Resize(CamModel.Width, CamModel.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
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
            {
                capture = new Capture();
                sleepTime = 0;
            }
            else
                capture = new Capture(file);
            capture.ImageGrabbed +=
                (sender, e) => { OnResultReady(new ResultReadyEventArgs<Image<Gray, PixelType>>(LastResult)); };
        }

        public void RestartVideo()
        {
            if (file != "")
            {
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_POS_FRAMES, 0);
                capture.Start();
            }
        }

        public void ChangeVideoSource(string _file = "")
        {
            capture.Stop();
            System.Threading.Thread.Sleep(1000);
            file = _file;
            Load();
            Start();
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

    public class ColorVideoSource : Supplier<Image<Bgr, byte>>
    {
        public Boolean Runs { get; private set; }
        int sleepTime = 5;

        public override Image<Bgr, byte> LastResult
        {
            get
            {
                var frame = capture.RetrieveBgrFrame();
                while (frame == null)
                {
                    Load();
                    frame = capture.RetrieveBgrFrame();
                }

                System.Threading.Thread.Sleep(sleepTime);

                return frame.Resize(CamModel.Width, CamModel.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
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
            {
                capture = new Capture(); //was empty
                sleepTime = 5;
            }
            else
                capture = new Capture(file);
            capture.ImageGrabbed +=
                (sender, e) => { OnResultReady(new ResultReadyEventArgs<Image<Bgr, byte>>(LastResult)); };
        }

        public void RestartVideo()
        {
            if (file != "")
            {
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_POS_FRAMES, 0);
                capture.Start();
            }
        }

        public void ChangeVideoSource(string _file="")
        {
            capture.Stop();
            System.Threading.Thread.Sleep(1000);
            file = _file;
            Load();
            Start();
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

}
