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

namespace CarVision
{
    public partial class ViewForm : Form
    {
        ColorVideoSource<byte> colorVideoSource;
        HsvFilter filter;

        RoadCenterDetector roadDetector;
        VisualiseSimpleRoadModel visRoad;
        PerspectiveCorrectionRgb invPerp;

        VideoWriter videoWriter;

        const string sourceInput = //@"C:/video/rec_2012-12-14_10_40_442.avi";
                                   @"C:/video/rec_2013-03-02_18_15_239.avi";

        // perspektywa
        //180 cm od niebieskiej
        //179 cm

        private void DisplayVideo(object sender, ResultReadyEventArgs<Image<Gray, Byte>> e)
        {
            ImageBox imgBox = imgDebug;
            imgBox.Image = (Image<Gray, Byte>)e.Result;
        }

        private void DisplayVideo(object sender, ResultReadyEventArgs<Image<Rgb, Byte>> e)
        {
            try
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
                if (videoWriter != null && sender == colorVideoSource)
                {
                    videoWriter.WriteFrame(((Image<Rgb, Byte>)e.Result).Convert<Bgr, byte>());
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Bad thing happened in display video :( -" + ex.Message);
            }
        }

        private Hsv ColorToHsv(Color col)
        {
            Hsv c  = new Hsv(col.GetHue(), col.GetSaturation() * 255, col.GetBrightness()*255);
            return c;
        }

        public ViewForm()
        {
            InitializeComponent();

            colorVideoSource = new ColorVideoSource<byte>(sourceInput);
            colorVideoSource.ResultReady += DisplayVideo;

            //Hsv minColor = new Hsv(194.0 / 2.0, 0.19 * 255.0, 0.56 * 255.0);
            //Hsv maxColor = new Hsv(222.0 / 2.0, 0.61 * 255.0, 0.78 * 255.0);

            Hsv minColor = new Hsv(150.0 / 2.0, 0.02 * 255.0, 0.7 * 255.0);
            Hsv maxColor = new Hsv(242.0 / 2.0, 0.19 * 255.0, 1.0 * 255.0);

            // light green lines
            //Hsv minColor = new Hsv(95 / 2, 0.6 * 255, 0.5 * 255);
            //Hsv maxColor = new Hsv(180 / 2, 255, 0.74 * 255);

            filter = new HsvFilter(colorVideoSource, minColor, maxColor);
            //filter.ResultReady += DisplayVideo;
            roadDetector = new RoadCenterDetector(filter);
           // roadDetector.Perceptor.perspectiveTransform.ResultReady += DisplayVideo;

            visRoad = new VisualiseSimpleRoadModel(roadDetector.Perceptor.roadDetector);
            visRoad.ResultReady += DisplayVideo;

            invPerp = new PerspectiveCorrectionRgb(visRoad, CamModel.dstPerspective, CamModel.srcPerspective);
            //invPerp = new PerspectiveCorrectionRgb(colorVideoSource, CamModel.srcPerspective, CamModel.dstPerspective);
            invPerp.ResultReady += DisplayVideo;

            colorVideoSource.Start();
        }

        private void ViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            colorVideoSource.Stop();
            colorVideoSource.ResultReady -= DisplayVideo;
            invPerp.ResultReady -= DisplayVideo;
            visRoad.ResultReady -= DisplayVideo;

            // wait a bit...
            System.Threading.Thread.Sleep(1000);
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
                //roadDetector.Perceptor.laneDetector.ResultReady += DisplayVideo;
            }
            else
            {
                visRoad.ResultReady += DisplayVideo;
                //roadDetector.Perceptor.laneDetector.ResultReady -= DisplayVideo;
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

        private void button3_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;

            if (b.Text == "REC")
            {
                string filename = String.Format("C:/video/rec_{0}_{1}_{2}.avi", DateTime.Now.ToShortDateString(),
                DateTime.Now.ToShortTimeString().ToString().Replace(':', '_'), DateTime.Now.Millisecond.ToString());

                videoWriter = new VideoWriter(filename, 25, CamModel.Width, CamModel.Height, true);

                b.Text = "STOP";

                System.Console.Out.WriteLine("NAGRYWANIE");
                System.Console.Out.WriteLine(filename);
            }
            else
            {
                videoWriter.Dispose();
                videoWriter = null;
                b.Text = "REC";

                System.Console.Out.WriteLine("KONIEC NAGRYWANIA");
            }
            
        }

        int num = 180;
        private void button4_Click(object sender, EventArgs e)
        {
            imgVideoSource.Image.Bitmap.Save(String.Format("C:/video/persp{0}.png", ++num));
        }
    }
}
