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

using Emgu.CV.UI;
using Emgu;
using VisionFilters.Filters.Lane_Mark_Detector;


namespace CarVision
{
    public partial class ViewForm : Form
    {
        GrayVideoSource<Byte> videoSource;
        LaneMarkDetector laneDetector;
        PerspectiveCorrection perpCorr;
        PerspectiveCorrection invPerpCorr;
        PerspectiveCorrectionRgb invPerpColor;
        ClusterLanes cluster;

        private void DisplayVideo(object sender, ResultReadyEventArgs<Image<Gray, Byte>> e)
        {
            ImageBox imgBox = null;
            if (sender == videoSource)
                imgBox = imgVideoSource;
            else if (sender == laneDetector)
            {
                imgBox = imgCanny;
            }
            else if (sender == invPerpCorr)
            {
                imgBox = imgSmoothener;
                Image<Rgb,Byte> ipc = ((Image<Gray, Byte>)e.Result).Convert<Rgb, Byte>();
                
                Image<Rgb, Byte> cam = new Image<Rgb, Byte>(imgVideoSource.Image.Bitmap);
                imgBox.Image = cam + (ipc * 2 - new Rgb(0.0, 255.0, 10.0)); // mix current video frame with founded line marks
                return;
            }
            else if (sender == perpCorr)
                imgBox = imgCanny;
            else return;
            if (imgBox == null)
                throw new InvalidOperationException("No receiver registered");
            imgBox.Image = (Image<Gray, Byte>)e.Result;
            
        }

        private void DisplayVideo(object sender, ResultReadyEventArgs<Image<Rgb, Byte>> e)
        {
            ImageBox imgBox = null;
            if (sender != invPerpColor)
            {
                imgBox = imgCanny;
            }
            else
            {
                imgBox = imgSmoothener;
                Image<Rgb, Byte> cam = new Image<Rgb, Byte>(imgVideoSource.Image.Bitmap);
                imgBox.Image = (Image<Rgb, Byte>)e.Result + cam;
                return;
            }
            imgBox.Image = (Image<Rgb, Byte>)e.Result;
        }

        public ViewForm()
        {
            InitializeComponent();

            videoSource = new GrayVideoSource<Byte>(@"D:/test.avi");
            videoSource.ResultReady += DisplayVideo;
            
            // FIXME: move to better place and enable changes this in runtime [and draw lines/points?]
            PointF[] src = { 
                                new PointF(116,      108), 
                                new PointF(116 + 88, 108),                                 
                                new PointF(320,     217), 
                                new PointF(0,       217), 
                           };

            int offset = 320 / 4;
            PointF[] dst = { 
                                new PointF(offset,       0), 
                                new PointF(320 - offset, 0), 
                                new PointF(src[2].X - offset, src[2].Y + 33), 
                                new PointF(src[3].X + offset, src[3].Y + 33) 
                           };
            
            perpCorr = new PerspectiveCorrection(videoSource, src, dst);
         
            laneDetector = new LaneMarkDetector(perpCorr);

            cluster = new ClusterLanes(laneDetector);
            cluster.ResultReady += DisplayVideo;

            invPerpColor = new PerspectiveCorrectionRgb(cluster, dst, src);
            invPerpColor.ResultReady += DisplayVideo;

            videoSource.Start();
        }

        private void ViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            videoSource.Stop();
            videoSource.ResultReady -= DisplayVideo;
        }
    }
}
