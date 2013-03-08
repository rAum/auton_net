namespace BrainProject
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
            this.grpDebug = new System.Windows.Forms.GroupBox();
            this.imgDebug = new Emgu.CV.UI.ImageBox();
            this.label_TargetSpeed = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_EnableAutomaticSteering = new System.Windows.Forms.Button();
            this.label_targetWheelAngle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.imgVideoSource = new Emgu.CV.UI.ImageBox();
            this.gbVideoSource = new System.Windows.Forms.GroupBox();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imgOutput)).BeginInit();
            this.grpDebug.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgDebug)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgVideoSource)).BeginInit();
            this.gbVideoSource.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgOutput
            // 
            this.imgOutput.BackColor = System.Drawing.Color.Transparent;
            this.imgOutput.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imgOutput.Location = new System.Drawing.Point(0, 1);
            this.imgOutput.Name = "imgOutput";
            this.imgOutput.Size = new System.Drawing.Size(640, 480);
            this.imgOutput.TabIndex = 2;
            this.imgOutput.TabStop = false;
            // 
            // grpDebug
            // 
            this.grpDebug.Controls.Add(this.imgDebug);
            this.grpDebug.Location = new System.Drawing.Point(646, 1);
            this.grpDebug.Name = "grpDebug";
            this.grpDebug.Size = new System.Drawing.Size(324, 240);
            this.grpDebug.TabIndex = 4;
            this.grpDebug.TabStop = false;
            this.grpDebug.Text = "Dbg View";
            // 
            // imgDebug
            // 
            this.imgDebug.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imgDebug.Location = new System.Drawing.Point(3, 16);
            this.imgDebug.MaximumSize = new System.Drawing.Size(314, 221);
            this.imgDebug.Name = "imgDebug";
            this.imgDebug.Size = new System.Drawing.Size(314, 221);
            this.imgDebug.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgDebug.TabIndex = 2;
            this.imgDebug.TabStop = false;
            // 
            // label_TargetSpeed
            // 
            this.label_TargetSpeed.AutoSize = true;
            this.label_TargetSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_TargetSpeed.Location = new System.Drawing.Point(342, 19);
            this.label_TargetSpeed.Name = "label_TargetSpeed";
            this.label_TargetSpeed.Size = new System.Drawing.Size(28, 16);
            this.label_TargetSpeed.TabIndex = 7;
            this.label_TargetSpeed.Text = "0.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(230, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "target speed";
            // 
            // button_EnableAutomaticSteering
            // 
            this.button_EnableAutomaticSteering.BackColor = System.Drawing.Color.DarkRed;
            this.button_EnableAutomaticSteering.Enabled = false;
            this.button_EnableAutomaticSteering.ForeColor = System.Drawing.Color.DarkOrange;
            this.button_EnableAutomaticSteering.Location = new System.Drawing.Point(778, 12);
            this.button_EnableAutomaticSteering.Name = "button_EnableAutomaticSteering";
            this.button_EnableAutomaticSteering.Size = new System.Drawing.Size(181, 38);
            this.button_EnableAutomaticSteering.TabIndex = 5;
            this.button_EnableAutomaticSteering.Text = "Enable Automatic Steering";
            this.button_EnableAutomaticSteering.UseVisualStyleBackColor = false;
            this.button_EnableAutomaticSteering.Click += new System.EventHandler(this.button_EnableAutomaticSteering_Click);
            // 
            // label_targetWheelAngle
            // 
            this.label_targetWheelAngle.AutoSize = true;
            this.label_targetWheelAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_targetWheelAngle.Location = new System.Drawing.Point(158, 19);
            this.label_targetWheelAngle.Name = "label_targetWheelAngle";
            this.label_targetWheelAngle.Size = new System.Drawing.Size(28, 16);
            this.label_targetWheelAngle.TabIndex = 4;
            this.label_targetWheelAngle.Text = "0.0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "target wheel angle";
            // 
            // imgVideoSource
            // 
            this.imgVideoSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgVideoSource.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imgVideoSource.Location = new System.Drawing.Point(3, 16);
            this.imgVideoSource.Name = "imgVideoSource";
            this.imgVideoSource.Size = new System.Drawing.Size(318, 206);
            this.imgVideoSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgVideoSource.TabIndex = 2;
            this.imgVideoSource.TabStop = false;
            // 
            // gbVideoSource
            // 
            this.gbVideoSource.Controls.Add(this.imgVideoSource);
            this.gbVideoSource.Location = new System.Drawing.Point(646, 256);
            this.gbVideoSource.Name = "gbVideoSource";
            this.gbVideoSource.Size = new System.Drawing.Size(324, 225);
            this.gbVideoSource.TabIndex = 0;
            this.gbVideoSource.TabStop = false;
            this.gbVideoSource.Text = "VideoSource";
            // 
            // colorDialog
            // 
            this.colorDialog.AnyColor = true;
            this.colorDialog.Color = System.Drawing.Color.Turquoise;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.button_EnableAutomaticSteering);
            this.groupBox1.Controls.Add(this.label_TargetSpeed);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label_targetWheelAngle);
            this.groupBox1.Location = new System.Drawing.Point(0, 487);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(970, 56);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Steering";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.OliveDrab;
            this.button1.ForeColor = System.Drawing.Color.SeaShell;
            this.button1.Location = new System.Drawing.Point(591, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(181, 38);
            this.button1.TabIndex = 8;
            this.button1.Text = "Enable Road Detection";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(971, 544);
            this.Controls.Add(this.imgOutput);
            this.Controls.Add(this.gbVideoSource);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpDebug);
            this.Location = new System.Drawing.Point(200, 200);
            this.Name = "ViewForm";
            this.Text = "Road Preview";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewForm_FormClosing);
            this.Load += new System.EventHandler(this.ViewForm_Load);
            this.Resize += new System.EventHandler(this.ViewForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.imgOutput)).EndInit();
            this.grpDebug.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgDebug)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgVideoSource)).EndInit();
            this.gbVideoSource.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox imgOutput;
        private System.Windows.Forms.GroupBox grpDebug;
        private Emgu.CV.UI.ImageBox imgDebug;
        private Emgu.CV.UI.ImageBox imgVideoSource;
        private System.Windows.Forms.GroupBox gbVideoSource;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Label label_targetWheelAngle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_EnableAutomaticSteering;
        private System.Windows.Forms.Label label_TargetSpeed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
    }
}