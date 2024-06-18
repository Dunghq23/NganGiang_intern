using Management.Models.Military;
using Management.Services;
using MilitaryUnitManagement.Controllers.Military;
using System;
using System.Data;
using System.Windows.Forms;

namespace Management.Views.MilitaryUnitManagement
{
    public partial class PlatoonForm : Form
    {
        private ConfigView configView;
        private int selectedBattalionID;
        private int selectedCompanyID;

        public PlatoonForm()
        {
            InitializeComponent();
            configView = new ConfigView();
            dtgv.RowTemplate.Height = 40;
        }

        private void PlatoonForm_Load(object sender, EventArgs e)
        {
            PlatoonController.Instance().LoadComboBoxData_Battalion(cbBattalion);
        }

        private void loadData(int FK_BattalionID, int FK_CompanyID)
        {
            PlatoonController.Instance().ShowData(dtgv, FK_BattalionID, FK_CompanyID);
            configView.ConfigureDataGridViewReadOnly(dtgv, "ID");
            configView.ConfigureColumnHeaders(dtgv);
            configView.ConfigureColumnAlignment(dtgv, new string[] { "ID", "Name" });
            configView.AddActionColumns(dtgv);
            configView.ConfigureSelectionMode(dtgv);
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
                    loadData(selectedBattalionID, selectedCompanyID);
                }
            }
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem selectedItem có phải là DataRowView hay không
            DataRowView selectedItem = cbCompany.SelectedItem as DataRowView;

            if (selectedItem != null)
            {
                if (int.TryParse(selectedItem["ID"].ToString(), out selectedCompanyID))
                {
                    string selectedText = selectedItem["Name"].ToString();

                    // Tải dữ liệu với ID của cbCompany đang được chọn
                    loadData(selectedBattalionID, selectedCompanyID);
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn ID hợp lệ.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn ID hợp lệ.");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PlatoonAddEditForm f = new PlatoonAddEditForm();
            f.ShowDialog();
            loadData(selectedBattalionID, selectedCompanyID);
        }

        private void dtgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Bỏ qua khi click vào tiêu đề

            var dgv = sender as DataGridView;
            var headerText = dgv.Columns[e.ColumnIndex].HeaderText;

            if (headerText == "Xóa")
            {
                HandleDelete(dgv, e.RowIndex);
            }
            else if (headerText == "Sửa")
            {
                HandleEdit(dgv, e.RowIndex);
            }
        }

        private void HandleDelete(DataGridView dgv, int rowIndex)
        {
            int Id = int.Parse(dgv.Rows[rowIndex].Cells[0].Value.ToString());
            var Name = dgv.Rows[rowIndex].Cells[1].Value.ToString();

            // Xác nhận trước khi xóa
            var confirmation = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa \"{Name}\"?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)
            {
                if (PlatoonController.Instance().RemoveData(new Platoon(Id)))
                {
                    loadData(selectedBattalionID, selectedCompanyID);
                    MessageBox.Show("Xóa Đại đội thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Xóa Đại đội thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void HandleEdit(DataGridView dgv, int rowIndex)
        {
            var Id = int.Parse(dgv.Rows[rowIndex].Cells[0].Value.ToString());
            var Name = dgv.Rows[rowIndex].Cells[1].Value.ToString();
            var Description = dgv.Rows[rowIndex].Cells[2].Value.ToString();
            var FK_CompanyID = Convert.ToInt32(cbCompany.SelectedValue);

            //if (SubjectController.Instance().checkDuplicate(subjectSymbol, subjectId, subjectName) > 0)
            //{
            //    MessageBox.Show("Ký hiệu hoặc tên môn học đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    dtgvEduProgram[columnIndex, rowIndex].Value = previousValue; // Khôi phục giá trị trước đó
            //    return;
            //}

            PlatoonAddEditForm f = new PlatoonAddEditForm(new Platoon(Id, Name, Description, FK_CompanyID), selectedBattalionID);
            f.ShowDialog();
            loadData(selectedBattalionID, selectedCompanyID);
        }
    }
}
