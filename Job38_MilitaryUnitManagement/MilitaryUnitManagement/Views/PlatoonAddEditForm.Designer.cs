namespace MilitaryUnitManagement.Views
{
    partial class PlatoonAddEditForm
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
            this.btnSave = new System.Windows.Forms.Button();
            this.txbDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txbNamePlatoon = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbBattalion = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbCompany = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(351, 395);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(171, 47);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Lưu lại";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txbDescription
            // 
            this.txbDescription.Location = new System.Drawing.Point(29, 190);
            this.txbDescription.Margin = new System.Windows.Forms.Padding(4);
            this.txbDescription.Multiline = true;
            this.txbDescription.Name = "txbDescription";
            this.txbDescription.Size = new System.Drawing.Size(491, 184);
            this.txbDescription.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 166);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Mô tả";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 90);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Tên trung đội";
            // 
            // txbNamePlatoon
            // 
            this.txbNamePlatoon.Location = new System.Drawing.Point(29, 113);
            this.txbNamePlatoon.Margin = new System.Windows.Forms.Padding(4);
            this.txbNamePlatoon.Name = "txbNamePlatoon";
            this.txbNamePlatoon.Size = new System.Drawing.Size(491, 22);
            this.txbNamePlatoon.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "Tiểu đoàn";
            // 
            // cbBattalion
            // 
            this.cbBattalion.FormattingEnabled = true;
            this.cbBattalion.Location = new System.Drawing.Point(29, 44);
            this.cbBattalion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbBattalion.Name = "cbBattalion";
            this.cbBattalion.Size = new System.Drawing.Size(220, 24);
            this.cbBattalion.TabIndex = 12;
            this.cbBattalion.SelectedIndexChanged += new System.EventHandler(this.cbBattalion_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(297, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 16);
            this.label4.TabIndex = 15;
            this.label4.Text = "Đại đội";
            // 
            // cbCompany
            // 
            this.cbCompany.FormattingEnabled = true;
            this.cbCompany.Location = new System.Drawing.Point(300, 44);
            this.cbCompany.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbCompany.Name = "cbCompany";
            this.cbCompany.Size = new System.Drawing.Size(220, 24);
            this.cbCompany.TabIndex = 14;
            this.cbCompany.SelectedIndexChanged += new System.EventHandler(this.cbCompany_SelectedIndexChanged);
            // 
            // PlatoonAddEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 470);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbCompany);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbBattalion);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txbDescription);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbNamePlatoon);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PlatoonAddEditForm";
            this.Text = "PlatoonAddEditForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txbDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbNamePlatoon;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbBattalion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbCompany;
    }
}