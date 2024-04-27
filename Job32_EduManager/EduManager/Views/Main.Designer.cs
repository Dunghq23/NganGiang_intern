namespace EduManager
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btnEduManage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEduManage
            // 
            this.btnEduManage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEduManage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEduManage.Location = new System.Drawing.Point(50, 71);
            this.btnEduManage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEduManage.Name = "btnEduManage";
            this.btnEduManage.Size = new System.Drawing.Size(110, 102);
            this.btnEduManage.TabIndex = 2;
            this.btnEduManage.Text = "Quản lý đào tạo";
            this.btnEduManage.UseVisualStyleBackColor = true;
            this.btnEduManage.Click += new System.EventHandler(this.btnEduManage_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(699, 367);
            this.Controls.Add(this.btnEduManage);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trường Đại học Thủy lợi";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnEduManage;
    }
}

