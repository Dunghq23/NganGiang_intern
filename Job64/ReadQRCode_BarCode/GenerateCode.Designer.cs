namespace ReadQRCode_BarCode
{
    partial class GenerateCode
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txb_BarCode = new TextBox();
            txb_QRCode = new TextBox();
            label2 = new Label();
            pb_QRCode = new PictureBox();
            pb_BarCode = new PictureBox();
            btn_Show = new Button();
            ((System.ComponentModel.ISupportInitialize)pb_QRCode).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_BarCode).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(334, 143);
            label1.Name = "label1";
            label1.Size = new Size(122, 20);
            label1.TabIndex = 0;
            label1.Text = "Dữ liệu Bar Code";
            // 
            // txb_BarCode
            // 
            txb_BarCode.Location = new Point(334, 167);
            txb_BarCode.Margin = new Padding(3, 4, 3, 4);
            txb_BarCode.Name = "txb_BarCode";
            txb_BarCode.Size = new Size(285, 27);
            txb_BarCode.TabIndex = 1;
            // 
            // txb_QRCode
            // 
            txb_QRCode.Location = new Point(334, 241);
            txb_QRCode.Margin = new Padding(3, 4, 3, 4);
            txb_QRCode.Name = "txb_QRCode";
            txb_QRCode.Size = new Size(285, 27);
            txb_QRCode.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(334, 217);
            label2.Name = "label2";
            label2.Size = new Size(120, 20);
            label2.TabIndex = 2;
            label2.Text = "Dữ liệu QR Code";
            // 
            // pb_QRCode
            // 
            pb_QRCode.Location = new Point(14, 16);
            pb_QRCode.Margin = new Padding(3, 4, 3, 4);
            pb_QRCode.Name = "pb_QRCode";
            pb_QRCode.Size = new Size(314, 333);
            pb_QRCode.TabIndex = 4;
            pb_QRCode.TabStop = false;
            // 
            // pb_BarCode
            // 
            pb_BarCode.Location = new Point(334, 16);
            pb_BarCode.Margin = new Padding(3, 4, 3, 4);
            pb_BarCode.Name = "pb_BarCode";
            pb_BarCode.Size = new Size(286, 107);
            pb_BarCode.TabIndex = 5;
            pb_BarCode.TabStop = false;
            // 
            // btn_Show
            // 
            btn_Show.Location = new Point(334, 292);
            btn_Show.Margin = new Padding(3, 4, 3, 4);
            btn_Show.Name = "btn_Show";
            btn_Show.Size = new Size(286, 57);
            btn_Show.TabIndex = 6;
            btn_Show.Text = "Hiển thị";
            btn_Show.UseVisualStyleBackColor = true;
            btn_Show.Click += btn_Show_Click;
            // 
            // GenerateCode
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(638, 367);
            Controls.Add(btn_Show);
            Controls.Add(pb_BarCode);
            Controls.Add(pb_QRCode);
            Controls.Add(txb_QRCode);
            Controls.Add(label2);
            Controls.Add(txb_BarCode);
            Controls.Add(label1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "GenerateCode";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pb_QRCode).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_BarCode).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txb_BarCode;
        private TextBox txb_QRCode;
        private Label label2;
        private PictureBox pb_QRCode;
        private PictureBox pb_BarCode;
        private Button btn_Show;
    }
}
