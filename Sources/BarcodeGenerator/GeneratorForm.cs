using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
            int num = Int32.Parse(toolStripTextBox1.Text);

            try
            {
                Image img = MBarcodeImager.Create(num, pictureBox1.Size);
                pictureBox1.Image = img;     

            }
            catch (ArgumentException)
            {
                MessageBox.Show("Invalid number");
            }

        }
    }
}
