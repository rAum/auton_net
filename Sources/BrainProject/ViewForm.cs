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
using autonomiczny_samochod;


namespace BrainProject
{
    public partial class ViewForm : Form
    {
        ColorVideoSource<byte> colorVideoSource;
        HsvFilter filter;

        RoadCenterDetector roadDetector;
        VisualiseSimpleRoadModel visRoad;
        PerspectiveCorrectionRgb invPerp;

        FollowTheRoadBrainCentre brain;

        CarController carController;
        MainWindow steeringWindow;

        //VideoWriter videoWriter;
        private delegate void InvokeHandler();

        private void DisplayVideo(object sender, ResultReadyEventArgs<Image<Gray, Byte>> e)
        {
            ImageBox imgBox = imgDebug;
            imgBox.Image = (Image<Gray, Byte>)e.Result;
        }

        private void DisplayVideo(object sender, ResultReadyEventArgs<Image<Rgb, Byte>> e)
        {
            ImageBox imgBox = imgDebug;
            if (sender == invPerp)
            {
                Image<Rgb, Byte> cam = new Image<Rgb, Byte>(imgVideoSource.Image.Bitmap);
                imgOutput.Image = (Image<Rgb, Byte>)e.Result + cam * 0.6;
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

            carController = new autonomiczny_samochod.CarController();
            steeringWindow = new autonomiczny_samochod.MainWindow(carController);
            steeringWindow.Show();
            steeringWindow.Activate();

            colorVideoSource = new ColorVideoSource<byte>();//@"D:\testBlue.avi");
            colorVideoSource.ResultReady += DisplayVideo;

            Hsv minColor = new Hsv(95 / 2, 0.6 * 255, 0.5 * 255);
            Hsv maxColor = new Hsv(180 / 2, 255, 0.74 * 255);

            filter = new HsvFilter(colorVideoSource, minColor, maxColor);
            //filter.ResultReady += DisplayVideo;
            roadDetector = new RoadCenterDetector(filter);
            // roadDetector.Perceptor.perspectiveTransform.ResultReady += DisplayVideo;

            visRoad = new VisualiseSimpleRoadModel(roadDetector.Perceptor.roadDetector);
            visRoad.ResultReady += DisplayVideo;

            invPerp = new PerspectiveCorrectionRgb(visRoad, CamModel.dstPerspective, CamModel.srcPerspective);
            invPerp.ResultReady += DisplayVideo;

            brain = new FollowTheRoadBrainCentre(roadDetector, carController);
            brain.evNewTargetWheelAngeCalculated += new FollowTheRoadBrainCentre.newTargetWheelAngeCalculatedEventHandler(brain_evNewTargetWheelAngeCalculated);
            brain.evNewTargetSpeedCalculated += new FollowTheRoadBrainCentre.newTargetSpeedCalculatedEventHandler(brain_evNewTargetSpeedCalculated);

            //videoSource.Start();
            colorVideoSource.Start();
        }


        void brain_evNewTargetSpeedCalculated(object sender, double speed)
        {
            Invoke(new Action<Label, double>(
                (labl, val) => labl.Text = String.Format("{0:0.###}", val)),
                label_TargetSpeed,
                speed
            );

            if (automaticSteeringEnabled)
            {
                carController.SetTargetSpeed(speed);
            }
        }

        void brain_evNewTargetWheelAngeCalculated(object sender, double angle)
        {
            Invoke(new Action<Label, double>(
                (labl, val) => labl.Text = String.Format("{0:0.###}", val)), 
                label_targetWheelAngle,
                angle
            );

            if (automaticSteeringEnabled)
            {
                carController.SetTargetWheelAngle(angle);
            }
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

        private bool automaticSteeringEnabled = false;
        private void button_EnableAutomaticSteering_Click(object sender, EventArgs e)
        {
            if (automaticSteeringEnabled)
            {
                automaticSteeringEnabled = false;
                button_EnableAutomaticSteering.Text = "Enable automatic steering";
            }
            else
            {
                automaticSteeringEnabled = true;
                button_EnableAutomaticSteering.Text = "Disable automatic steering";
            }
        }
    }
}
