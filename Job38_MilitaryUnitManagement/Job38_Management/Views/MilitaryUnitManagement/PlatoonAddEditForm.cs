using Management.Models.Military;
using MilitaryUnitManagement.Controllers.Military;
using System;
using System.Data;
using System.Windows.Forms;

namespace Management.Views.MilitaryUnitManagement
{
    public partial class PlatoonAddEditForm : Form
    {
        private bool isEdit = false;
        private Platoon platoonEdit = new Platoon();
        private int selectedBattalionID;
        private int selectedCompanyID;

        public PlatoonAddEditForm()
        {
            InitializeComponent();
            CompanyController.Instance().LoadComboBoxData(cbBattalion);
            PlatoonController.Instance().LoadComboBoxData_Battalion(cbBattalion);
        }

        public PlatoonAddEditForm(Platoon platoon, int selectedBattalionID)
        {
            InitializeComponent();
            CompanyController.Instance().LoadComboBoxData(cbBattalion);
            PlatoonController.Instance().LoadComboBoxData_Battalion(cbBattalion);

            isEdit = true;
            platoonEdit.ID = platoon.ID;
            txbNamePlatoon.Text = platoon.Name;
            txbDescription.Text = platoon.Description;
            cbCompany.SelectedValue = platoon.FK_CompanyID;
            cbBattalion.SelectedValue = selectedBattalionID;
        }

        private void cbBattalion_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem selectedItem có phải là DataRowView hay không
            DataRowView selectedItem = cbBattalion.SelectedItem as DataRowView;

            if (selectedItem != null)
            {
                if (int.TryParse(selectedItem["ID"].ToString(), out selectedBattalionID))
                {
                    string selectedText = selectedItem["Name"].ToString();
                    PlatoonController.Instance().LoadComboBoxData_Company(cbCompany, selectedBattalionID);
                    selectedCompanyID = Convert.ToInt32(cbCompany.SelectedValue);
                }
            }
        }


        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView selectedItem = cbCompany.SelectedItem as DataRowView;

            if (selectedItem != null)
            {
                selectedCompanyID = Convert.ToInt32(selectedItem["ID"]);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isEdit)
            {
                Platoon platoon = new Platoon();
                platoon.Name = txbNamePlatoon.Text;
                platoon.Description = txbDescription.Text;
                platoon.FK_CompanyID = selectedCompanyID;

                if (cbCompany.SelectedItem == null)
                {
                    MessageBox.Show($"{cbBattalion.Text} chưa có Đại đội nào. Hãy thêm Đại đội rồi quay lại nhé!");
                    return;
                }

                if (PlatoonController.Instance().AddData(platoon))
                {
                    MessageBox.Show("Thêm Trung đội thành công!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm Trung đội thất bại!");
                }
            }
            else
            {
                platoonEdit.Name = txbNamePlatoon.Text;
                platoonEdit.Description = txbDescription.Text;
                platoonEdit.FK_CompanyID = selectedCompanyID;
                MessageBox.Show($"Mã Trung đội: {platoonEdit.ID}\nMã đại đội: {selectedCompanyID}");

                if (PlatoonController.Instance().EditData(platoonEdit))
                {
                    MessageBox.Show("Sửa Trung đội thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sửa Trung đội thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
