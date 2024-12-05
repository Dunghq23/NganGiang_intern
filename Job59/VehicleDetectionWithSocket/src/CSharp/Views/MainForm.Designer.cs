namespace VehicleDetection_8._0_
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            wmpVideo = new AxWMPLib.AxWindowsMediaPlayer();
            pictureBox1 = new PictureBox();
            panel2 = new Panel();
            btnExtractImages = new Button();
            btnSelectFile = new Button();
            nmrframeSkip = new NumericUpDown();
            label5 = new Label();
            panel1 = new Panel();
            dataGridView1 = new DataGridView();
            lbProcessed = new Label();
            label3 = new Label();
            lbTotalVehicles = new Label();
            lbTotalTime = new Label();
            ((System.ComponentModel.ISupportInitialize)wmpVideo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrframeSkip).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // wmpVideo
            // 
            wmpVideo.Enabled = true;
            wmpVideo.Location = new Point(23, 345);
            wmpVideo.Name = "wmpVideo";
            wmpVideo.OcxState = (AxHost.State)resources.GetObject("wmpVideo.OcxState");
            wmpVideo.Size = new Size(450, 297);
            wmpVideo.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(508, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(878, 734);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnExtractImages);
            panel2.Controls.Add(btnSelectFile);
            panel2.Controls.Add(nmrframeSkip);
            panel2.Controls.Add(label5);
            panel2.Location = new Point(12, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(490, 83);
            panel2.TabIndex = 3;
            // 
            // btnExtractImages
            // 
            btnExtractImages.Location = new Point(353, 11);
            btnExtractImages.Name = "btnExtractImages";
            btnExtractImages.Size = new Size(120, 59);
            btnExtractImages.TabIndex = 3;
            btnExtractImages.Text = "Trích xuất ảnh";
            btnExtractImages.UseVisualStyleBackColor = true;
            btnExtractImages.Click += btnExtractImages_Click;
            // 
            // btnSelectFile
            // 
            btnSelectFile.Location = new Point(184, 11);
            btnSelectFile.Name = "btnSelectFile";
            btnSelectFile.Size = new Size(120, 59);
            btnSelectFile.TabIndex = 2;
            btnSelectFile.Text = "Chọn File";
            btnSelectFile.UseVisualStyleBackColor = true;
            btnSelectFile.Click += btnSelectFile_Click;
            // 
            // nmrframeSkip
            // 
            nmrframeSkip.Location = new Point(18, 43);
            nmrframeSkip.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nmrframeSkip.Name = "nmrframeSkip";
            nmrframeSkip.Size = new Size(130, 27);
            nmrframeSkip.TabIndex = 1;
            nmrframeSkip.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nmrframeSkip.ValueChanged += nmrframeSkip_ValueChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(16, 15);
            label5.Name = "label5";
            label5.Size = new Size(132, 20);
            label5.TabIndex = 0;
            label5.Text = "Bỏ qua khung hình";
            // 
            // panel1
            // 
            panel1.Controls.Add(dataGridView1);
            panel1.Controls.Add(lbProcessed);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(wmpVideo);
            panel1.Controls.Add(lbTotalVehicles);
            panel1.Controls.Add(lbTotalTime);
            panel1.Location = new Point(12, 101);
            panel1.Name = "panel1";
            panel1.Size = new Size(490, 645);
            panel1.TabIndex = 4;
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(23, 100);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(450, 195);
            dataGridView1.TabIndex = 4;
            // 
            // lbProcessed
            // 
            lbProcessed.AutoSize = true;
            lbProcessed.Location = new Point(23, 309);
            lbProcessed.Name = "lbProcessed";
            lbProcessed.Size = new Size(63, 20);
            lbProcessed.TabIndex = 3;
            lbProcessed.Text = "Đã xử lý";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(19, 70);
            label3.Name = "label3";
            label3.Size = new Size(111, 20);
            label3.TabIndex = 2;
            label3.Text = "Số phương tiện";
            // 
            // lbTotalVehicles
            // 
            lbTotalVehicles.AutoSize = true;
            lbTotalVehicles.Location = new Point(18, 40);
            lbTotalVehicles.Name = "lbTotalVehicles";
            lbTotalVehicles.Size = new Size(147, 20);
            lbTotalVehicles.TabIndex = 1;
            lbTotalVehicles.Text = "Tổng số phương tiện";
            // 
            // lbTotalTime
            // 
            lbTotalTime.AutoSize = true;
            lbTotalTime.Location = new Point(19, 9);
            lbTotalTime.Name = "lbTotalTime";
            lbTotalTime.Size = new Size(171, 20);
            lbTotalTime.TabIndex = 0;
            lbTotalTime.Text = "Tổng thời gian thực hiện";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1398, 758);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Controls.Add(pictureBox1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)wmpVideo).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nmrframeSkip).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer wmpVideo;
        private PictureBox pictureBox1;
        private Panel panel2;
        private Button button4;
        private Button btnExtractImages;
        private Button btnSelectFile;
        private NumericUpDown nmrframeSkip;
        private Label label5;
        private Panel panel1;
        private DataGridView dataGridView1;
        private Label lbProcessed;
        private Label label3;
        private Label lbTotalVehicles;
        private Label lbTotalTime;
    }
}
