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
        ColorVideoSource colorVideoSource;
        HsvFilter filter;

        RoadCenterDetector roadDetector;
        VisualiseSimpleRoadModel visRoad;
        PerspectiveCorrectionRgb invPerp;

        DrawPoints filtered;

        VideoWriter videoWriter;

        bool colorCapture;
        bool perspCapture;

        // perspektywa
        //180 cm od niebieskiej
        //179 cm

        private void DisplayVideo(object sender, ResultReadyEventArgs<Image<Gray, Byte>> e)
        {
            ImageBox imgBox = imgDebug2;
            try
            {
                if (sender == visRoad)
                    imgBox = imgDebug;
                imgBox.Image = (Image<Gray, byte>)e.Result;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Bad thing happened in display gray video :( -" + ex.Message);
            }
        }
        bool swapImages = false;
        private void DisplayVideo(object sender, ResultReadyEventArgs<Image<Bgr, byte>> e)
        {
            try
            {
                ImageBox big  = imgVideoSource,
                        small = imgDebug;

                if (swapImages)
                {
                    ImageBox tmp = big;
                    big = small;
                    small = tmp;
                }


                if (sender == visRoad)
                    big = imgOutput;

                if (sender == invPerp)
                {
                    Image<Bgr, byte> cam = new Image<Bgr, byte>(big.Image.Bitmap);
                    small.Image = (Image<Bgr, byte>)e.Result + cam;
                    return;
                }
                
                // colorVideoSource
                big.Image = (Image<Bgr, byte>)e.Result;


                if (videoWriter != null && sender == colorVideoSource)
                {
                    videoWriter.WriteFrame(((Image<Bgr, byte>)e.Result).Convert<Bgr, byte>());
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

        private string getVideoSource()
        {
            string src = (string)comboBox1.Items[comboBox1.SelectedIndex];
            tbVideoSource.Text = src;
            if (src == "Camera" || src == "") return "";
            else return "C:/video/" + src + ".avi";
        }

        public ViewForm()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 1; // not cam!!
            colorVideoSource = new ColorVideoSource(getVideoSource());
            colorVideoSource.ResultReady += DisplayVideo;

            // light green lines
            Hsv minColor = new Hsv(95 / 2, 0.6 * 255, 0.5 * 255);
            Hsv maxColor = new Hsv(180 / 2, 255, 0.74 * 255);

            filter = new HsvFilter(colorVideoSource, minColor, maxColor);
            //filter.ResultReady += DisplayVideo;
            roadDetector = new RoadCenterDetector(filter);
           // roadDetector.Perceptor.perspectiveTransform.ResultReady += DisplayVideo;
            filtered = new DrawPoints(roadDetector.Perceptor.laneDetector);
            filtered.ResultReady += DisplayVideo;
            filtered.Active = true;

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

        Hsv color = new Hsv(67, 128, 148);
        private void imgVideoSource_Click(object sender, EventArgs e)
        {
            if (colorCapture)
            {
                try
                {
                    lColorPrev.BackColor = ExtractPixel(sender);
                    color = ColorToHsv(lColorPrev.BackColor);
                    SetHsvFilter(color);
                }
                catch (Exception)
                {
                    //no.
                }
            }
        }

        private void SetHsvFilter(Hsv color)
        {
            int h = (int)nud1.Value,
                s = (int)nud2.Value,
                v = (int)nud3.Value;

            Hsv min = new Hsv(),
                max = new Hsv();

            min.Hue = Math.Max(color.Hue - h, 0) / 2;
            min.Satuation = Math.Max(color.Satuation - s, 0);
            min.Value = Math.Max(color.Value - v, 0);

            max.Hue = Math.Min(color.Hue + h, 255) / 2;
            max.Satuation = Math.Min(color.Satuation + s, 255);
            max.Value = Math.Min(color.Value + v, 255);

            Hsv a = new Hsv(Math.Min(min.Hue, max.Hue), Math.Min(min.Satuation, max.Satuation), Math.Min(min.Value, max.Value)), 
                b = new Hsv(Math.Max(min.Hue, max.Hue), Math.Max(min.Satuation, max.Satuation), Math.Max(min.Value, max.Value));

            if (filter != null)
            {
                filter.lower = a;
                filter.upper = b;
            }

            tbColor.Text = String.Format("Hsv minColor = new Hsv({0,3}, {1,3}, {2,3});\r\nHsv maxColor = new Hsv({3,3}, {4,3}, {5,3});",
                                            (int)Math.Round(a.Hue), (int)Math.Round(a.Satuation), (int)Math.Round(a.Value),
                                            (int)Math.Round(b.Hue), (int)Math.Round(b.Satuation), (int)Math.Round(b.Value)
                                        );
        }

        private Color ExtractPixel(object sender)
        {
            Emgu.CV.UI.ImageBox pb = sender as Emgu.CV.UI.ImageBox;
            Bitmap img = pb.Image.Bitmap;
            Point p = pb.PointToClient(Cursor.Position);
            return img.GetPixel(p.X, p.Y);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            colorCapture = !colorCapture;
            if (colorCapture)
            {
                filtered.Active = false;
                filter.ResultReady += DisplayVideo;
                filtered.ResultReady -= DisplayVideo;
            }
            else
            {
                filter.ResultReady -= DisplayVideo;
                filtered.ResultReady += DisplayVideo;
                filtered.Active = true;
            }
        }

        private void imgVideoSource_MouseMove(object sender, MouseEventArgs e)
        {
            if (colorCapture)
            {
                try
                {
                    lColorPrev2.BackColor = ExtractPixel(sender);
                }
                catch (Exception)
                {
                    // no.
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            colorVideoSource.RestartVideo();
            pause = false;
        }

        bool pause = false;
        private void button6_Click(object sender, EventArgs e)
        {
            if (pause == false)
                colorVideoSource.Pause();
            else
                colorVideoSource.Start();
            pause = !pause;
        }

        private void nud1_ValueChanged(object sender, EventArgs e)
        {
            if (colorCapture)
            {
                SetHsvFilter(color);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (colorVideoSource != null)
            {
                colorVideoSource.ChangeVideoSource(getVideoSource());
            }
        }

        private void cbRoadPreview_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            swapImages = cb.Checked;
        }
    }
}
