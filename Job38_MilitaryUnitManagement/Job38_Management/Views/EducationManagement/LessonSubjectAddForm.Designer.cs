namespace Management.Views.EducationManagement
{
    partial class LessonSubjectAddForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LessonSubjectAddForm));
            this.txbLesson = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txbSym_Sub = new System.Windows.Forms.TextBox();
            this.nmBT = new System.Windows.Forms.NumericUpDown();
            this.nmTH = new System.Windows.Forms.NumericUpDown();
            this.nmLT = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txbLessonName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nmBT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmTH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmLT)).BeginInit();
            this.SuspendLayout();
            // 
            // txbLesson
            // 
            this.txbLesson.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbLesson.Location = new System.Drawing.Point(539, 40);
            this.txbLesson.Margin = new System.Windows.Forms.Padding(4);
            this.txbLesson.Name = "txbLesson";
            this.txbLesson.Size = new System.Drawing.Size(228, 27);
            this.txbLesson.TabIndex = 57;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(569, 144);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 20);
            this.label8.TabIndex = 56;
            this.label8.Text = "Thực hành";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(310, 144);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 20);
            this.label7.TabIndex = 55;
            this.label7.Text = "Bài tập";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(30, 142);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 20);
            this.label6.TabIndex = 54;
            this.label6.Text = "Lý thuyết";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(557, 213);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(211, 49);
            this.btnSave.TabIndex = 53;
            this.btnSave.Text = "Lưu lại";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(30, 40);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 20);
            this.label3.TabIndex = 52;
            this.label3.Text = "Ký hiệu môn học";
            // 
            // txbSym_Sub
            // 
            this.txbSym_Sub.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbSym_Sub.Location = new System.Drawing.Point(220, 37);
            this.txbSym_Sub.Margin = new System.Windows.Forms.Padding(4);
            this.txbSym_Sub.Name = "txbSym_Sub";
            this.txbSym_Sub.ReadOnly = true;
            this.txbSym_Sub.Size = new System.Drawing.Size(152, 27);
            this.txbSym_Sub.TabIndex = 51;
            // 
            // nmBT
            // 
            this.nmBT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmBT.Location = new System.Drawing.Point(412, 139);
            this.nmBT.Margin = new System.Windows.Forms.Padding(4);
            this.nmBT.Name = "nmBT";
            this.nmBT.Size = new System.Drawing.Size(82, 27);
            this.nmBT.TabIndex = 50;
            // 
            // nmTH
            // 
            this.nmTH.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmTH.Location = new System.Drawing.Point(686, 139);
            this.nmTH.Margin = new System.Windows.Forms.Padding(4);
            this.nmTH.Name = "nmTH";
            this.nmTH.Size = new System.Drawing.Size(82, 27);
            this.nmTH.TabIndex = 49;
            // 
            // nmLT
            // 
            this.nmLT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmLT.Location = new System.Drawing.Point(172, 139);
            this.nmLT.Margin = new System.Windows.Forms.Padding(4);
            this.nmLT.Name = "nmLT";
            this.nmLT.Size = new System.Drawing.Size(82, 27);
            this.nmLT.TabIndex = 48;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(30, 86);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 20);
            this.label2.TabIndex = 47;
            this.label2.Text = "Tiêu đề bài học";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(444, 44);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 20);
            this.label1.TabIndex = 46;
            this.label1.Text = "Bài học";
            // 
            // txbLessonName
            // 
            this.txbLessonName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbLessonName.Location = new System.Drawing.Point(220, 86);
            this.txbLessonName.Margin = new System.Windows.Forms.Padding(4);
            this.txbLessonName.Name = "txbLessonName";
            this.txbLessonName.Size = new System.Drawing.Size(546, 27);
            this.txbLessonName.TabIndex = 45;
            // 
            // LessonSubjectAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 279);
            this.Controls.Add(this.txbLesson);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txbSym_Sub);
            this.Controls.Add(this.nmBT);
            this.Controls.Add(this.nmTH);
            this.Controls.Add(this.nmLT);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbLessonName);
            this.Name = "LessonSubjectAddForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thêm môn học";
            this.Load += new System.EventHandler(this.LessonSubjectAddForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nmBT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmTH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmLT)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbLesson;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbSym_Sub;
        private System.Windows.Forms.NumericUpDown nmBT;
        private System.Windows.Forms.NumericUpDown nmTH;
        private System.Windows.Forms.NumericUpDown nmLT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbLessonName;
    }
}