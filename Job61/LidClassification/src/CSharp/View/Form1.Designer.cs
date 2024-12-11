namespace LidClassification
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnChooseImage = new Button();
            pbOriginal = new PictureBox();
            pbGray = new PictureBox();
            btnToGray = new Button();
            pbHistogramGray = new PictureBox();
            pbHistogramRed = new PictureBox();
            pbHistogramGreen = new PictureBox();
            pbHistogramBlue = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)pbOriginal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbGray).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbHistogramGray).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbHistogramRed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbHistogramGreen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbHistogramBlue).BeginInit();
            SuspendLayout();
            // 
            // btnChooseImage
            // 
            btnChooseImage.Location = new Point(12, 12);
            btnChooseImage.Name = "btnChooseImage";
            btnChooseImage.Size = new Size(94, 29);
            btnChooseImage.TabIndex = 0;
            btnChooseImage.Text = "Chọn ảnh";
            btnChooseImage.UseVisualStyleBackColor = true;
            btnChooseImage.Click += btnChooseImage_Click;
            // 
            // pbOriginal
            // 
            pbOriginal.Location = new Point(13, 49);
            pbOriginal.Name = "pbOriginal";
            pbOriginal.Size = new Size(719, 507);
            pbOriginal.TabIndex = 1;
            pbOriginal.TabStop = false;
            // 
            // pbGray
            // 
            pbGray.Location = new Point(738, 49);
            pbGray.Name = "pbGray";
            pbGray.Size = new Size(710, 507);
            pbGray.TabIndex = 2;
            pbGray.TabStop = false;
            // 
            // btnToGray
            // 
            btnToGray.Location = new Point(112, 12);
            btnToGray.Name = "btnToGray";
            btnToGray.Size = new Size(177, 29);
            btnToGray.TabIndex = 3;
            btnToGray.Text = "Chuyển sang ảnh xám";
            btnToGray.UseVisualStyleBackColor = true;
            btnToGray.Click += btnToGray_Click;
            // 
            // pbHistogramGray
            // 
            pbHistogramGray.BackColor = SystemColors.ButtonFace;
            pbHistogramGray.Location = new Point(12, 580);
            pbHistogramGray.Name = "pbHistogramGray";
            pbHistogramGray.Size = new Size(351, 145);
            pbHistogramGray.TabIndex = 5;
            pbHistogramGray.TabStop = false;
            // 
            // pbHistogramRed
            // 
            pbHistogramRed.Location = new Point(369, 580);
            pbHistogramRed.Name = "pbHistogramRed";
            pbHistogramRed.Size = new Size(351, 145);
            pbHistogramRed.TabIndex = 6;
            pbHistogramRed.TabStop = false;
            // 
            // pbHistogramGreen
            // 
            pbHistogramGreen.Location = new Point(726, 580);
            pbHistogramGreen.Name = "pbHistogramGreen";
            pbHistogramGreen.Size = new Size(351, 145);
            pbHistogramGreen.TabIndex = 7;
            pbHistogramGreen.TabStop = false;
            // 
            // pbHistogramBlue
            // 
            pbHistogramBlue.Location = new Point(1083, 580);
            pbHistogramBlue.Name = "pbHistogramBlue";
            pbHistogramBlue.Size = new Size(351, 145);
            pbHistogramBlue.TabIndex = 8;
            pbHistogramBlue.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 558);
            label1.Name = "label1";
            label1.Size = new Size(107, 20);
            label1.TabIndex = 9;
            label1.Text = "Kênh màu xám";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(369, 558);
            label2.Name = "label2";
            label2.Size = new Size(97, 20);
            label2.TabIndex = 10;
            label2.Text = "Kênh màu đỏ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(726, 557);
            label3.Name = "label3";
            label3.Size = new Size(126, 20);
            label3.TabIndex = 11;
            label3.Text = "Kênh màu xanh lá";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(1083, 557);
            label4.Name = "label4";
            label4.Size = new Size(158, 20);
            label4.TabIndex = 12;
            label4.Text = "Kênh màu xanh dương";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1460, 737);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pbHistogramBlue);
            Controls.Add(pbHistogramGreen);
            Controls.Add(pbHistogramRed);
            Controls.Add(pbHistogramGray);
            Controls.Add(btnToGray);
            Controls.Add(pbGray);
            Controls.Add(pbOriginal);
            Controls.Add(btnChooseImage);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pbOriginal).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbGray).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbHistogramGray).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbHistogramRed).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbHistogramGreen).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbHistogramBlue).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnChooseImage;
        private PictureBox pbOriginal;
        private PictureBox pbGray;
        private Button btnToGray;
        private PictureBox pbHistogramGray;
        private PictureBox pbHistogramRed;
        private PictureBox pbHistogramGreen;
        private PictureBox pbHistogramBlue;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}
