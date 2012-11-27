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
using Emgu;

namespace CarVision
{
    public partial class ViewForm : Form
    {
        VideoSource videoSource;
        LaneMarkDetector laneDetector;
        PerspectiveCorrection perpCorr;
        PerspectiveCorrection invPerpCorr;

        private void DisplayVideo(object sender, CarVision.ResultReadyEventArgs e)
        {
            ImageBox imgBox = null;
            if (sender == videoSource)
                imgBox = imgVideoSource;
            else if (sender == invPerpCorr)
            {
                imgBox = imgSmoothener;
                //TODO remove this, make better
                //Image<Gray, Byte>[] t = { ((Image<Gray, Byte>)e.Result), ((Image<Gray, Byte>)e.Result), ((Image<Gray, Byte>)e.Result) };
                Image<Rgb,Byte> ipc = ((Image<Gray, Byte>)e.Result).Convert<Rgb, Byte>();
                
                Image<Rgb, Byte> cam = new Image<Rgb, Byte>(imgVideoSource.Image.Bitmap);
                imgBox.Image = cam + (ipc - new Rgb(0.0, 255.0, 255.0)); // mix current video frame with founded line marks
                return;
            }
            else if (sender == perpCorr)
                imgBox = imgCanny;
            else return;
            if (imgBox == null)
                throw new InvalidOperationException("No receiver registered");
            imgBox.Image = (Image<Gray, Byte>)e.Result;
            
        }


        public ViewForm()
        {
            InitializeComponent();
            //videoSource = new VideoSource();
            videoSource = new VideoSource(@"C:/film.avi");
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
            
            perpCorr.ResultReady += DisplayVideo;           
         
            laneDetector = new LaneMarkDetector(perpCorr);
            laneDetector.ResultReady += DisplayVideo;

            invPerpCorr = new PerspectiveCorrection(laneDetector, dst, src);
            invPerpCorr.ResultReady += DisplayVideo;
        }

        private void ViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            videoSource.ResultReady -= DisplayVideo;
            videoSource.Stop();
        }
    }
}
