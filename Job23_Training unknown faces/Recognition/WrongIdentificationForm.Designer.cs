namespace Recognition
{
    partial class WrongIdentificationForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtgvReport = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpk = new System.Windows.Forms.DateTimePicker();
            this.btnShowAll = new System.Windows.Forms.Button();
            this.STT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImageIdentification = new System.Windows.Forms.DataGridViewImageColumn();
            this.WrongIdentificationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CorrectIdentificationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnTrain = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvReport)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgvReport
            // 
            this.dtgvReport.AllowUserToAddRows = false;
            this.dtgvReport.AllowUserToDeleteRows = false;
            this.dtgvReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgvReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgvReport.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgvReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvReport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.STT,
            this.ImageIdentification,
            this.WrongIdentificationName,
            this.CorrectIdentificationName,
            this.btnTrain});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgvReport.DefaultCellStyle = dataGridViewCellStyle2;
            this.dtgvReport.Location = new System.Drawing.Point(0, 80);
            this.dtgvReport.Name = "dtgvReport";
            this.dtgvReport.RowHeadersWidth = 51;
            this.dtgvReport.RowTemplate.Height = 24;
            this.dtgvReport.Size = new System.Drawing.Size(1189, 617);
            this.dtgvReport.TabIndex = 0;
            this.dtgvReport.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgvReport_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Chọn ngày";
            // 
            // dtpk
            // 
            this.dtpk.Location = new System.Drawing.Point(90, 10);
            this.dtpk.Name = "dtpk";
            this.dtpk.Size = new System.Drawing.Size(200, 22);
            this.dtpk.TabIndex = 2;
            // 
            // btnShowAll
            // 
            this.btnShowAll.Location = new System.Drawing.Point(331, 11);
            this.btnShowAll.Name = "btnShowAll";
            this.btnShowAll.Size = new System.Drawing.Size(75, 23);
            this.btnShowAll.TabIndex = 3;
            this.btnShowAll.Text = "Tất cả";
            this.btnShowAll.UseVisualStyleBackColor = true;
            this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);
            // 
            // STT
            // 
            this.STT.FillWeight = 50.05905F;
            this.STT.HeaderText = "STT";
            this.STT.MinimumWidth = 6;
            this.STT.Name = "STT";
            this.STT.ReadOnly = true;
            this.STT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ImageIdentification
            // 
            this.ImageIdentification.FillWeight = 221.5113F;
            this.ImageIdentification.HeaderText = "Ảnh nhận diện";
            this.ImageIdentification.MinimumWidth = 6;
            this.ImageIdentification.Name = "ImageIdentification";
            this.ImageIdentification.ReadOnly = true;
            // 
            // WrongIdentificationName
            // 
            this.WrongIdentificationName.FillWeight = 53.47593F;
            this.WrongIdentificationName.HeaderText = "Tên nhận diện sai";
            this.WrongIdentificationName.MinimumWidth = 6;
            this.WrongIdentificationName.Name = "WrongIdentificationName";
            this.WrongIdentificationName.ReadOnly = true;
            this.WrongIdentificationName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CorrectIdentificationName
            // 
            this.CorrectIdentificationName.FillWeight = 89.37374F;
            this.CorrectIdentificationName.HeaderText = "Tên nhận diện đúng";
            this.CorrectIdentificationName.MinimumWidth = 6;
            this.CorrectIdentificationName.Name = "CorrectIdentificationName";
            this.CorrectIdentificationName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btnTrain
            // 
            this.btnTrain.FillWeight = 85.57997F;
            this.btnTrain.HeaderText = "Tùy chọn";
            this.btnTrain.MinimumWidth = 6;
            this.btnTrain.Name = "btnTrain";
            // 
            // WrongIdentificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1189, 697);
            this.Controls.Add(this.btnShowAll);
            this.Controls.Add(this.dtpk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtgvReport);
            this.Name = "WrongIdentificationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WrongIdentificationForm";
            ((System.ComponentModel.ISupportInitialize)(this.dtgvReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dtgvReport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpk;
        private System.Windows.Forms.Button btnShowAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn STT;
        private System.Windows.Forms.DataGridViewImageColumn ImageIdentification;
        private System.Windows.Forms.DataGridViewTextBoxColumn WrongIdentificationName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CorrectIdentificationName;
        private System.Windows.Forms.DataGridViewButtonColumn btnTrain;
    }
}