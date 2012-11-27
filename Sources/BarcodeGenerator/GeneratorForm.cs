using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Windows.Media;
using System.Windows;
using System.Windows.Xps;
using System.IO.Packaging;
using System.IO;
using System.Printing;
using System.Windows.Xps.Packaging;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace BarcodeGenerator
{
    public partial class GeneratorForm : Form
    {
        public GeneratorForm()
        {
            InitializeComponent();
        }

        private void generateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*int num = Int32.Parse(toolStripTextBox1.Text);

            try
            {
                Image img = MBarcodeImager.Create(num, pictureBox1.Size);
                pictureBox1.Image = img;     

            }
            catch (ArgumentException)
            {
                MessageBox.Show("Invalid number");
            }*/

        }

        private Image FromImage(Image source, System.Drawing.Size size)
        {
            Bitmap result = new Bitmap(size.Width, size.Height);
            Graphics graphics = Graphics.FromImage(result);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            float ratio = ((float)source.Width) / source.Height;
            float dest_ratio = ((float)size.Width) / size.Height;

            System.Drawing.Size target_size;
            if (ratio <= dest_ratio) // wolne pola po bokach
            {
                target_size = new System.Drawing.Size((int)(ratio * size.Height), size.Height);
            }
            else
            {
                target_size = new System.Drawing.Size(size.Width, (int)(size.Width / ratio));
            }

            System.Drawing.Point topleft = new System.Drawing.Point((size.Width - target_size.Width) / 2, (size.Height - target_size.Height) / 2);

            graphics.DrawImage(source, new Rectangle(topleft, target_size));
            return result;
        }

        private Image fromText(string text, float font_size, System.Drawing.Size rect_size)
        {
            Bitmap result = new Bitmap(rect_size.Width, rect_size.Height);
            Graphics graphics = Graphics.FromImage(result);

            System.Drawing.Brush brush = new SolidBrush(System.Drawing.Color.Black);
            
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            graphics.DrawString(text, new Font("Consolas", font_size * rect_size.Height, GraphicsUnit.Pixel), brush, rect_size.Width * 0.5f, rect_size.Height * 0.5f, stringFormat);


            return result;
            //throw new NotImplementedException();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Image generated = Generate();

            if (saveImageDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                generated.Save(saveImageDialog.FileName);
        }

        private Image Generate()
        {
            System.Drawing.Size pageSize = new System.Drawing.Size(2970, 2100);

            int leftMargin = 0;
            int rightMargin = 0;
            int topMargin = 0;
            int bottomMargin = 0;
            int gap = 0;

            Rectangle topRect = new Rectangle(leftMargin, topMargin, pageSize.Width - leftMargin - rightMargin, (pageSize.Height - topMargin - gap - bottomMargin) / 2);
            Rectangle bottomRect = new Rectangle(leftMargin, topMargin + topRect.Height + gap, topRect.Width, topRect.Height);

            Bitmap resultImage = new Bitmap(pageSize.Width, pageSize.Height);
            Graphics graphics = Graphics.FromImage(resultImage);
            graphics.Clear(System.Drawing.Color.White);
            
            Image halfImage;

            if (topRadioBarcode.Checked)
                halfImage = MBarcodeImager.Create((int)topNumber.Value, topRect.Size);
            else if (topRadioImage.Checked)
            {
                Image readImage = Image.FromFile(topImagePath.Text);
                halfImage = FromImage(readImage, topRect.Size);
            }
            else if (topRadioText.Checked)
            {
                halfImage = fromText(topText.Text, (float)topFontSize.Value, topRect.Size);
            }
            else
                throw new Exception("BUG: none of the radio buttons checked");

            graphics.DrawImageUnscaled(halfImage, topRect);

            if (bottomRadioBarcode.Checked)
                halfImage = MBarcodeImager.Create((int)bottomNumber.Value, bottomRect.Size);
            else if (bottomRadioImage.Checked)
            {
                Image readImage = Image.FromFile(bottomImagePath.Text);
                halfImage = FromImage(readImage, bottomRect.Size);
            }
            else if (bottomRadioText.Checked)
            {
                halfImage = fromText(bottomText.Text, (float)bottomFontSize.Value, bottomRect.Size);
            }
            else
                throw new Exception("BUG: none of the radio buttons checked");

            graphics.DrawImageUnscaled(halfImage, bottomRect);

            return resultImage;
        }

        

        private void bottomImageBrowse_Click(object sender, EventArgs e)
        {
            bottomImageFileDialog.ShowDialog();
            bottomImagePath.Text = bottomImageFileDialog.FileName;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            GenerateAndPreview();
        }

        private void GenerateAndPreview()
        {
            Image generated = Generate();

            Image displayed = new Bitmap(generated, pictureBox1.Size);
            pictureBox1.Image = displayed;
        }

        private void topFontSize_ValueChanged(object sender, EventArgs e)
        {
            GenerateAndPreview();
        }

        private void topNumber_ValueChanged(object sender, EventArgs e)
        {
            GenerateAndPreview();

        }
    }
}
