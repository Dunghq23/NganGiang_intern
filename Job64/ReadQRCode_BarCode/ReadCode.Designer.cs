namespace ReadQRCode_BarCode
{
    partial class ReadCode
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
            btn_ChooseQRCode = new Button();
            pb_BarCode = new PictureBox();
            pb_QRCode = new PictureBox();
            txb_QRCode = new TextBox();
            label2 = new Label();
            txb_BarCode = new TextBox();
            label1 = new Label();
            btn_ChooseBarCode = new Button();
            btn_ReadCode = new Button();
            ((System.ComponentModel.ISupportInitialize)pb_BarCode).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_QRCode).BeginInit();
            SuspendLayout();
            // 
            // btn_ChooseQRCode
            // 
            btn_ChooseQRCode.Location = new Point(483, 273);
            btn_ChooseQRCode.Margin = new Padding(3, 4, 3, 4);
            btn_ChooseQRCode.Name = "btn_ChooseQRCode";
            btn_ChooseQRCode.Size = new Size(135, 32);
            btn_ChooseQRCode.TabIndex = 13;
            btn_ChooseQRCode.Text = "Chọn QR Code";
            btn_ChooseQRCode.UseVisualStyleBackColor = true;
            btn_ChooseQRCode.Click += btn_ChooseQRCode_Click;
            // 
            // pb_BarCode
            // 
            pb_BarCode.BackColor = SystemColors.ControlLight;
            pb_BarCode.Location = new Point(332, 13);
            pb_BarCode.Margin = new Padding(3, 4, 3, 4);
            pb_BarCode.Name = "pb_BarCode";
            pb_BarCode.Size = new Size(286, 107);
            pb_BarCode.TabIndex = 12;
            pb_BarCode.TabStop = false;
            // 
            // pb_QRCode
            // 
            pb_QRCode.BackColor = SystemColors.ControlLight;
            pb_QRCode.Location = new Point(12, 13);
            pb_QRCode.Margin = new Padding(3, 4, 3, 4);
            pb_QRCode.Name = "pb_QRCode";
            pb_QRCode.Size = new Size(314, 333);
            pb_QRCode.TabIndex = 11;
            pb_QRCode.TabStop = false;
            // 
            // txb_QRCode
            // 
            txb_QRCode.Location = new Point(332, 238);
            txb_QRCode.Margin = new Padding(3, 4, 3, 4);
            txb_QRCode.Name = "txb_QRCode";
            txb_QRCode.Size = new Size(285, 27);
            txb_QRCode.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(332, 214);
            label2.Name = "label2";
            label2.Size = new Size(120, 20);
            label2.TabIndex = 9;
            label2.Text = "Dữ liệu QR Code";
            // 
            // txb_BarCode
            // 
            txb_BarCode.Location = new Point(332, 164);
            txb_BarCode.Margin = new Padding(3, 4, 3, 4);
            txb_BarCode.Name = "txb_BarCode";
            txb_BarCode.Size = new Size(285, 27);
            txb_BarCode.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(332, 140);
            label1.Name = "label1";
            label1.Size = new Size(122, 20);
            label1.TabIndex = 7;
            label1.Text = "Dữ liệu Bar Code";
            // 
            // btn_ChooseBarCode
            // 
            btn_ChooseBarCode.Location = new Point(332, 274);
            btn_ChooseBarCode.Margin = new Padding(3, 4, 3, 4);
            btn_ChooseBarCode.Name = "btn_ChooseBarCode";
            btn_ChooseBarCode.Size = new Size(135, 32);
            btn_ChooseBarCode.TabIndex = 14;
            btn_ChooseBarCode.Text = "Chọn Bar Code";
            btn_ChooseBarCode.UseVisualStyleBackColor = true;
            btn_ChooseBarCode.Click += btn_ChooseBarCode_Click;
            // 
            // btn_ReadCode
            // 
            btn_ReadCode.Location = new Point(332, 314);
            btn_ReadCode.Margin = new Padding(3, 4, 3, 4);
            btn_ReadCode.Name = "btn_ReadCode";
            btn_ReadCode.Size = new Size(285, 32);
            btn_ReadCode.TabIndex = 15;
            btn_ReadCode.Text = "Đọc mã Code";
            btn_ReadCode.UseVisualStyleBackColor = true;
            btn_ReadCode.Click += btn_ReadCode_Click;
            // 
            // ReadCode
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(630, 363);
            Controls.Add(btn_ReadCode);
            Controls.Add(btn_ChooseBarCode);
            Controls.Add(btn_ChooseQRCode);
            Controls.Add(pb_BarCode);
            Controls.Add(pb_QRCode);
            Controls.Add(txb_QRCode);
            Controls.Add(label2);
            Controls.Add(txb_BarCode);
            Controls.Add(label1);
            Name = "ReadCode";
            Text = "ReadCode";
            ((System.ComponentModel.ISupportInitialize)pb_BarCode).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_QRCode).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_ChooseQRCode;
        private PictureBox pb_BarCode;
        private PictureBox pb_QRCode;
        private TextBox txb_QRCode;
        private Label label2;
        private TextBox txb_BarCode;
        private Label label1;
        private Button btn_ChooseBarCode;
        private Button btn_ReadCode;
    }
}