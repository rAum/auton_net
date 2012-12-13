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
            this.gbOutput = new System.Windows.Forms.GroupBox();
            this.imgOutput = new Emgu.CV.UI.ImageBox();
            this.grpDebug = new System.Windows.Forms.GroupBox();
            this.label_targetWheelAngle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.imgDebug = new Emgu.CV.UI.ImageBox();
            this.imgVideoSource = new Emgu.CV.UI.ImageBox();
            this.gbVideoSource = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.button_EnableAutomaticSteering = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label_TargetSpeed = new System.Windows.Forms.Label();
            this.gbOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgOutput)).BeginInit();
            this.grpDebug.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgDebug)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgVideoSource)).BeginInit();
            this.gbVideoSource.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbOutput
            // 
            this.gbOutput.Controls.Add(this.imgOutput);
            this.gbOutput.Location = new System.Drawing.Point(326, 2);
            this.gbOutput.Name = "gbOutput";
            this.gbOutput.Size = new System.Drawing.Size(320, 240);
            this.gbOutput.TabIndex = 3;
            this.gbOutput.TabStop = false;
            this.gbOutput.Text = "Output";
            // 
            // imgOutput
            // 
            this.imgOutput.BackColor = System.Drawing.Color.Transparent;
            this.imgOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgOutput.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imgOutput.Location = new System.Drawing.Point(3, 16);
            this.imgOutput.Name = "imgOutput";
            this.imgOutput.Size = new System.Drawing.Size(314, 221);
            this.imgOutput.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgOutput.TabIndex = 2;
            this.imgOutput.TabStop = false;
            // 
            // grpDebug
            // 
            this.grpDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDebug.Controls.Add(this.label_TargetSpeed);
            this.grpDebug.Controls.Add(this.label4);
            this.grpDebug.Controls.Add(this.button_EnableAutomaticSteering);
            this.grpDebug.Controls.Add(this.label_targetWheelAngle);
            this.grpDebug.Controls.Add(this.label1);
            this.grpDebug.Controls.Add(this.imgDebug);
            this.grpDebug.Location = new System.Drawing.Point(652, 2);
            this.grpDebug.Name = "grpDebug";
            this.grpDebug.Size = new System.Drawing.Size(668, 240);
            this.grpDebug.TabIndex = 4;
            this.grpDebug.TabStop = false;
            this.grpDebug.Text = "Dbg View";
            // 
            // label_targetWheelAngle
            // 
            this.label_targetWheelAngle.AutoSize = true;
            this.label_targetWheelAngle.Location = new System.Drawing.Point(369, 56);
            this.label_targetWheelAngle.Name = "label_targetWheelAngle";
            this.label_targetWheelAngle.Size = new System.Drawing.Size(22, 13);
            this.label_targetWheelAngle.TabIndex = 4;
            this.label_targetWheelAngle.Text = "0.0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(346, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "target wheel angle";
            // 
            // imgDebug
            // 
            this.imgDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgDebug.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imgDebug.Location = new System.Drawing.Point(3, 16);
            this.imgDebug.MaximumSize = new System.Drawing.Size(314, 221);
            this.imgDebug.Name = "imgDebug";
            this.imgDebug.Size = new System.Drawing.Size(314, 221);
            this.imgDebug.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgDebug.TabIndex = 2;
            this.imgDebug.TabStop = false;
            // 
            // imgVideoSource
            // 
            this.imgVideoSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgVideoSource.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imgVideoSource.Location = new System.Drawing.Point(3, 16);
            this.imgVideoSource.Name = "imgVideoSource";
            this.imgVideoSource.Size = new System.Drawing.Size(662, 221);
            this.imgVideoSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgVideoSource.TabIndex = 2;
            this.imgVideoSource.TabStop = false;
            // 
            // gbVideoSource
            // 
            this.gbVideoSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbVideoSource.Controls.Add(this.imgVideoSource);
            this.gbVideoSource.Location = new System.Drawing.Point(0, 2);
            this.gbVideoSource.Name = "gbVideoSource";
            this.gbVideoSource.Size = new System.Drawing.Size(668, 240);
            this.gbVideoSource.TabIndex = 0;
            this.gbVideoSource.TabStop = false;
            this.gbVideoSource.Text = "VideoSource";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 242);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1322, 58);
            this.panel1.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 17);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(199, 27);
            this.button2.TabIndex = 1;
            this.button2.Text = "Define Line Color Range";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(691, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(230, 27);
            this.button1.TabIndex = 0;
            this.button1.Text = "Toggle lane detector / road detector";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // colorDialog
            // 
            this.colorDialog.AnyColor = true;
            this.colorDialog.Color = System.Drawing.Color.Turquoise;
            // 
            // button_EnableAutomaticSteering
            // 
            this.button_EnableAutomaticSteering.Location = new System.Drawing.Point(487, 28);
            this.button_EnableAutomaticSteering.Name = "button_EnableAutomaticSteering";
            this.button_EnableAutomaticSteering.Size = new System.Drawing.Size(143, 23);
            this.button_EnableAutomaticSteering.TabIndex = 5;
            this.button_EnableAutomaticSteering.Text = "Enable Automatic Steering";
            this.button_EnableAutomaticSteering.UseVisualStyleBackColor = true;
            this.button_EnableAutomaticSteering.Click += new System.EventHandler(this.button_EnableAutomaticSteering_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(346, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "target speed";
            // 
            // label_TargetSpeed
            // 
            this.label_TargetSpeed.AutoSize = true;
            this.label_TargetSpeed.Location = new System.Drawing.Point(369, 137);
            this.label_TargetSpeed.Name = "label_TargetSpeed";
            this.label_TargetSpeed.Size = new System.Drawing.Size(22, 13);
            this.label_TargetSpeed.TabIndex = 7;
            this.label_TargetSpeed.Text = "0.0";
            // 
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1322, 300);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grpDebug);
            this.Controls.Add(this.gbOutput);
            this.Controls.Add(this.gbVideoSource);
            this.Location = new System.Drawing.Point(200, 200);
            this.Name = "ViewForm";
            this.Text = "ViewForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewForm_FormClosing);
            this.Load += new System.EventHandler(this.ViewForm_Load);
            this.Resize += new System.EventHandler(this.ViewForm_Resize);
            this.gbOutput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgOutput)).EndInit();
            this.grpDebug.ResumeLayout(false);
            this.grpDebug.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgDebug)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgVideoSource)).EndInit();
            this.gbVideoSource.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbOutput;
        private Emgu.CV.UI.ImageBox imgOutput;
        private System.Windows.Forms.GroupBox grpDebug;
        private Emgu.CV.UI.ImageBox imgDebug;
        private Emgu.CV.UI.ImageBox imgVideoSource;
        private System.Windows.Forms.GroupBox gbVideoSource;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Label label_targetWheelAngle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_EnableAutomaticSteering;
        private System.Windows.Forms.Label label_TargetSpeed;
        private System.Windows.Forms.Label label4;
    }
}