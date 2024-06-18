using Management.Models.Military;
using Management.Services;
using MilitaryUnitManagement.Controllers.Military;
using System;
using System.Data;
using System.Windows.Forms;

namespace Management.Views.MilitaryUnitManagement
{
    public partial class CompanyForm : Form
    {

        private ConfigView configView;
        private int selectedValue;
        private string selectedText;

        public CompanyForm()
        {
            InitializeComponent();
            configView = new ConfigView(); // Initialize ConfigView instance
            dtgv.RowTemplate.Height = 40;
        }

        private void CompanyForm_Load(object sender, EventArgs e)
        {
            CompanyController.Instance().LoadComboBoxData(cb);
        }

        private void loadData(int ID)
        {
            CompanyController.Instance().ShowData(dtgv, ID);
            // Use the ConfigView instance to configure the DataGridView
            configView.ConfigureDataGridViewReadOnly(dtgv, "ID");
            configView.ConfigureColumnHeaders(dtgv);
            configView.ConfigureColumnAlignment(dtgv, new string[] { "ID", "Name" });
            configView.AddActionColumns(dtgv);
            configView.ConfigureSelectionMode(dtgv);
        }

        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem selectedItem có phải là DataRowView hay không
            DataRowView selectedItem = cb.SelectedItem as DataRowView;

            if (selectedItem != null)
            {
                // Trích xuất giá trị ID từ DataRowView
                if (int.TryParse(selectedItem["ID"].ToString(), out selectedValue))
                {
                    // Lấy tên từ DataRowView
                    selectedText = selectedItem["Name"].ToString();

                    // Load dữ liệu dựa trên selectedValue
                    loadData(selectedValue);
                }
                else
                {
                    // Xử lý trường hợp không thể chuyển đổi giá trị ID thành số nguyên
                    MessageBox.Show("Vui lòng chọn ID hợp lệ.");
                }
            }
            else
            {
                // Xử lý khi không có item được chọn hoặc item không phải là DataRowView
                MessageBox.Show("Vui lòng chọn ID hợp lệ.");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CompanyAddEditForm f = new CompanyAddEditForm();
            f.ShowDialog();
            loadData(selectedValue);
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
                $"Bạn có chắc chắn muốn xóa \"{Name}\"? Thao tác này sẽ xóa toàn bộ các Trung đội của của Đại đội này (nếu có).",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)
            {
                if (CompanyController.Instance().RemoveData(new Company(Id)))
                {
                    loadData(selectedValue); // Tải lại dữ liệu
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
            var FK_BattalionID = Convert.ToInt32(cb.SelectedValue);

            //if (SubjectController.Instance().checkDuplicate(subjectSymbol, subjectId, subjectName) > 0)
            //{
            //    MessageBox.Show("Ký hiệu hoặc tên môn học đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    dtgvEduProgram[columnIndex, rowIndex].Value = previousValue; // Khôi phục giá trị trước đó
            //    return;
            //}

            CompanyAddEditForm f = new CompanyAddEditForm(new Company(Id, Name, Description, FK_BattalionID));
            f.ShowDialog();
            loadData(selectedValue);

        }

    }
}
