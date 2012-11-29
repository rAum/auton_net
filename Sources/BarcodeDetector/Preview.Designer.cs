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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.outputImage = new Emgu.CV.UI.ImageBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.numSleep = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numThreshold = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.grpPOIFound = new System.Windows.Forms.GroupBox();
            this.txtFoundWB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFoundBW = new System.Windows.Forms.TextBox();
            this.txtFoundTotal = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnFileBrowse = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.radFile = new System.Windows.Forms.RadioButton();
            this.radCamera = new System.Windows.Forms.RadioButton();
            this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.udSmoothRadius = new System.Windows.Forms.NumericUpDown();
            this.udSobelRadius = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outputImage)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSleep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).BeginInit();
            this.grpPOIFound.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udSmoothRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udSobelRadius)).BeginInit();
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
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.numSleep);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 241);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(311, 77);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parameters";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 49);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(74, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // numSleep
            // 
            this.numSleep.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numSleep.Location = new System.Drawing.Point(183, 19);
            this.numSleep.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numSleep.Name = "numSleep";
            this.numSleep.Size = new System.Drawing.Size(120, 20);
            this.numSleep.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Interframe Sleep [ms]";
            // 
            // numThreshold
            // 
            this.numThreshold.Location = new System.Drawing.Point(205, 95);
            this.numThreshold.Name = "numThreshold";
            this.numThreshold.Size = new System.Drawing.Size(100, 20);
            this.numThreshold.TabIndex = 1;
            this.numThreshold.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numThreshold.ValueChanged += new System.EventHandler(this.numThreshold_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Threshold";
            // 
            // grpPOIFound
            // 
            this.grpPOIFound.Controls.Add(this.txtFoundWB);
            this.grpPOIFound.Controls.Add(this.label3);
            this.grpPOIFound.Controls.Add(this.txtFoundBW);
            this.grpPOIFound.Controls.Add(this.numThreshold);
            this.grpPOIFound.Controls.Add(this.label4);
            this.grpPOIFound.Controls.Add(this.txtFoundTotal);
            this.grpPOIFound.Controls.Add(this.label2);
            this.grpPOIFound.Controls.Add(this.label1);
            this.grpPOIFound.Location = new System.Drawing.Point(12, 12);
            this.grpPOIFound.Name = "grpPOIFound";
            this.grpPOIFound.Size = new System.Drawing.Size(311, 121);
            this.grpPOIFound.TabIndex = 0;
            this.grpPOIFound.TabStop = false;
            this.grpPOIFound.Text = "POIs Found";
            // 
            // txtFoundWB
            // 
            this.txtFoundWB.Location = new System.Drawing.Point(205, 17);
            this.txtFoundWB.Name = "txtFoundWB";
            this.txtFoundWB.ReadOnly = true;
            this.txtFoundWB.Size = new System.Drawing.Size(100, 20);
            this.txtFoundWB.TabIndex = 5;
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
            // txtFoundBW
            // 
            this.txtFoundBW.Location = new System.Drawing.Point(205, 43);
            this.txtFoundBW.Name = "txtFoundBW";
            this.txtFoundBW.ReadOnly = true;
            this.txtFoundBW.Size = new System.Drawing.Size(100, 20);
            this.txtFoundBW.TabIndex = 4;
            // 
            // txtFoundTotal
            // 
            this.txtFoundTotal.Location = new System.Drawing.Point(205, 69);
            this.txtFoundTotal.Name = "txtFoundTotal";
            this.txtFoundTotal.ReadOnly = true;
            this.txtFoundTotal.Size = new System.Drawing.Size(100, 20);
            this.txtFoundTotal.TabIndex = 3;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "White -> Black";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnFileBrowse);
            this.groupBox3.Controls.Add(this.txtFilePath);
            this.groupBox3.Controls.Add(this.radFile);
            this.groupBox3.Controls.Add(this.radCamera);
            this.groupBox3.Location = new System.Drawing.Point(12, 139);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(311, 96);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Source";
            // 
            // btnFileBrowse
            // 
            this.btnFileBrowse.Location = new System.Drawing.Point(279, 66);
            this.btnFileBrowse.Name = "btnFileBrowse";
            this.btnFileBrowse.Size = new System.Drawing.Size(24, 23);
            this.btnFileBrowse.TabIndex = 3;
            this.btnFileBrowse.Text = "...";
            this.btnFileBrowse.UseVisualStyleBackColor = true;
            this.btnFileBrowse.Click += new System.EventHandler(this.btnFileBrowse_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(9, 68);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(264, 20);
            this.txtFilePath.TabIndex = 2;
            // 
            // radFile
            // 
            this.radFile.AutoSize = true;
            this.radFile.Location = new System.Drawing.Point(7, 44);
            this.radFile.Name = "radFile";
            this.radFile.Size = new System.Drawing.Size(41, 17);
            this.radFile.TabIndex = 1;
            this.radFile.Text = "File";
            this.radFile.UseVisualStyleBackColor = true;
            this.radFile.CheckedChanged += new System.EventHandler(this.radFile_CheckedChanged);
            // 
            // radCamera
            // 
            this.radCamera.AutoSize = true;
            this.radCamera.Checked = true;
            this.radCamera.Location = new System.Drawing.Point(9, 20);
            this.radCamera.Name = "radCamera";
            this.radCamera.Size = new System.Drawing.Size(61, 17);
            this.radCamera.TabIndex = 0;
            this.radCamera.TabStop = true;
            this.radCamera.Text = "Camera";
            this.radCamera.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.udSobelRadius);
            this.groupBox4.Controls.Add(this.udSmoothRadius);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(12, 325);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(311, 178);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Detector parameters";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Sobel radius";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Smooth radius";
            // 
            // udSmoothRadius
            // 
            this.udSmoothRadius.Location = new System.Drawing.Point(183, 20);
            this.udSmoothRadius.Name = "udSmoothRadius";
            this.udSmoothRadius.Size = new System.Drawing.Size(120, 20);
            this.udSmoothRadius.TabIndex = 2;
            this.udSmoothRadius.ValueChanged += new System.EventHandler(this.udSmoothRadius_ValueChanged);
            // 
            // udSobelRadius
            // 
            this.udSobelRadius.Location = new System.Drawing.Point(183, 47);
            this.udSobelRadius.Name = "udSobelRadius";
            this.udSobelRadius.Size = new System.Drawing.Size(120, 20);
            this.udSobelRadius.TabIndex = 3;
            this.udSobelRadius.ValueChanged += new System.EventHandler(this.udSobelRadius_ValueChanged);
            // 
            // Preview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 540);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
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
            ((System.ComponentModel.ISupportInitialize)(this.numSleep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).EndInit();
            this.grpPOIFound.ResumeLayout(false);
            this.grpPOIFound.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udSmoothRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udSobelRadius)).EndInit();
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
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnFileBrowse;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.RadioButton radFile;
        private System.Windows.Forms.RadioButton radCamera;
        private System.Windows.Forms.SaveFileDialog saveFileDialog2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.NumericUpDown numSleep;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown udSobelRadius;
        private System.Windows.Forms.NumericUpDown udSmoothRadius;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}

