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
using CarController;
using Helpers;


namespace BrainProject
{
    public partial class ViewForm : Form
    {
        ColorVideoSource colorVideoSource;
        HsvFilter filter;

        RoadCenterDetector roadDetector;
        VisualiseSimpleRoadModel visRoad;
        PerspectiveCorrectionRgb invPerp;

        FollowTheRoadBrainCentre brain;

        DefaultCarController carController;
        MainWindow steeringWindow;

        private delegate void InvokeHandler();

        private void DisplayVideo(object sender, ResultReadyEventArgs<Image<Gray, Byte>> e)
        {
            try
            {
                ImageBox imgBox = imgDebug;
                imgBox.Image = (Image<Gray, Byte>)e.Result;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Bad thing happened in display video :( -" + ex.Message);
            }
        }

        private void DisplayVideo(object sender, ResultReadyEventArgs<Image<Bgr, byte>> e)
        {
            try
            {
                ImageBox imgBox = imgDebug;
                if (sender == invPerp)
                {
                    Image<Bgr, byte> cam = new Image<Bgr, byte>(imgVideoSource.Image.Bitmap);
                    imgOutput.Image = (Image<Bgr, byte>)e.Result + cam;
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
                imgBox.Image = (Image<Bgr, byte>)e.Result;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Bad thing happened in display video :( -" + ex.Message);
            }
        }

        public ViewForm()
        {
            InitializeComponent();

            carController = new CarController.DefaultCarController();
            steeringWindow = new CarController.MainWindow(carController);
            steeringWindow.Show();
            steeringWindow.Activate();
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
            colorVideoSource.Stop();
            colorVideoSource.ResultReady -= DisplayVideo;
            invPerp.ResultReady -= DisplayVideo;
            visRoad.ResultReady -= DisplayVideo;

            // wait a bit
            System.Threading.Thread.Sleep(1500);
        }

        private void ViewForm_Resize(object sender, EventArgs e)
        {
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

        private void CreateVisionProcess(ref SettingsRepository.SettingsRepository setRepo)
        {
            colorVideoSource = new ColorVideoSource();
            colorVideoSource.ResultReady += DisplayVideo;

            Hsv min = new Hsv((int)setRepo.Get("min-h")
                              , (int)setRepo.Get("min-s")
                              , (int)setRepo.Get("min-v")),
                max = new Hsv((int)setRepo.Get("max-h")
                              , (int)setRepo.Get("max-s")
                              , (int)setRepo.Get("max-v"));

            filter = new HsvFilter(colorVideoSource, min, max);
            roadDetector = new RoadCenterDetector(filter);
            // roadDetector.Perceptor.perspectiveTransform.ResultReady += DisplayVideo;
            visRoad = new VisualiseSimpleRoadModel(roadDetector.Perceptor.roadDetector);
            invPerp = new PerspectiveCorrectionRgb(visRoad, CamModel.dstPerspective, CamModel.srcPerspective);

            brain = new FollowTheRoadBrainCentre(roadDetector, carController);
            brain.evNewTargetWheelAngeCalculated += new FollowTheRoadBrainCentre.newTargetWheelAngeCalculatedEventHandler(brain_evNewTargetWheelAngeCalculated);
            brain.evNewTargetSpeedCalculated += new FollowTheRoadBrainCentre.newTargetSpeedCalculatedEventHandler(brain_evNewTargetSpeedCalculated);

            roadDetector.Perceptor.laneDetector.Tau            = (int) setRepo.Get("tau");
            roadDetector.Perceptor.laneDetector.Threshold      = (byte)((int)setRepo.Get("threshold"));
            roadDetector.Perceptor.laneDetector.VerticalOffset = (int) setRepo.Get("v-offset");

            visRoad.ResultReady += DisplayVideo;
            invPerp.ResultReady += DisplayVideo;
            colorVideoSource.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SettingsRepository.SettingsRepository setRepo = new SettingsRepository.SettingsRepository("../../../../config.xml");

            if (!setRepo.Correct)
            {
                MessageBox.Show("Invalid configuration file!\nCheck config.xml file or run tool to generate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CreateVisionProcess(ref setRepo);

            (sender as Button).Enabled = false;
        }
    }
}
