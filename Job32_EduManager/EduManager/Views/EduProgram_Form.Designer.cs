namespace EduManager.Views
{
    partial class EduProgram_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EduProgram_Form));
            this.dtgvEduProgram = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddSubject = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvEduProgram)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgvEduProgram
            // 
            this.dtgvEduProgram.AllowUserToAddRows = false;
            this.dtgvEduProgram.AllowUserToDeleteRows = false;
            this.dtgvEduProgram.AllowUserToResizeRows = false;
            this.dtgvEduProgram.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgvEduProgram.BackgroundColor = System.Drawing.Color.White;
            this.dtgvEduProgram.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvEduProgram.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7});
            this.dtgvEduProgram.Location = new System.Drawing.Point(13, 89);
            this.dtgvEduProgram.Margin = new System.Windows.Forms.Padding(4);
            this.dtgvEduProgram.Name = "dtgvEduProgram";
            this.dtgvEduProgram.RowHeadersVisible = false;
            this.dtgvEduProgram.RowHeadersWidth = 51;
            this.dtgvEduProgram.RowTemplate.Height = 24;
            this.dtgvEduProgram.Size = new System.Drawing.Size(1474, 668);
            this.dtgvEduProgram.TabIndex = 0;
            this.dtgvEduProgram.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgvEduProgram_CellClick);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "ID";
            this.Column1.HeaderText = "ID";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Mã môn học";
            this.Column2.HeaderText = "Mã môn học";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "Tên môn học";
            this.Column3.HeaderText = "Tên môn học";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Hình thức";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "Số tiết học";
            this.Column5.HeaderText = "Số tiết";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Sửa";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Xóa";
            this.Column7.MinimumWidth = 6;
            this.Column7.Name = "Column7";
            // 
            // btnAddSubject
            // 
            this.btnAddSubject.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSubject.Location = new System.Drawing.Point(13, 13);
            this.btnAddSubject.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddSubject.Name = "btnAddSubject";
            this.btnAddSubject.Size = new System.Drawing.Size(130, 59);
            this.btnAddSubject.TabIndex = 19;
            this.btnAddSubject.Text = "+ Thêm môn học";
            this.btnAddSubject.UseVisualStyleBackColor = true;
            this.btnAddSubject.Click += new System.EventHandler(this.btnAddSubject_Click);
            // 
            // EduProgram_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1500, 770);
            this.Controls.Add(this.btnAddSubject);
            this.Controls.Add(this.dtgvEduProgram);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EduProgram_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quản lý đào tạo";
            this.Load += new System.EventHandler(this.EduProgram_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvEduProgram)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dtgvEduProgram;
        private System.Windows.Forms.Button btnAddSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
    }
}