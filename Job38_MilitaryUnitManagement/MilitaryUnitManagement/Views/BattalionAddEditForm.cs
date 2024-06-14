using MilitaryUnitManagement.Controllers;
using MilitaryUnitManagement.Models;
using System;
using System.Windows.Forms;

namespace MilitaryUnitManagement.Views
{
    public partial class BattalionAddEditForm : Form
    {
        private bool isEdit = false;
        private Battalion battalionEdit = new Battalion();

        public BattalionAddEditForm()
        {
            InitializeComponent();
        }

        public BattalionAddEditForm(Battalion battalion)
        {
            InitializeComponent();
            isEdit = true;

            battalionEdit.ID = battalion.ID;
            txbNameBattalion.Text = battalion.Name;
            txbDescription.Text = battalion.Description;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isEdit)
            {
                Battalion battalion = new Battalion();
                battalion.Name = txbNameBattalion.Text;
                battalion.Description = txbDescription.Text;


                if (BattalionController.Instance().AddData(battalion))
                {
                    MessageBox.Show("Thêm tiểu đoàn thành công!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm tiểu đoàn thất bại!");
                }
            }
            else
            {
                battalionEdit.Name = txbNameBattalion.Text;
                battalionEdit.Description = txbDescription.Text;

                if (BattalionController.Instance().EditData(battalionEdit))
                {
                    MessageBox.Show("Sửa tiểu đoàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sửa tiểu đoàn thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
