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
            this.gbOutput = new System.Windows.Forms.GroupBox();
            this.imgOutput = new Emgu.CV.UI.ImageBox();
            this.grpDebug = new System.Windows.Forms.GroupBox();
            this.imgDebug = new Emgu.CV.UI.ImageBox();
            this.imgVideoSource = new Emgu.CV.UI.ImageBox();
            this.gbVideoSource = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
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
            this.grpDebug.Controls.Add(this.imgDebug);
            this.grpDebug.Location = new System.Drawing.Point(652, 2);
            this.grpDebug.Name = "grpDebug";
            this.grpDebug.Size = new System.Drawing.Size(320, 295);
            this.grpDebug.TabIndex = 4;
            this.grpDebug.TabStop = false;
            this.grpDebug.Text = "Dbg View";
            // 
            // imgDebug
            // 
            this.imgDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgDebug.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imgDebug.Location = new System.Drawing.Point(3, 16);
            this.imgDebug.Name = "imgDebug";
            this.imgDebug.Size = new System.Drawing.Size(314, 276);
            this.imgDebug.TabIndex = 2;
            this.imgDebug.TabStop = false;
            // 
            // imgVideoSource
            // 
            this.imgVideoSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgVideoSource.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imgVideoSource.Location = new System.Drawing.Point(3, 16);
            this.imgVideoSource.Name = "imgVideoSource";
            this.imgVideoSource.Size = new System.Drawing.Size(314, 276);
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
            this.gbVideoSource.Size = new System.Drawing.Size(320, 295);
            this.gbVideoSource.TabIndex = 0;
            this.gbVideoSource.TabStop = false;
            this.gbVideoSource.Text = "VideoSource";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 242);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(974, 58);
            this.panel1.TabIndex = 5;
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
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(974, 300);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grpDebug);
            this.Controls.Add(this.gbOutput);
            this.Controls.Add(this.gbVideoSource);
            this.Location = new System.Drawing.Point(200, 200);
            this.Name = "ViewForm";
            this.Text = "ViewForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewForm_FormClosing);
            this.Resize += new System.EventHandler(this.ViewForm_Resize);
            this.gbOutput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgOutput)).EndInit();
            this.grpDebug.ResumeLayout(false);
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
    }
}