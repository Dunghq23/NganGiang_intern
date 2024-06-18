namespace Job38_Management
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.quảnLýGiáoDụcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quảnLýĐàoTạoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quảnLýPhânPhốiChươngTrìnhToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quảnLýĐơnVịToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tiểuĐoànToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.đạiĐộiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trungĐộiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quảnLýGiáoDụcToolStripMenuItem,
            this.quảnLýĐơnVịToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(819, 28);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // quảnLýGiáoDụcToolStripMenuItem
            // 
            this.quảnLýGiáoDụcToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quảnLýĐàoTạoToolStripMenuItem,
            this.quảnLýPhânPhốiChươngTrìnhToolStripMenuItem});
            this.quảnLýGiáoDụcToolStripMenuItem.Name = "quảnLýGiáoDụcToolStripMenuItem";
            this.quảnLýGiáoDụcToolStripMenuItem.Size = new System.Drawing.Size(135, 24);
            this.quảnLýGiáoDụcToolStripMenuItem.Text = "Quản lý giáo dục";
            // 
            // quảnLýĐàoTạoToolStripMenuItem
            // 
            this.quảnLýĐàoTạoToolStripMenuItem.Name = "quảnLýĐàoTạoToolStripMenuItem";
            this.quảnLýĐàoTạoToolStripMenuItem.Size = new System.Drawing.Size(301, 26);
            this.quảnLýĐàoTạoToolStripMenuItem.Text = "Quản lý đào tạo";
            this.quảnLýĐàoTạoToolStripMenuItem.Click += new System.EventHandler(this.quảnLýĐàoTạoToolStripMenuItem_Click);
            // 
            // quảnLýPhânPhốiChươngTrìnhToolStripMenuItem
            // 
            this.quảnLýPhânPhốiChươngTrìnhToolStripMenuItem.Name = "quảnLýPhânPhốiChươngTrìnhToolStripMenuItem";
            this.quảnLýPhânPhốiChươngTrìnhToolStripMenuItem.Size = new System.Drawing.Size(301, 26);
            this.quảnLýPhânPhốiChươngTrìnhToolStripMenuItem.Text = "Quản lý phân phối chương trình";
            this.quảnLýPhânPhốiChươngTrìnhToolStripMenuItem.Click += new System.EventHandler(this.quảnLýPhânPhốiChươngTrìnhToolStripMenuItem_Click);
            // 
            // quảnLýĐơnVịToolStripMenuItem
            // 
            this.quảnLýĐơnVịToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tiểuĐoànToolStripMenuItem,
            this.đạiĐộiToolStripMenuItem,
            this.trungĐộiToolStripMenuItem});
            this.quảnLýĐơnVịToolStripMenuItem.Name = "quảnLýĐơnVịToolStripMenuItem";
            this.quảnLýĐơnVịToolStripMenuItem.Size = new System.Drawing.Size(118, 24);
            this.quảnLýĐơnVịToolStripMenuItem.Text = "Quản lý đơn vị";
            // 
            // tiểuĐoànToolStripMenuItem
            // 
            this.tiểuĐoànToolStripMenuItem.Name = "tiểuĐoànToolStripMenuItem";
            this.tiểuĐoànToolStripMenuItem.Size = new System.Drawing.Size(158, 26);
            this.tiểuĐoànToolStripMenuItem.Text = "Tiểu đoàn";
            this.tiểuĐoànToolStripMenuItem.Click += new System.EventHandler(this.tiểuĐoànToolStripMenuItem_Click);
            // 
            // đạiĐộiToolStripMenuItem
            // 
            this.đạiĐộiToolStripMenuItem.Name = "đạiĐộiToolStripMenuItem";
            this.đạiĐộiToolStripMenuItem.Size = new System.Drawing.Size(158, 26);
            this.đạiĐộiToolStripMenuItem.Text = "Đại đội";
            this.đạiĐộiToolStripMenuItem.Click += new System.EventHandler(this.đạiĐộiToolStripMenuItem_Click);
            // 
            // trungĐộiToolStripMenuItem
            // 
            this.trungĐộiToolStripMenuItem.Name = "trungĐộiToolStripMenuItem";
            this.trungĐộiToolStripMenuItem.Size = new System.Drawing.Size(158, 26);
            this.trungĐộiToolStripMenuItem.Text = "Trung đội";
            this.trungĐộiToolStripMenuItem.Click += new System.EventHandler(this.trungĐộiToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 472);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem quảnLýGiáoDụcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quảnLýĐàoTạoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quảnLýPhânPhốiChươngTrìnhToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quảnLýĐơnVịToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tiểuĐoànToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem đạiĐộiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trungĐộiToolStripMenuItem;
    }
}

