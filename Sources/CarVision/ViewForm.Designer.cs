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
            this.gbVideoSource = new System.Windows.Forms.GroupBox();
            this.imgVideoSource = new Emgu.CV.UI.ImageBox();
            this.grpSmoothener = new System.Windows.Forms.GroupBox();
            this.imgSmoothener = new Emgu.CV.UI.ImageBox();
            this.grpCanny = new System.Windows.Forms.GroupBox();
            this.imgCanny = new Emgu.CV.UI.ImageBox();
            this.gbVideoSource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgVideoSource)).BeginInit();
            this.grpSmoothener.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgSmoothener)).BeginInit();
            this.grpCanny.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgCanny)).BeginInit();
            this.SuspendLayout();
            // 
            // gbVideoSource
            // 
            this.gbVideoSource.Controls.Add(this.imgVideoSource);
            this.gbVideoSource.Location = new System.Drawing.Point(0, 2);
            this.gbVideoSource.Name = "gbVideoSource";
            this.gbVideoSource.Size = new System.Drawing.Size(422, 326);
            this.gbVideoSource.TabIndex = 0;
            this.gbVideoSource.TabStop = false;
            this.gbVideoSource.Text = "VideoSource";
            // 
            // imgVideoSource
            // 
            this.imgVideoSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgVideoSource.Location = new System.Drawing.Point(3, 16);
            this.imgVideoSource.Name = "imgVideoSource";
            this.imgVideoSource.Size = new System.Drawing.Size(416, 307);
            this.imgVideoSource.TabIndex = 2;
            this.imgVideoSource.TabStop = false;
            // 
            // grpSmoothener
            // 
            this.grpSmoothener.Controls.Add(this.imgSmoothener);
            this.grpSmoothener.Location = new System.Drawing.Point(425, 2);
            this.grpSmoothener.Name = "grpSmoothener";
            this.grpSmoothener.Size = new System.Drawing.Size(419, 326);
            this.grpSmoothener.TabIndex = 3;
            this.grpSmoothener.TabStop = false;
            this.grpSmoothener.Text = "Smoothener";
            // 
            // imgSmoothener
            // 
            this.imgSmoothener.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgSmoothener.Location = new System.Drawing.Point(3, 16);
            this.imgSmoothener.Name = "imgSmoothener";
            this.imgSmoothener.Size = new System.Drawing.Size(413, 307);
            this.imgSmoothener.TabIndex = 2;
            this.imgSmoothener.TabStop = false;
            // 
            // grpCanny
            // 
            this.grpCanny.Controls.Add(this.imgCanny);
            this.grpCanny.Location = new System.Drawing.Point(847, 2);
            this.grpCanny.Name = "grpCanny";
            this.grpCanny.Size = new System.Drawing.Size(419, 326);
            this.grpCanny.TabIndex = 4;
            this.grpCanny.TabStop = false;
            this.grpCanny.Text = "Canny";
            // 
            // imgCanny
            // 
            this.imgCanny.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgCanny.Location = new System.Drawing.Point(3, 16);
            this.imgCanny.Name = "imgCanny";
            this.imgCanny.Size = new System.Drawing.Size(413, 307);
            this.imgCanny.TabIndex = 2;
            this.imgCanny.TabStop = false;
            // 
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1316, 493);
            this.Controls.Add(this.grpCanny);
            this.Controls.Add(this.grpSmoothener);
            this.Controls.Add(this.gbVideoSource);
            this.Name = "ViewForm";
            this.Text = "ViewForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewForm_FormClosing);
            this.gbVideoSource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgVideoSource)).EndInit();
            this.grpSmoothener.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgSmoothener)).EndInit();
            this.grpCanny.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgCanny)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbVideoSource;
        private Emgu.CV.UI.ImageBox imgVideoSource;
        private System.Windows.Forms.GroupBox grpSmoothener;
        private Emgu.CV.UI.ImageBox imgSmoothener;
        private System.Windows.Forms.GroupBox grpCanny;
        private Emgu.CV.UI.ImageBox imgCanny;
    }
}