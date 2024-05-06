namespace EduManager.Views
{
    partial class Subjects
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Subjects));
            this.btnAddSubject = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.nmBT = new System.Windows.Forms.NumericUpDown();
            this.nmTH = new System.Windows.Forms.NumericUpDown();
            this.nmLT = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.chbBT = new System.Windows.Forms.CheckBox();
            this.chbTH = new System.Windows.Forms.CheckBox();
            this.chbLT = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txbName_Sub = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txbSym_Sub = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmBT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmTH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmLT)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddSubject
            // 
            this.btnAddSubject.Location = new System.Drawing.Point(221, 285);
            this.btnAddSubject.Name = "btnAddSubject";
            this.btnAddSubject.Size = new System.Drawing.Size(75, 43);
            this.btnAddSubject.TabIndex = 0;
            this.btnAddSubject.Text = "Thêm môn học";
            this.btnAddSubject.UseVisualStyleBackColor = true;
            this.btnAddSubject.Click += new System.EventHandler(this.btnAddSubject_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.nmBT);
            this.panel2.Controls.Add(this.nmTH);
            this.panel2.Controls.Add(this.nmLT);
            this.panel2.Controls.Add(this.btnAddSubject);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.chbBT);
            this.panel2.Controls.Add(this.chbTH);
            this.panel2.Controls.Add(this.chbLT);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txbName_Sub);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txbSym_Sub);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(319, 341);
            this.panel2.TabIndex = 1;
            // 
            // nmBT
            // 
            this.nmBT.Location = new System.Drawing.Point(221, 177);
            this.nmBT.Name = "nmBT";
            this.nmBT.Size = new System.Drawing.Size(66, 22);
            this.nmBT.TabIndex = 14;
            this.nmBT.Visible = false;
            // 
            // nmTH
            // 
            this.nmTH.Location = new System.Drawing.Point(221, 205);
            this.nmTH.Name = "nmTH";
            this.nmTH.Size = new System.Drawing.Size(66, 22);
            this.nmTH.TabIndex = 13;
            this.nmTH.Visible = false;
            // 
            // nmLT
            // 
            this.nmLT.Location = new System.Drawing.Point(221, 149);
            this.nmLT.Name = "nmLT";
            this.nmLT.Size = new System.Drawing.Size(66, 22);
            this.nmLT.TabIndex = 12;
            this.nmLT.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(218, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Số tiết học";
            // 
            // chbBT
            // 
            this.chbBT.AutoSize = true;
            this.chbBT.Location = new System.Drawing.Point(46, 179);
            this.chbBT.Name = "chbBT";
            this.chbBT.Size = new System.Drawing.Size(141, 20);
            this.chbBT.TabIndex = 7;
            this.chbBT.Text = "Bài tập / Thảo luận";
            this.chbBT.UseVisualStyleBackColor = true;
            this.chbBT.CheckedChanged += new System.EventHandler(this.chbBT_CheckedChanged);
            // 
            // chbTH
            // 
            this.chbTH.AutoSize = true;
            this.chbTH.Location = new System.Drawing.Point(46, 207);
            this.chbTH.Name = "chbTH";
            this.chbTH.Size = new System.Drawing.Size(91, 20);
            this.chbTH.TabIndex = 6;
            this.chbTH.Text = "Thực hành";
            this.chbTH.UseVisualStyleBackColor = true;
            this.chbTH.CheckedChanged += new System.EventHandler(this.chbTH_CheckedChanged);
            // 
            // chbLT
            // 
            this.chbLT.AutoSize = true;
            this.chbLT.Location = new System.Drawing.Point(46, 149);
            this.chbLT.Name = "chbLT";
            this.chbLT.Size = new System.Drawing.Size(133, 20);
            this.chbLT.TabIndex = 5;
            this.chbLT.Text = "Lý thuyết / Lý luận";
            this.chbLT.UseVisualStyleBackColor = true;
            this.chbLT.CheckedChanged += new System.EventHandler(this.chbLT_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Hình thức học";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tên môn học";
            // 
            // txbName_Sub
            // 
            this.txbName_Sub.Location = new System.Drawing.Point(25, 87);
            this.txbName_Sub.Name = "txbName_Sub";
            this.txbName_Sub.Size = new System.Drawing.Size(262, 22);
            this.txbName_Sub.TabIndex = 2;
            this.txbName_Sub.TextChanged += new System.EventHandler(this.txbName_Sub_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ký hiệu môn học";
            // 
            // txbSym_Sub
            // 
            this.txbSym_Sub.Location = new System.Drawing.Point(25, 32);
            this.txbSym_Sub.Name = "txbSym_Sub";
            this.txbSym_Sub.Size = new System.Drawing.Size(262, 22);
            this.txbSym_Sub.TabIndex = 0;
            this.txbSym_Sub.TextChanged += new System.EventHandler(this.txbId_Sub_TextChanged);
            // 
            // Subjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 365);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Subjects";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Subjects";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmBT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmTH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmLT)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAddSubject;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbName_Sub;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbSym_Sub;
        private System.Windows.Forms.CheckBox chbLT;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chbBT;
        private System.Windows.Forms.CheckBox chbTH;
        private System.Windows.Forms.NumericUpDown nmBT;
        private System.Windows.Forms.NumericUpDown nmTH;
        private System.Windows.Forms.NumericUpDown nmLT;
    }
}