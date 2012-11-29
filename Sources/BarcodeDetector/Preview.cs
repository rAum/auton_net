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
using System.Threading;


namespace BarcodeDetector
{
    public partial class Preview : Form
    {

        private Capture camera;
        POIDetector detector;
       
        public Preview(string filename = null)
        {
            InitializeComponent();
            detector = new POIDetector((double)numThreshold.Value);

            udSmoothRadius.Value = detector.SmoothRadius;
            udSobelRadius.Value = detector.SobelRadius;
            udScanlineWidth.Value = detector.MeanRadius;
            udMult.Value = detector.AveragingMultipiler;
        }


        private void DrawFunction(Image<Bgr, Byte> frame, double[] ys, Bgr color) 
        {
            Point previous = new Point(0, (int)ys[0] + frame.Height / 2);
            for (int i = 1; i < frame.Width; ++i)
            {
                Point current = new Point(i, (int)ys[i] + frame.Height / 2);
                frame.Draw(new LineSegment2D(previous, current), color, 1);
                previous = current;
            }
        }


        private void ProcessFrame(object sender, EventArgs args) 
        {
            if (camera == null)
                return; 
        

            Image<Bgr, Byte> frame = camera.RetrieveBgrFrame().Clone();
            Image<Gray, float> gray = frame.Convert<Gray, float>();

            detector.LoadImage(gray);
            List<POI> points = detector.FindPOI();

            
            // draw debug information on frame
            LineSegment2D line = new LineSegment2D(new Point(0, frame.Height / 2), new Point(frame.Width, frame.Height / 2));
            frame.Draw(line, new Bgr(Color.Red), 2);
            
            foreach(POI p in points){
                Point begin = new Point(p.X, p.Y);
                Point end = new Point((int)(p.X + p.GX), (int)(p.Y + p.GY));
                LineSegment2D arrow = new LineSegment2D(begin, end);
                CircleF circle = new CircleF(begin, 5);
                frame.Draw(circle, new Bgr(Color.Blue), 2);
                frame.Draw(arrow, new Bgr(Color.Blue), 1);
            }

            DrawFunction(frame, detector.AbsMeanMagnitude, new Bgr(Color.Green));
            DrawFunction(frame, detector.AdaptiveThreshold, new Bgr(Color.Yellow));  
            
            try{
                outputImage.Image = frame.Clone();
                this.Invoke((MethodInvoker)delegate()
                {
                    txtFoundBW.Text = detector.FoundBW.ToString();
                    txtFoundWB.Text = detector.FoundWB.ToString();
                    txtFoundTotal.Text = detector.Found.ToString();
                });
            }
            catch (InvalidOperationException)
            {}


            Thread.Sleep((int) numSleep.Value);
        }

        private void numThreshold_ValueChanged(object sender, EventArgs e)
        {
            detector.GradientMagnitudeThreshold = (double)numThreshold.Value;
        }

        private void btnFileBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                txtFilePath.Text = openFileDialog1.FileName;
        }

        private void radFile_CheckedChanged(object sender, EventArgs e)
        {
            if (radFile.Checked)
                btnFileBrowse_Click(sender, e);
        }

        private bool started = false;

        private void button1_Click(object sender, EventArgs e)
        {
            if (!started)
            {
                if (camera != null)
                {
                    camera.Stop();
                    camera.Dispose();
                }
                try
                {
                    if (string.IsNullOrWhiteSpace(txtFilePath.Text))
                        camera = new Capture();
                    else
                        camera = new Capture(txtFilePath.Text);
                    camera.ImageGrabbed += ProcessFrame;

                    camera.Start();
                    started = true;
                    button1.Text = "Pau&se";
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Unable to open file/camera. I'm gonna crash");
                    return;
                }
            }
            else
            {
                if (camera != null)
                {
                    camera.Pause();
                    started = false;
                    button1.Text = "Re&sume";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void udSmoothRadius_ValueChanged(object sender, EventArgs e)
        {
            detector.SmoothRadius = (int)udSmoothRadius.Value;
        }

        private void udSobelRadius_ValueChanged(object sender, EventArgs e)
        {
            detector.SobelRadius = (int)udSobelRadius.Value;
        }

        private void udScanlineWidth_ValueChanged(object sender, EventArgs e)
        {
            detector.MeanRadius = (int)udScanlineWidth.Value;
        }

        private void udMult_ValueChanged(object sender, EventArgs e)
        {
            detector.AveragingMultipiler = (int)udMult.Value;
        }

        private void Preview_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (camera != null)
                camera.Stop();
        }
    }
}
