namespace BarcodeGenerator
{
    partial class GeneratorForm
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
            this.grpTop = new System.Windows.Forms.GroupBox();
            this.topText = new System.Windows.Forms.TextBox();
            this.topImagePath = new System.Windows.Forms.Label();
            this.topImageBrowse = new System.Windows.Forms.Button();
            this.topNumber = new System.Windows.Forms.NumericUpDown();
            this.topRadioText = new System.Windows.Forms.RadioButton();
            this.topRadioImage = new System.Windows.Forms.RadioButton();
            this.topRadioBarcode = new System.Windows.Forms.RadioButton();
            this.grpBottom = new System.Windows.Forms.GroupBox();
            this.bottomText = new System.Windows.Forms.TextBox();
            this.bottomImagePath = new System.Windows.Forms.Label();
            this.bottomImageBrowse = new System.Windows.Forms.Button();
            this.bottomNumber = new System.Windows.Forms.NumericUpDown();
            this.bottomRadioText = new System.Windows.Forms.RadioButton();
            this.bottomRadioImage = new System.Windows.Forms.RadioButton();
            this.bottomRadioBarcode = new System.Windows.Forms.RadioButton();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bottomImageFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.topFontSize = new System.Windows.Forms.NumericUpDown();
            this.bottomFontSize = new System.Windows.Forms.NumericUpDown();
            this.grpTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.topNumber)).BeginInit();
            this.grpBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bottomNumber)).BeginInit();
            this.grpPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topFontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottomFontSize)).BeginInit();
            this.SuspendLayout();
            // 
            // grpTop
            // 
            this.grpTop.Controls.Add(this.topFontSize);
            this.grpTop.Controls.Add(this.topText);
            this.grpTop.Controls.Add(this.topImagePath);
            this.grpTop.Controls.Add(this.topImageBrowse);
            this.grpTop.Controls.Add(this.topNumber);
            this.grpTop.Controls.Add(this.topRadioText);
            this.grpTop.Controls.Add(this.topRadioImage);
            this.grpTop.Controls.Add(this.topRadioBarcode);
            this.grpTop.Location = new System.Drawing.Point(12, 12);
            this.grpTop.Name = "grpTop";
            this.grpTop.Size = new System.Drawing.Size(352, 121);
            this.grpTop.TabIndex = 0;
            this.grpTop.TabStop = false;
            this.grpTop.Text = "Top";
            // 
            // topText
            // 
            this.topText.Location = new System.Drawing.Point(89, 85);
            this.topText.Name = "topText";
            this.topText.Size = new System.Drawing.Size(177, 20);
            this.topText.TabIndex = 6;
            // 
            // topImagePath
            // 
            this.topImagePath.AutoSize = true;
            this.topImagePath.Location = new System.Drawing.Point(170, 61);
            this.topImagePath.Name = "topImagePath";
            this.topImagePath.Size = new System.Drawing.Size(35, 13);
            this.topImagePath.TabIndex = 5;
            this.topImagePath.Text = "label1";
            // 
            // topImageBrowse
            // 
            this.topImageBrowse.Location = new System.Drawing.Point(89, 56);
            this.topImageBrowse.Name = "topImageBrowse";
            this.topImageBrowse.Size = new System.Drawing.Size(75, 23);
            this.topImageBrowse.TabIndex = 4;
            this.topImageBrowse.Text = "button1";
            this.topImageBrowse.UseVisualStyleBackColor = true;
            // 
            // topNumber
            // 
            this.topNumber.Location = new System.Drawing.Point(89, 30);
            this.topNumber.Name = "topNumber";
            this.topNumber.Size = new System.Drawing.Size(245, 20);
            this.topNumber.TabIndex = 3;
            this.topNumber.ValueChanged += new System.EventHandler(this.topNumber_ValueChanged);
            // 
            // topRadioText
            // 
            this.topRadioText.AutoSize = true;
            this.topRadioText.Location = new System.Drawing.Point(17, 86);
            this.topRadioText.Name = "topRadioText";
            this.topRadioText.Size = new System.Drawing.Size(46, 17);
            this.topRadioText.TabIndex = 2;
            this.topRadioText.TabStop = true;
            this.topRadioText.Text = "Text";
            this.topRadioText.UseVisualStyleBackColor = true;
            // 
            // topRadioImage
            // 
            this.topRadioImage.AutoSize = true;
            this.topRadioImage.Location = new System.Drawing.Point(17, 59);
            this.topRadioImage.Name = "topRadioImage";
            this.topRadioImage.Size = new System.Drawing.Size(54, 17);
            this.topRadioImage.TabIndex = 1;
            this.topRadioImage.TabStop = true;
            this.topRadioImage.Text = "Image";
            this.topRadioImage.UseVisualStyleBackColor = true;
            // 
            // topRadioBarcode
            // 
            this.topRadioBarcode.AutoSize = true;
            this.topRadioBarcode.Location = new System.Drawing.Point(17, 30);
            this.topRadioBarcode.Name = "topRadioBarcode";
            this.topRadioBarcode.Size = new System.Drawing.Size(65, 17);
            this.topRadioBarcode.TabIndex = 0;
            this.topRadioBarcode.TabStop = true;
            this.topRadioBarcode.Text = "Barcode";
            this.topRadioBarcode.UseVisualStyleBackColor = true;
            // 
            // grpBottom
            // 
            this.grpBottom.Controls.Add(this.bottomFontSize);
            this.grpBottom.Controls.Add(this.bottomText);
            this.grpBottom.Controls.Add(this.bottomImagePath);
            this.grpBottom.Controls.Add(this.bottomImageBrowse);
            this.grpBottom.Controls.Add(this.bottomNumber);
            this.grpBottom.Controls.Add(this.bottomRadioText);
            this.grpBottom.Controls.Add(this.bottomRadioImage);
            this.grpBottom.Controls.Add(this.bottomRadioBarcode);
            this.grpBottom.Location = new System.Drawing.Point(12, 139);
            this.grpBottom.Name = "grpBottom";
            this.grpBottom.Size = new System.Drawing.Size(352, 121);
            this.grpBottom.TabIndex = 7;
            this.grpBottom.TabStop = false;
            this.grpBottom.Text = "Bottom";
            // 
            // bottomText
            // 
            this.bottomText.Location = new System.Drawing.Point(89, 85);
            this.bottomText.Name = "bottomText";
            this.bottomText.Size = new System.Drawing.Size(177, 20);
            this.bottomText.TabIndex = 6;
            // 
            // bottomImagePath
            // 
            this.bottomImagePath.AutoSize = true;
            this.bottomImagePath.Location = new System.Drawing.Point(170, 61);
            this.bottomImagePath.Name = "bottomImagePath";
            this.bottomImagePath.Size = new System.Drawing.Size(35, 13);
            this.bottomImagePath.TabIndex = 5;
            this.bottomImagePath.Text = "label2";
            // 
            // bottomImageBrowse
            // 
            this.bottomImageBrowse.Location = new System.Drawing.Point(89, 56);
            this.bottomImageBrowse.Name = "bottomImageBrowse";
            this.bottomImageBrowse.Size = new System.Drawing.Size(75, 23);
            this.bottomImageBrowse.TabIndex = 4;
            this.bottomImageBrowse.Text = "button2";
            this.bottomImageBrowse.UseVisualStyleBackColor = true;
            this.bottomImageBrowse.Click += new System.EventHandler(this.bottomImageBrowse_Click);
            // 
            // bottomNumber
            // 
            this.bottomNumber.Location = new System.Drawing.Point(89, 30);
            this.bottomNumber.Name = "bottomNumber";
            this.bottomNumber.Size = new System.Drawing.Size(245, 20);
            this.bottomNumber.TabIndex = 3;
            // 
            // bottomRadioText
            // 
            this.bottomRadioText.AutoSize = true;
            this.bottomRadioText.Location = new System.Drawing.Point(17, 86);
            this.bottomRadioText.Name = "bottomRadioText";
            this.bottomRadioText.Size = new System.Drawing.Size(46, 17);
            this.bottomRadioText.TabIndex = 2;
            this.bottomRadioText.TabStop = true;
            this.bottomRadioText.Text = "Text";
            this.bottomRadioText.UseVisualStyleBackColor = true;
            // 
            // bottomRadioImage
            // 
            this.bottomRadioImage.AutoSize = true;
            this.bottomRadioImage.Location = new System.Drawing.Point(17, 59);
            this.bottomRadioImage.Name = "bottomRadioImage";
            this.bottomRadioImage.Size = new System.Drawing.Size(54, 17);
            this.bottomRadioImage.TabIndex = 1;
            this.bottomRadioImage.TabStop = true;
            this.bottomRadioImage.Text = "Image";
            this.bottomRadioImage.UseVisualStyleBackColor = true;
            // 
            // bottomRadioBarcode
            // 
            this.bottomRadioBarcode.AutoSize = true;
            this.bottomRadioBarcode.Checked = true;
            this.bottomRadioBarcode.Location = new System.Drawing.Point(17, 30);
            this.bottomRadioBarcode.Name = "bottomRadioBarcode";
            this.bottomRadioBarcode.Size = new System.Drawing.Size(65, 17);
            this.bottomRadioBarcode.TabIndex = 0;
            this.bottomRadioBarcode.TabStop = true;
            this.bottomRadioBarcode.Text = "Barcode";
            this.bottomRadioBarcode.UseVisualStyleBackColor = true;
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(13, 267);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(146, 23);
            this.btnPreview.TabIndex = 8;
            this.btnPreview.Text = "Generate && Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(217, 267);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(147, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Generate && Save...";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grpPreview
            // 
            this.grpPreview.Controls.Add(this.pictureBox1);
            this.grpPreview.Location = new System.Drawing.Point(371, 12);
            this.grpPreview.Name = "grpPreview";
            this.grpPreview.Size = new System.Drawing.Size(414, 278);
            this.grpPreview.TabIndex = 10;
            this.grpPreview.TabStop = false;
            this.grpPreview.Text = "Preview";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(408, 259);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // topFontSize
            // 
            this.topFontSize.DecimalPlaces = 2;
            this.topFontSize.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.topFontSize.Location = new System.Drawing.Point(272, 85);
            this.topFontSize.Name = "topFontSize";
            this.topFontSize.Size = new System.Drawing.Size(62, 20);
            this.topFontSize.TabIndex = 7;
            this.topFontSize.ValueChanged += new System.EventHandler(this.topFontSize_ValueChanged);
            // 
            // bottomFontSize
            // 
            this.bottomFontSize.DecimalPlaces = 2;
            this.bottomFontSize.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.bottomFontSize.Location = new System.Drawing.Point(272, 85);
            this.bottomFontSize.Name = "bottomFontSize";
            this.bottomFontSize.Size = new System.Drawing.Size(62, 20);
            this.bottomFontSize.TabIndex = 8;
            // 
            // GeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 295);
            this.Controls.Add(this.grpPreview);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.grpBottom);
            this.Controls.Add(this.grpTop);
            this.Name = "GeneratorForm";
            this.Text = "MBarcode Generator";
            this.grpTop.ResumeLayout(false);
            this.grpTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.topNumber)).EndInit();
            this.grpBottom.ResumeLayout(false);
            this.grpBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bottomNumber)).EndInit();
            this.grpPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topFontSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottomFontSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpTop;
        private System.Windows.Forms.RadioButton topRadioText;
        private System.Windows.Forms.RadioButton topRadioImage;
        private System.Windows.Forms.RadioButton topRadioBarcode;
        private System.Windows.Forms.TextBox topText;
        private System.Windows.Forms.Label topImagePath;
        private System.Windows.Forms.Button topImageBrowse;
        private System.Windows.Forms.NumericUpDown topNumber;
        private System.Windows.Forms.GroupBox grpBottom;
        private System.Windows.Forms.TextBox bottomText;
        private System.Windows.Forms.Label bottomImagePath;
        private System.Windows.Forms.Button bottomImageBrowse;
        private System.Windows.Forms.NumericUpDown bottomNumber;
        private System.Windows.Forms.RadioButton bottomRadioText;
        private System.Windows.Forms.RadioButton bottomRadioImage;
        private System.Windows.Forms.RadioButton bottomRadioBarcode;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox grpPreview;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.OpenFileDialog bottomImageFileDialog;
        private System.Windows.Forms.SaveFileDialog saveImageDialog;
        private System.Windows.Forms.NumericUpDown topFontSize;
        private System.Windows.Forms.NumericUpDown bottomFontSize;

    }
}

