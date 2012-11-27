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
using CarVision.Filters;
using Emgu.CV.UI;

namespace CarVision
{
    public partial class ViewForm : Form
    {
        VideoSource videoSource;
        Smoothener smoothener;
        LaneMarkDetector laneDetector;
        Canny canny;
        PerspectiveCorrection perpCorr;

        private void DisplayVideo(object sender, CarVision.ResultReadyEventArgs e)
        {
            ImageBox imgBox = null;
            if (sender == videoSource)
                imgBox = imgVideoSource;
            else if (sender == smoothener)
                imgBox = imgSmoothener;
            else if (sender == laneDetector || sender == perpCorr)
                imgBox = imgSmoothener;
            else if (sender == canny)
                imgBox = imgCanny;
            if (imgBox == null)
                throw new InvalidOperationException("No receiver registered");
            imgBox.Image = (Image<Gray, Byte>)e.Result;
            
        }


        public ViewForm()
        {
            InitializeComponent();
            videoSource = new VideoSource();
            videoSource.ResultReady += DisplayVideo;

            //smoothener = new Smoothener(videoSource);
            //smoothener.ResultReady += DisplayVideo;

            laneDetector = new LaneMarkDetector(videoSource);
            //laneDetector.ResultReady += perpCorr.;
            canny = new Canny(smoothener);
            canny.ResultReady += DisplayVideo;

            PointF[] src = { new PointF(0,320), new PointF(320, 320), new PointF(150, 320-240), new PointF(75,320-240) };
            PointF[] dst = { new PointF(0,320), new PointF(320, 320), new PointF(320, 320-240), new PointF(0,320-240) };
            
            perpCorr = new PerspectiveCorrection(laneDetector, src, dst);
            perpCorr.ResultReady += DisplayVideo;


            
        }

        private void ViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            videoSource.ResultReady -= DisplayVideo;
            videoSource.Stop();
        }
    }
}
