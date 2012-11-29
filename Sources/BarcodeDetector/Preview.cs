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

using Auton.CarVision.Video;
using Auton.CarVision.Video.Filters;

namespace BarcodeDetector
{
    public partial class Preview : Form
    {

        private Capture camera;
        POIDetector detector;
       
        public Preview(string filename = null)
        {
            InitializeComponent();
            GrayVideoSource<float> source = new GrayVideoSource<float>();
            POIDetector detector;
            //source.ResultReady += detector.

            detector = new POIDetector(source, (double)numThreshold.Value);

            udSmoothRadius.Value = detector.SmoothRadius;
            udSobelRadius.Value = detector.SobelRadius;
            udScanlineWidth.Value = detector.ScanlineWidth;

            detector.ResultReady += displayResult;
        }

        private void displayResult(object sender, ResultReadyEventArgs<Image<Bgr, float>> e)
        {
            try
            {
                outputImage.Image = (Image<Bgr, float>)e.Result;
            }
            catch (Exception)
            {
            }
        }

        private void Preview_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void numThreshold_ValueChanged(object sender, EventArgs e)
        {
            //detector.GradientMagnitudeThreshold = (double)numThreshold.Value;
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
            //if (!started)
            //{
            //    if (camera != null)
            //    {
            //        camera.Stop();
            //        camera.Dispose();
            //    }
            //    try
            //    {
            //        if (string.IsNullOrWhiteSpace(txtFilePath.Text))
            //            camera = new Capture();
            //        else
            //            camera = new Capture(txtFilePath.Text);
            //        camera.ImageGrabbed += ProcessFrame;

            //        camera.Start();
            //        started = true;
            //        button1.Text = "Pause";
            //    }
            //    catch (System.Exception ex)
            //    {
            //        MessageBox.Show("Unable to open file/camera. I'm gonna crash");
            //        return;
            //    }
            //}
            //else
            //{
            //    if (camera != null)
            //    {
            //        camera.Pause();
            //        started = false;
            //        button1.Text = "Resume";
            //    }
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void udSmoothRadius_ValueChanged(object sender, EventArgs e)
        {
            //detector.SmoothRadius = (int)udSmoothRadius.Value;
        }

        private void udSobelRadius_ValueChanged(object sender, EventArgs e)
        {
            //detector.SobelRadius = (int)udSobelRadius.Value;
        }

        private void udScanlineWidth_ValueChanged(object sender, EventArgs e)
        {
            //detector.ScanlineWidth = (int)udScanlineWidth.Value;
        }

    }
}
