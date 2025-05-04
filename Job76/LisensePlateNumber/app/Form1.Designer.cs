namespace LisensePlateNumber
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
            btnRecognize = new Button();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            txbLPN = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnChooseImage
            // 
            btnChooseImage.Location = new Point(12, 12);
            btnChooseImage.Name = "btnChooseImage";
            btnChooseImage.Size = new Size(94, 50);
            btnChooseImage.TabIndex = 0;
            btnChooseImage.Text = "Chọn ảnh";
            btnChooseImage.UseVisualStyleBackColor = true;
            btnChooseImage.Click += btnChooseImage_Click;
            // 
            // btnRecognize
            // 
            btnRecognize.Location = new Point(112, 12);
            btnRecognize.Name = "btnRecognize";
            btnRecognize.Size = new Size(94, 50);
            btnRecognize.TabIndex = 1;
            btnRecognize.Text = "Nhận diện";
            btnRecognize.UseVisualStyleBackColor = true;
            btnRecognize.Click += btnRecognize_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = SystemColors.ButtonFace;
            pictureBox1.Location = new Point(212, 15);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(576, 423);
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 91);
            label2.Name = "label2";
            label2.Size = new Size(145, 20);
            label2.TabIndex = 4;
            label2.Text = "Biển số xe nhận diện";
            // 
            // txbLPN
            // 
            txbLPN.Font = new Font("Segoe UI", 14F);
            txbLPN.Location = new Point(20, 114);
            txbLPN.Name = "txbLPN";
            txbLPN.ReadOnly = true;
            txbLPN.Size = new Size(186, 39);
            txbLPN.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGray;
            ClientSize = new Size(800, 450);
            Controls.Add(txbLPN);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            Controls.Add(btnRecognize);
            Controls.Add(btnChooseImage);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnChooseImage;
        private Button btnRecognize;
        private PictureBox pictureBox1;
        private Label label2;
        private TextBox txbLPN;
    }
}
