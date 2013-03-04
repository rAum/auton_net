namespace CarVision
{
    partial class ViewForm
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
            this.imgOutput = new Emgu.CV.UI.ImageBox();
            this.imgDebug = new Emgu.CV.UI.ImageBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbVideoSource = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tbColor = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.nud3 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nud2 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.lColorPrev2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.lColorPrev = new System.Windows.Forms.Label();
            this.nud1 = new System.Windows.Forms.NumericUpDown();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.imgVideoSource = new Emgu.CV.UI.ImageBox();
            this.imgDebug2 = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgDebug)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgVideoSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgDebug2)).BeginInit();
            this.SuspendLayout();
            // 
            // imgOutput
            // 
            this.imgOutput.BackColor = System.Drawing.Color.Transparent;
            this.imgOutput.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imgOutput.Location = new System.Drawing.Point(658, 10);
            this.imgOutput.Name = "imgOutput";
            this.imgOutput.Size = new System.Drawing.Size(160, 120);
            this.imgOutput.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgOutput.TabIndex = 2;
            this.imgOutput.TabStop = false;
            // 
            // imgDebug
            // 
            this.imgDebug.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imgDebug.Location = new System.Drawing.Point(658, 136);
            this.imgDebug.Name = "imgDebug";
            this.imgDebug.Size = new System.Drawing.Size(160, 120);
            this.imgDebug.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgDebug.TabIndex = 2;
            this.imgDebug.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbVideoSource);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.tbColor);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.nud3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.nud2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lColorPrev2);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.lColorPrev);
            this.panel1.Controls.Add(this.nud1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 496);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(822, 117);
            this.panel1.TabIndex = 5;
            // 
            // tbVideoSource
            // 
            this.tbVideoSource.Location = new System.Drawing.Point(550, 39);
            this.tbVideoSource.Name = "tbVideoSource";
            this.tbVideoSource.ReadOnly = true;
            this.tbVideoSource.Size = new System.Drawing.Size(149, 20);
            this.tbVideoSource.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(500, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Current:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(470, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Video Source:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Camera",
            "rec_2012-12-14_10_40_442",
            "rec_2012-12-14_10_33_969",
            "rec_2012-12-14_10_37_533",
            "rec_2013-03-02_18_15_708",
            "rec_2013-03-04_13_41_782",
            "rec_2013-03-02_18_11_569",
            "rec_2013-03-02_18_12_374",
            "rec_2013-03-02_18_14_124",
            "rec_2013-03-02_18_15_239",
            "rec_2013-03-02_18_15_708",
            "testBlue"});
            this.comboBox1.Location = new System.Drawing.Point(550, 10);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(149, 21);
            this.comboBox1.TabIndex = 17;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // tbColor
            // 
            this.tbColor.Location = new System.Drawing.Point(0, 3);
            this.tbColor.Multiline = true;
            this.tbColor.Name = "tbColor";
            this.tbColor.ReadOnly = true;
            this.tbColor.Size = new System.Drawing.Size(271, 64);
            this.tbColor.TabIndex = 16;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(705, 39);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(113, 23);
            this.button6.TabIndex = 15;
            this.button6.Text = "CAM (UN)PAUSE";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(705, 10);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(113, 23);
            this.button5.TabIndex = 7;
            this.button5.Text = "CAM RESET";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(119, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "V:";
            // 
            // nud3
            // 
            this.nud3.Location = new System.Drawing.Point(109, 85);
            this.nud3.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.nud3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud3.Name = "nud3";
            this.nud3.Size = new System.Drawing.Size(42, 20);
            this.nud3.TabIndex = 13;
            this.nud3.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nud3.ValueChanged += new System.EventHandler(this.nud1_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(71, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "S:";
            // 
            // nud2
            // 
            this.nud2.Location = new System.Drawing.Point(61, 85);
            this.nud2.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.nud2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud2.Name = "nud2";
            this.nud2.Size = new System.Drawing.Size(42, 20);
            this.nud2.TabIndex = 11;
            this.nud2.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nud2.ValueChanged += new System.EventHandler(this.nud1_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "H:";
            // 
            // lColorPrev2
            // 
            this.lColorPrev2.AutoSize = true;
            this.lColorPrev2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lColorPrev2.Location = new System.Drawing.Point(277, 39);
            this.lColorPrev2.Name = "lColorPrev2";
            this.lColorPrev2.Size = new System.Drawing.Size(41, 20);
            this.lColorPrev2.TabIndex = 9;
            this.lColorPrev2.Text = "[      ]";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(157, 86);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(81, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Probe Color";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lColorPrev
            // 
            this.lColorPrev.AutoSize = true;
            this.lColorPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lColorPrev.Location = new System.Drawing.Point(277, 13);
            this.lColorPrev.Name = "lColorPrev";
            this.lColorPrev.Size = new System.Drawing.Size(41, 20);
            this.lColorPrev.TabIndex = 4;
            this.lColorPrev.Text = "[      ]";
            // 
            // nud1
            // 
            this.nud1.Location = new System.Drawing.Point(13, 85);
            this.nud1.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.nud1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud1.Name = "nud1";
            this.nud1.Size = new System.Drawing.Size(42, 20);
            this.nud1.TabIndex = 2;
            this.nud1.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nud1.ValueChanged += new System.EventHandler(this.nud1_ValueChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(662, 463);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(156, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Photo";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.Control;
            this.button3.ForeColor = System.Drawing.Color.Red;
            this.button3.Location = new System.Drawing.Point(662, 434);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(156, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "REC";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // imgVideoSource
            // 
            this.imgVideoSource.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imgVideoSource.Location = new System.Drawing.Point(12, 10);
            this.imgVideoSource.Name = "imgVideoSource";
            this.imgVideoSource.Size = new System.Drawing.Size(640, 480);
            this.imgVideoSource.TabIndex = 4;
            this.imgVideoSource.TabStop = false;
            this.imgVideoSource.Click += new System.EventHandler(this.imgVideoSource_Click);
            this.imgVideoSource.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imgVideoSource_MouseMove);
            // 
            // imgDebug2
            // 
            this.imgDebug2.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imgDebug2.Location = new System.Drawing.Point(658, 262);
            this.imgDebug2.Name = "imgDebug2";
            this.imgDebug2.Size = new System.Drawing.Size(160, 120);
            this.imgDebug2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgDebug2.TabIndex = 7;
            this.imgDebug2.TabStop = false;
            // 
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(822, 613);
            this.Controls.Add(this.imgDebug2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.imgDebug);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.imgOutput);
            this.Controls.Add(this.imgVideoSource);
            this.Location = new System.Drawing.Point(200, 200);
            this.Name = "ViewForm";
            this.Text = "Perception Debugger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewForm_FormClosing);
            this.Resize += new System.EventHandler(this.ViewForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.imgOutput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgDebug)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgVideoSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgDebug2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox imgOutput;
        private Emgu.CV.UI.ImageBox imgDebug;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private Emgu.CV.UI.ImageBox imgVideoSource;
        private System.Windows.Forms.NumericUpDown nud1;
        private System.Windows.Forms.Label lColorPrev;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label lColorPrev2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nud3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nud2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox tbColor;
        private Emgu.CV.UI.ImageBox imgDebug2;
        private System.Windows.Forms.TextBox tbVideoSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}