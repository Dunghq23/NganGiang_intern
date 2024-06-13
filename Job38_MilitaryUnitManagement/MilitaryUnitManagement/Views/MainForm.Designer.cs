namespace MilitaryUnitManagement
{
    partial class MainForm
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
            this.btnBattalion = new System.Windows.Forms.Button();
            this.btnCompany = new System.Windows.Forms.Button();
            this.btnPlatoon = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBattalion
            // 
            this.btnBattalion.Location = new System.Drawing.Point(16, 15);
            this.btnBattalion.Margin = new System.Windows.Forms.Padding(4);
            this.btnBattalion.Name = "btnBattalion";
            this.btnBattalion.Size = new System.Drawing.Size(100, 69);
            this.btnBattalion.TabIndex = 0;
            this.btnBattalion.Text = "Tiểu đoàn";
            this.btnBattalion.UseVisualStyleBackColor = true;
            this.btnBattalion.Click += new System.EventHandler(this.btnBattalion_Click);
            // 
            // btnCompany
            // 
            this.btnCompany.Location = new System.Drawing.Point(124, 15);
            this.btnCompany.Margin = new System.Windows.Forms.Padding(4);
            this.btnCompany.Name = "btnCompany";
            this.btnCompany.Size = new System.Drawing.Size(100, 69);
            this.btnCompany.TabIndex = 1;
            this.btnCompany.Text = "Đại đội";
            this.btnCompany.UseVisualStyleBackColor = true;
            // 
            // btnPlatoon
            // 
            this.btnPlatoon.Location = new System.Drawing.Point(232, 15);
            this.btnPlatoon.Margin = new System.Windows.Forms.Padding(4);
            this.btnPlatoon.Name = "btnPlatoon";
            this.btnPlatoon.Size = new System.Drawing.Size(100, 69);
            this.btnPlatoon.TabIndex = 2;
            this.btnPlatoon.Text = "Trung đội";
            this.btnPlatoon.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 391);
            this.Controls.Add(this.btnPlatoon);
            this.Controls.Add(this.btnCompany);
            this.Controls.Add(this.btnBattalion);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Military Unit Management";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBattalion;
        private System.Windows.Forms.Button btnCompany;
        private System.Windows.Forms.Button btnPlatoon;
    }
}

