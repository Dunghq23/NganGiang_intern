namespace Management.Views.EducationManagement
{
    partial class LessonSubjectForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtgvSubjectsList = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAddLessonSubject = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dtgvLessonList = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvSubjectsList)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvLessonList)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.dtgvSubjectsList);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 728);
            this.panel1.TabIndex = 2;
            // 
            // dtgvSubjectsList
            // 
            this.dtgvSubjectsList.AllowUserToAddRows = false;
            this.dtgvSubjectsList.AllowUserToDeleteRows = false;
            this.dtgvSubjectsList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dtgvSubjectsList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgvSubjectsList.BackgroundColor = System.Drawing.Color.White;
            this.dtgvSubjectsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvSubjectsList.Location = new System.Drawing.Point(-11, 85);
            this.dtgvSubjectsList.Margin = new System.Windows.Forms.Padding(4);
            this.dtgvSubjectsList.Name = "dtgvSubjectsList";
            this.dtgvSubjectsList.ReadOnly = true;
            this.dtgvSubjectsList.RowHeadersVisible = false;
            this.dtgvSubjectsList.RowHeadersWidth = 51;
            this.dtgvSubjectsList.RowTemplate.Height = 24;
            this.dtgvSubjectsList.Size = new System.Drawing.Size(450, 643);
            this.dtgvSubjectsList.TabIndex = 1;
            this.dtgvSubjectsList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgvSubjectsList_CellClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(69, 24);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(290, 29);
            this.label3.TabIndex = 5;
            this.label3.Text = "DANH SÁCH MÔN HỌC";
            // 
            // btnAddLessonSubject
            // 
            this.btnAddLessonSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddLessonSubject.Location = new System.Drawing.Point(4, 4);
            this.btnAddLessonSubject.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddLessonSubject.Name = "btnAddLessonSubject";
            this.btnAddLessonSubject.Size = new System.Drawing.Size(222, 74);
            this.btnAddLessonSubject.TabIndex = 2;
            this.btnAddLessonSubject.Text = "Thêm bài học";
            this.btnAddLessonSubject.UseVisualStyleBackColor = true;
            this.btnAddLessonSubject.Click += new System.EventHandler(this.btnAddLessonSubject_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.dtgvLessonList);
            this.panel2.Controls.Add(this.btnAddLessonSubject);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(433, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1242, 728);
            this.panel2.TabIndex = 3;
            // 
            // dtgvLessonList
            // 
            this.dtgvLessonList.AllowUserToAddRows = false;
            this.dtgvLessonList.AllowUserToDeleteRows = false;
            this.dtgvLessonList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgvLessonList.BackgroundColor = System.Drawing.Color.White;
            this.dtgvLessonList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvLessonList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8});
            this.dtgvLessonList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dtgvLessonList.Location = new System.Drawing.Point(0, 86);
            this.dtgvLessonList.Margin = new System.Windows.Forms.Padding(4);
            this.dtgvLessonList.Name = "dtgvLessonList";
            this.dtgvLessonList.ReadOnly = true;
            this.dtgvLessonList.RowHeadersVisible = false;
            this.dtgvLessonList.RowHeadersWidth = 51;
            this.dtgvLessonList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dtgvLessonList.RowTemplate.Height = 24;
            this.dtgvLessonList.Size = new System.Drawing.Size(1242, 642);
            this.dtgvLessonList.TabIndex = 0;
            this.dtgvLessonList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgvLessonList_CellClick);
            // 
            // Column1
            // 
            this.Column1.FillWeight = 42.78075F;
            this.Column1.HeaderText = "Ký hiệu môn học";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.FillWeight = 108.1742F;
            this.Column2.HeaderText = "Bài học";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.FillWeight = 108.1742F;
            this.Column3.HeaderText = "Tên bài học";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.FillWeight = 108.1742F;
            this.Column4.HeaderText = "Lý thuyết";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.FillWeight = 108.1742F;
            this.Column5.HeaderText = "Bài tập";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.FillWeight = 108.1742F;
            this.Column6.HeaderText = "Thực hành";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.FillWeight = 108.1742F;
            this.Column7.HeaderText = "Sửa";
            this.Column7.MinimumWidth = 6;
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column8
            // 
            this.Column8.FillWeight = 108.1742F;
            this.Column8.HeaderText = "Xóa";
            this.Column8.MinimumWidth = 6;
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            // 
            // LessonSubjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1675, 728);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LessonSubjectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Phân phối chương trình";
            this.Load += new System.EventHandler(this.LessonSubjectForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvSubjectsList)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvLessonList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dtgvSubjectsList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAddLessonSubject;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dtgvLessonList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
    }
}