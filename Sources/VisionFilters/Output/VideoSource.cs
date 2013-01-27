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
        int sleepTime = 200;

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

    public class ColorVideoSource<PixelType> : Supplier<Image<Rgb, PixelType>> where PixelType : new()
    {
        public Boolean Runs { get; private set; }
        int sleepTime = 30;

        public override Image<Rgb, PixelType> LastResult
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

                return frame.Convert<Rgb,PixelType>().Resize(CamModel.Width, CamModel.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
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
                sleepTime = 0;
            }
            else
                capture = new Capture(file);
            capture.ImageGrabbed +=
                (sender, e) => { OnResultReady(new ResultReadyEventArgs<Image<Rgb, PixelType>>(LastResult)); };
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
