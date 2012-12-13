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
using VisionFilters.Filters.Image_Operations;
using VisionFilters;


namespace BrainProject
{
    public partial class ViewForm : Form
    {
        //GrayVideoSource<byte> videoSource;
        ColorVideoSource<byte> colorVideoSource;
        HsvFilter filter;

        RoadCenterDetector roadDetector;
        VisualiseSimpleRoadModel visRoad;
        PerspectiveCorrectionRgb invPerp;

        FollowTheRoadBrainCentre brain;

        //VideoWriter videoWriter;
        private delegate void InvokeHandler();

        private void DisplayVideo(object sender, ResultReadyEventArgs<Image<Gray, Byte>> e)
        {
            ImageBox imgBox = imgDebug;
            //if (sender == videoSource)
            //    imgBox = imgVideoSource;
            imgBox.Image = (Image<Gray, Byte>)e.Result;
        }

        private void DisplayVideo(object sender, ResultReadyEventArgs<Image<Rgb, Byte>> e)
        {
            ImageBox imgBox = imgDebug;
            if (sender == invPerp)
            {
                Image<Rgb, Byte> cam = new Image<Rgb, Byte>(imgVideoSource.Image.Bitmap);
                imgOutput.Image = (Image<Rgb, Byte>)e.Result + cam;
                return;
            }
            else if (sender == colorVideoSource)
            {
                imgBox = imgVideoSource;
            }
            if (imgBox == null)
            {
                System.Console.Out.WriteLine("No receiver registered!!");
                return;
            }
            imgBox.Image = (Image<Rgb, Byte>)e.Result;
        }

        private Hsv ColorToHsv(Color col)
        {
            Hsv c  = new Hsv(col.GetHue(), col.GetSaturation() * 255, col.GetBrightness()*255);
            return c;
        }

        public ViewForm()
        {
            InitializeComponent();

            //videoSource = new GrayVideoSource<byte>(@"D:/niebieskie.avi");
            //videoSource.ResultReady += DisplayVideo;
            colorVideoSource = new ColorVideoSource<byte>(@"D:\testBlue.avi");
            colorVideoSource.ResultReady += DisplayVideo;

            Hsv minColor = new Hsv(194.0 / 2.0, 0.19 * 255.0, 0.56 * 255.0);
            Hsv maxColor = new Hsv(222.0 / 2.0, 0.61 * 255.0, 0.78 * 255.0);

            filter = new HsvFilter(colorVideoSource, minColor, maxColor, false);
            //filter.ResultReady += DisplayVideo;
            roadDetector = new RoadCenterDetector(filter);
            // roadDetector.Perceptor.perspectiveTransform.ResultReady += DisplayVideo;

            visRoad = new VisualiseSimpleRoadModel(roadDetector.Perceptor.roadDetector);
            visRoad.ResultReady += DisplayVideo;

            invPerp = new PerspectiveCorrectionRgb(visRoad, CamModel.dstPerspective, CamModel.srcPerspective);
            invPerp.ResultReady += DisplayVideo;

            brain = new FollowTheRoadBrainCentre(roadDetector);
            brain.evNewTargetWheelAngeCalculated += new FollowTheRoadBrainCentre.newTargetWheelAngeCalculatedEventHandler(brain_evNewTargetWheelAngeCalculated);

            //videoSource.Start();
            colorVideoSource.Start();
        }

        void brain_evNewTargetWheelAngeCalculated(object sender, double angle)
        {
            Invoke(new Action<Label, double>(
                (labl, val) => labl.Text =  String.Format("{0:0.###}", val.ToString())), 
                label_targetWheelAngle,
                angle
            );
        }

        private void ViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //videoSource.Stop();

            colorVideoSource.Stop();
            colorVideoSource.ResultReady -= DisplayVideo;
            //videoSource.ResultReady -= DisplayVideo;
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

        Color colLower;
        Color colUpper;
        private void button2_Click(object sender, EventArgs e)
        {
            colorDialog.Color = colLower;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                colLower = colorDialog.Color;
            }

            colorDialog.Color = colUpper;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                colUpper = colorDialog.Color;
            }

            filter.lower = ColorToHsv(colLower);
            filter.upper = ColorToHsv(colUpper);
        }

        private void ViewForm_Load(object sender, EventArgs e)
        {

        }
    }
}
