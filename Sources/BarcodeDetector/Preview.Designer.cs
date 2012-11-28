namespace BarcodeDetector
{
    partial class Preview
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.outputImage = new Emgu.CV.UI.ImageBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grpPOIFound = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFoundTotal = new System.Windows.Forms.TextBox();
            this.txtFoundBW = new System.Windows.Forms.TextBox();
            this.txtFoundWB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numThreshold = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outputImage)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.grpPOIFound.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.outputImage);
            this.groupBox1.Location = new System.Drawing.Point(329, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(583, 491);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input";
            // 
            // outputImage
            // 
            this.outputImage.Location = new System.Drawing.Point(6, 19);
            this.outputImage.Name = "outputImage";
            this.outputImage.Size = new System.Drawing.Size(571, 466);
            this.outputImage.TabIndex = 2;
            this.outputImage.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numThreshold);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(311, 385);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parameters";
            // 
            // grpPOIFound
            // 
            this.grpPOIFound.Controls.Add(this.txtFoundWB);
            this.grpPOIFound.Controls.Add(this.label3);
            this.grpPOIFound.Controls.Add(this.txtFoundBW);
            this.grpPOIFound.Controls.Add(this.txtFoundTotal);
            this.grpPOIFound.Controls.Add(this.label2);
            this.grpPOIFound.Controls.Add(this.label1);
            this.grpPOIFound.Location = new System.Drawing.Point(12, 12);
            this.grpPOIFound.Name = "grpPOIFound";
            this.grpPOIFound.Size = new System.Drawing.Size(311, 100);
            this.grpPOIFound.TabIndex = 0;
            this.grpPOIFound.TabStop = false;
            this.grpPOIFound.Text = "POIs Found";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "White -> Black";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Black -> White";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Total";
            // 
            // txtFoundTotal
            // 
            this.txtFoundTotal.Location = new System.Drawing.Point(205, 69);
            this.txtFoundTotal.Name = "txtFoundTotal";
            this.txtFoundTotal.ReadOnly = true;
            this.txtFoundTotal.Size = new System.Drawing.Size(100, 20);
            this.txtFoundTotal.TabIndex = 3;
            // 
            // txtFoundBW
            // 
            this.txtFoundBW.Location = new System.Drawing.Point(205, 43);
            this.txtFoundBW.Name = "txtFoundBW";
            this.txtFoundBW.ReadOnly = true;
            this.txtFoundBW.Size = new System.Drawing.Size(100, 20);
            this.txtFoundBW.TabIndex = 4;
            // 
            // txtFoundWB
            // 
            this.txtFoundWB.Location = new System.Drawing.Point(205, 17);
            this.txtFoundWB.Name = "txtFoundWB";
            this.txtFoundWB.ReadOnly = true;
            this.txtFoundWB.Size = new System.Drawing.Size(100, 20);
            this.txtFoundWB.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Threshold";
            // 
            // numThreshold
            // 
            this.numThreshold.Location = new System.Drawing.Point(185, 19);
            this.numThreshold.Name = "numThreshold";
            this.numThreshold.Size = new System.Drawing.Size(120, 20);
            this.numThreshold.TabIndex = 1;
            this.numThreshold.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numThreshold.ValueChanged += new System.EventHandler(this.numThreshold_ValueChanged);
            // 
            // Preview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 515);
            this.Controls.Add(this.grpPOIFound);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Preview";
            this.Text = "Preview";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Preview_FormClosing);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.outputImage)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpPOIFound.ResumeLayout(false);
            this.grpPOIFound.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private Emgu.CV.UI.ImageBox outputImage;
        private System.Windows.Forms.NumericUpDown numThreshold;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox grpPOIFound;
        private System.Windows.Forms.TextBox txtFoundWB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFoundBW;
        private System.Windows.Forms.TextBox txtFoundTotal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

