namespace Recognition
{
    partial class TrainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txbName = new System.Windows.Forms.TextBox();
            this.btnOpenImageTrain = new System.Windows.Forms.Button();
            this.picBoxTrain = new System.Windows.Forms.PictureBox();
            this.btnTrain = new System.Windows.Forms.Button();
            this.ofdTrain = new System.Windows.Forms.OpenFileDialog();
            this.btnCamera = new System.Windows.Forms.Button();
            this.btnCaptureImage = new System.Windows.Forms.Button();
            this.nudNumberOfPhotos = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTrain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberOfPhotos)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Nhập tên";
            // 
            // txbName
            // 
            this.txbName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txbName.Location = new System.Drawing.Point(104, 12);
            this.txbName.Name = "txbName";
            this.txbName.Size = new System.Drawing.Size(239, 22);
            this.txbName.TabIndex = 7;
            // 
            // btnOpenImageTrain
            // 
            this.btnOpenImageTrain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenImageTrain.Location = new System.Drawing.Point(22, 478);
            this.btnOpenImageTrain.Name = "btnOpenImageTrain";
            this.btnOpenImageTrain.Size = new System.Drawing.Size(138, 38);
            this.btnOpenImageTrain.TabIndex = 6;
            this.btnOpenImageTrain.Text = "Chọn ảnh";
            this.btnOpenImageTrain.UseVisualStyleBackColor = true;
            this.btnOpenImageTrain.Click += new System.EventHandler(this.btnOpenImageTrain_Click);
            // 
            // picBoxTrain
            // 
            this.picBoxTrain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picBoxTrain.BackColor = System.Drawing.Color.Transparent;
            this.picBoxTrain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picBoxTrain.Location = new System.Drawing.Point(22, 40);
            this.picBoxTrain.Name = "picBoxTrain";
            this.picBoxTrain.Size = new System.Drawing.Size(808, 432);
            this.picBoxTrain.TabIndex = 5;
            this.picBoxTrain.TabStop = false;
            // 
            // btnTrain
            // 
            this.btnTrain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTrain.Location = new System.Drawing.Point(692, 478);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(138, 38);
            this.btnTrain.TabIndex = 9;
            this.btnTrain.Text = "Huấn luyện";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // ofdTrain
            // 
            this.ofdTrain.FileName = "openFileDialog1";
            // 
            // btnCamera
            // 
            this.btnCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCamera.Location = new System.Drawing.Point(166, 478);
            this.btnCamera.Name = "btnCamera";
            this.btnCamera.Size = new System.Drawing.Size(138, 38);
            this.btnCamera.TabIndex = 10;
            this.btnCamera.Text = "Camera";
            this.btnCamera.UseVisualStyleBackColor = true;
            this.btnCamera.Click += new System.EventHandler(this.btnCamera_Click);
            // 
            // btnCaptureImage
            // 
            this.btnCaptureImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCaptureImage.Location = new System.Drawing.Point(310, 478);
            this.btnCaptureImage.Name = "btnCaptureImage";
            this.btnCaptureImage.Size = new System.Drawing.Size(138, 38);
            this.btnCaptureImage.TabIndex = 11;
            this.btnCaptureImage.Text = "Chụp ảnh";
            this.btnCaptureImage.UseVisualStyleBackColor = true;
            this.btnCaptureImage.Click += new System.EventHandler(this.btnCaptureImage_Click);
            // 
            // nudNumberOfPhotos
            // 
            this.nudNumberOfPhotos.Location = new System.Drawing.Point(772, 9);
            this.nudNumberOfPhotos.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudNumberOfPhotos.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudNumberOfPhotos.Name = "nudNumberOfPhotos";
            this.nudNumberOfPhotos.Size = new System.Drawing.Size(58, 22);
            this.nudNumberOfPhotos.TabIndex = 12;
            this.nudNumberOfPhotos.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(617, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "Số lượng ảnh cần chụp";
            // 
            // TrainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 528);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudNumberOfPhotos);
            this.Controls.Add(this.btnCaptureImage);
            this.Controls.Add(this.btnCamera);
            this.Controls.Add(this.btnTrain);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbName);
            this.Controls.Add(this.btnOpenImageTrain);
            this.Controls.Add(this.picBoxTrain);
            this.Name = "TrainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Huấn luyện khuôn mặt";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Train_FormClosing);
            this.Load += new System.EventHandler(this.TrainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTrain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberOfPhotos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbName;
        private System.Windows.Forms.Button btnOpenImageTrain;
        private System.Windows.Forms.PictureBox picBoxTrain;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.OpenFileDialog ofdTrain;
        private System.Windows.Forms.Button btnCamera;
        private System.Windows.Forms.Button btnCaptureImage;
        private System.Windows.Forms.NumericUpDown nudNumberOfPhotos;
        private System.Windows.Forms.Label label2;
    }
}