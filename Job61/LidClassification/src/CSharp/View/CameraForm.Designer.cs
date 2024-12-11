namespace LidClassification.View
{
    partial class CameraForm
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
            pictureBox1 = new PictureBox();
            btnStart = new Button();
            btnDetect = new Button();
            pictureBox2 = new PictureBox();
            lbProcessed = new Label();
            lbTimeProcess = new Label();
            txbCheckLid = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(662, 533);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 645);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(143, 56);
            btnStart.TabIndex = 1;
            btnStart.Text = "Bắt đầu chụp";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnDetect
            // 
            btnDetect.Location = new Point(157, 645);
            btnDetect.Name = "btnDetect";
            btnDetect.Size = new Size(143, 56);
            btnDetect.TabIndex = 2;
            btnDetect.Text = "Nhận diện";
            btnDetect.UseVisualStyleBackColor = true;
            btnDetect.Click += btnDetect_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new Point(680, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(735, 533);
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // lbProcessed
            // 
            lbProcessed.AccessibleRole = AccessibleRole.None;
            lbProcessed.AutoSize = true;
            lbProcessed.Location = new Point(1189, 671);
            lbProcessed.Name = "lbProcessed";
            lbProcessed.Size = new Size(63, 20);
            lbProcessed.TabIndex = 4;
            lbProcessed.Text = "Đã xử lý";
            // 
            // lbTimeProcess
            // 
            lbTimeProcess.AccessibleRole = AccessibleRole.None;
            lbTimeProcess.AutoSize = true;
            lbTimeProcess.Location = new Point(844, 671);
            lbTimeProcess.Name = "lbTimeProcess";
            lbTimeProcess.Size = new Size(113, 20);
            lbTimeProcess.TabIndex = 5;
            lbTimeProcess.Text = "Thời gian xử lý: ";
            // 
            // txbCheckLid
            // 
            txbCheckLid.Location = new Point(583, 664);
            txbCheckLid.Name = "txbCheckLid";
            txbCheckLid.ReadOnly = true;
            txbCheckLid.Size = new Size(193, 27);
            txbCheckLid.TabIndex = 6;
            // 
            // CameraForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1427, 713);
            Controls.Add(txbCheckLid);
            Controls.Add(lbTimeProcess);
            Controls.Add(lbProcessed);
            Controls.Add(pictureBox2);
            Controls.Add(btnDetect);
            Controls.Add(btnStart);
            Controls.Add(pictureBox1);
            Name = "CameraForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Camera Form";
            FormClosing += CameraForm_FormClosing;
            Load += CameraForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button btnStart;
        private Button btnDetect;
        private PictureBox pictureBox2;
        private Label lbProcessed;
        private Label lbTimeProcess;
        private TextBox txbCheckLid;
    }
}