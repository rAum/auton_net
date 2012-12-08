using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using Auton.CarVision.Video;
using Auton.CarVision.Video.Filters;

using VisionFilters.Output;
using Emgu.CV.UI;
using VisionFilters.Filters.Lane_Mark_Detector;


namespace CarVision
{
    public partial class ViewForm : Form
    {
        GrayVideoSource<byte> videoSource;
        RoadCenterDetector roadDetector;
        VisualiseSimpleRoadModel visRoad;
        PerspectiveCorrectionRgb invPerp;

        private void DisplayVideo(object sender, ResultReadyEventArgs<Image<Gray, Byte>> e)
        {
            ImageBox imgBox = imgDebug;
            if (sender == videoSource)
                imgBox = imgVideoSource;
            if (imgBox == null)
            {
                System.Console.Out.WriteLine("No receiver registered!!");
                return;
            }
            imgBox.Image = (Image<Gray, Byte>)e.Result;
        }

        private void DisplayVideo(object sender, ResultReadyEventArgs<Image<Rgb, Byte>> e)
        {
            ImageBox imgBox = imgDebug;
            if (sender == invPerp)
            {
                imgBox = imgOutput;
                Image<Rgb, Byte> cam = new Image<Rgb, Byte>(imgVideoSource.Image.Bitmap);
                imgBox.Image = (Image<Rgb, Byte>)e.Result + cam;
                return;
            }
            else if (sender == visRoad)
            {
                imgBox = imgDebug;
            }
            if (imgBox == null)
            {
                System.Console.Out.WriteLine("No receiver registered!!");
                return;
            }
            imgBox.Image = (Image<Rgb, Byte>)e.Result;
        }

        public ViewForm()
        {
            InitializeComponent();

            videoSource = new GrayVideoSource<Byte>("");//@"C:/test.avi");
            videoSource.ResultReady += DisplayVideo;

            roadDetector = new RoadCenterDetector(videoSource);

            visRoad = new VisualiseSimpleRoadModel(roadDetector.Perceptor.roadDetector);
            visRoad.ResultReady += DisplayVideo;

            invPerp = new PerspectiveCorrectionRgb(visRoad, roadDetector.Perceptor.dst, roadDetector.Perceptor.src);
            invPerp.ResultReady += DisplayVideo;

            videoSource.Start();
        }

        private void ViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            videoSource.Stop();

            videoSource.ResultReady -= DisplayVideo;
            invPerp.ResultReady -= DisplayVideo;
            visRoad.ResultReady -= DisplayVideo;
        }

        private void ViewForm_Resize(object sender, EventArgs e)
        {
        }

        int next = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            next = (next + 1) % 2;

            if (next == 1)
            {
                visRoad.ResultReady -= DisplayVideo;
                roadDetector.Perceptor.laneDetector.ResultReady += DisplayVideo;
            }
            else
            {
                visRoad.ResultReady += DisplayVideo;
                roadDetector.Perceptor.laneDetector.ResultReady -= DisplayVideo;
            }

        }
    }
}
