namespace View_Map_on_Winform
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.latitude = new System.Windows.Forms.TextBox();
            this.longitude = new System.Windows.Forms.TextBox();
            this.goCordinates = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.zoomLevel = new System.Windows.Forms.NumericUpDown();
            this.btnReturnCordinate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.zoomLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // latitude
            // 
            this.latitude.Location = new System.Drawing.Point(88, 16);
            this.latitude.Name = "latitude";
            this.latitude.Size = new System.Drawing.Size(162, 22);
            this.latitude.TabIndex = 0;
            // 
            // longitude
            // 
            this.longitude.Location = new System.Drawing.Point(88, 44);
            this.longitude.Name = "longitude";
            this.longitude.Size = new System.Drawing.Size(162, 22);
            this.longitude.TabIndex = 1;
            // 
            // goCordinates
            // 
            this.goCordinates.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.goCordinates.Location = new System.Drawing.Point(12, 72);
            this.goCordinates.Name = "goCordinates";
            this.goCordinates.Size = new System.Drawing.Size(238, 49);
            this.goCordinates.TabIndex = 0;
            this.goCordinates.Text = "Go to Cordinates";
            this.goCordinates.UseVisualStyleBackColor = true;
            this.goCordinates.Click += new System.EventHandler(this.goCordinates_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(267, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(521, 419);
            this.panel1.TabIndex = 5;
            // 
            // zoomLevel
            // 
            this.zoomLevel.Location = new System.Drawing.Point(125, 144);
            this.zoomLevel.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.zoomLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.zoomLevel.Name = "zoomLevel";
            this.zoomLevel.Size = new System.Drawing.Size(125, 22);
            this.zoomLevel.TabIndex = 6;
            this.zoomLevel.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.zoomLevel.ValueChanged += new System.EventHandler(this.zoomLevel_ValueChanged);
            // 
            // btnReturnCordinate
            // 
            this.btnReturnCordinate.Location = new System.Drawing.Point(12, 188);
            this.btnReturnCordinate.Name = "btnReturnCordinate";
            this.btnReturnCordinate.Size = new System.Drawing.Size(238, 49);
            this.btnReturnCordinate.TabIndex = 7;
            this.btnReturnCordinate.Text = "Active Cordinates Click";
            this.btnReturnCordinate.UseVisualStyleBackColor = true;
            this.btnReturnCordinate.Click += new System.EventHandler(this.btnReturnCordinate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Change zoom";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Latitude";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Longitude";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnReturnCordinate);
            this.Controls.Add(this.zoomLevel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.goCordinates);
            this.Controls.Add(this.longitude);
            this.Controls.Add(this.latitude);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.zoomLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox latitude;
        private System.Windows.Forms.TextBox longitude;
        private System.Windows.Forms.Button goCordinates;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown zoomLevel;
        private System.Windows.Forms.Button btnReturnCordinate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

