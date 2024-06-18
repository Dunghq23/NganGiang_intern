using Management.Models.Military;
using MilitaryUnitManagement.Controllers.Military;
using System;
using System.Windows.Forms;

namespace Management.Views.MilitaryUnitManagement
{
    public partial class CompanyAddEditForm : Form
    {
        private bool isEdit = false;
        private Company companyEdit = new Company();

        public CompanyAddEditForm()
        {
            InitializeComponent();
            CompanyController.Instance().LoadComboBoxData(cbBattalion);
        }

        public CompanyAddEditForm(Company company)
        {
            InitializeComponent();
            CompanyController.Instance().LoadComboBoxData(cbBattalion);
            isEdit = true;

            companyEdit.ID = company.ID;
            txbNameBattalion.Text = company.Name;
            txbDescription.Text = company.Description;
            cbBattalion.SelectedValue = company.FK_BattalionID;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isEdit)
            {
                Company company = new Company();
                company.Name = txbNameBattalion.Text;
                company.Description = txbDescription.Text;
                company.FK_BattalionID = Convert.ToInt32(cbBattalion.SelectedValue);

                if (CompanyController.Instance().AddData(company))
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
                companyEdit.Name = txbNameBattalion.Text;
                companyEdit.Description = txbDescription.Text;
                companyEdit.FK_BattalionID = Convert.ToInt32(cbBattalion.SelectedValue);
                if (CompanyController.Instance().EditData(companyEdit))
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
