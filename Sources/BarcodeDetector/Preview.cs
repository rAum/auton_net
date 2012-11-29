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

        GrayVideoSource<float> source;
        POIDetector detector;
       
        public Preview(string filename = null)
        {
            InitializeComponent();
            source = new GrayVideoSource<float>();
            

            detector = new POIDetector(source, (double)numThreshold.Value);

            udSmoothRadius.Value = detector.SmoothRadius;
            udSobelRadius.Value = detector.SobelRadius;
            udMult.Value = detector.AveragingMultipiler;
            udScanlineWidth.Value = detector.MeanRadius;

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


        private void button1_Click(object sender, EventArgs e)
        {
            if (source == null)
                return;

            if (source.Runs)
            {
                source.Pause();
                button1.Text = "Re&sume";
            }
            else {
                source.Start();
                button1.Text = "Pau&se";
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

        private void Preview_FormClosed(object sender, FormClosedEventArgs e)
        {
            source.Stop();
            Application.Exit();
        }
    }
}
