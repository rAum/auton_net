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


namespace BarcodeDetector
{
    public partial class Preview : Form
    {

        private Capture camera;
        POIDetector detector;
       
        public Preview()
        {
            InitializeComponent();
            camera = new Capture();
            if (camera == null) 
            {
                MessageBox.Show("Unable to open camera");
            }

            detector = new POIDetector((double) numThreshold.Value);
            
            camera.ImageGrabbed += ProcessFrame;
            camera.Start();
        }

        private void ProcessFrame(object sender, EventArgs args) 
        {
            
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

            Point previous = new Point(0, (int) detector.GradientMagnitude(0, frame.Height/2) + frame.Height/2);

            for (int i = 1; i < frame.Width; ++i)
            {
                Point current = new Point(i, (int)detector.GradientMagnitude(i, frame.Height / 2) + frame.Height / 2);
                frame.Draw(new LineSegment2D(previous, current), new Bgr(Color.Green), 1);
                previous = current;
            }

            frame.Draw(new LineSegment2D(
                new Point(0, frame.Height / 2 + (int)detector.GradientMagnitudeThreshold),
                new Point(frame.Width, frame.Height / 2 + (int)detector.GradientMagnitudeThreshold
                    )), new Bgr(Color.Yellow), 1);

            frame.Draw(new LineSegment2D(
                new Point(0, frame.Height / 2 - (int)detector.GradientMagnitudeThreshold),
                new Point(frame.Width, frame.Height / 2 - (int)detector.GradientMagnitudeThreshold
                    )), new Bgr(Color.Yellow), 1);

            outputImage.Image = frame.Clone();
            
            this.Invoke((MethodInvoker)delegate()
            {
                txtFoundBW.Text = detector.FoundBW.ToString();
                txtFoundWB.Text = detector.FoundWB.ToString();
                txtFoundTotal.Text = detector.Found.ToString();
            });
            
        }

        private void Preview_FormClosing(object sender, FormClosingEventArgs e)
        {
            camera.Stop();
        }

        private void numThreshold_ValueChanged(object sender, EventArgs e)
        {
            detector.GradientMagnitudeThreshold = (double)numThreshold.Value;
        }

    }
}
