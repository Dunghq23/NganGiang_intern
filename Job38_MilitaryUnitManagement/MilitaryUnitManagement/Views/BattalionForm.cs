using MilitaryUnitManagement.Models;
using MilitaryUnitManagement.Controllers;
using MilitaryUnitManagement.Services;
using System;
using System.Windows.Forms;

namespace MilitaryUnitManagement.Views
{
    public partial class BattalionForm : Form
    {
        private ConfigView configView = new ConfigView(); // Khai báo biến static

        public BattalionForm()
        {
            InitializeComponent();
            dtgv.RowTemplate.Height = 40;
        }

        // Load dữ liệu
        private void BattalionForm_Load(object sender, EventArgs e)
        {
            LoadData(); 
        }

        public void LoadData()
        {
            BattalionController.Instance().ShowData(dtgv);
            configView.ConfigureDataGridViewReadOnly(dtgv, "ID");
            configView.ConfigureColumnHeaders(dtgv);
            configView.ConfigureColumnAlignment(dtgv, new string[] { "ID", "Name" });
            configView.AddActionColumns(dtgv);
            configView.ConfigureSelectionMode(dtgv);
        }
        #region EVENT HANDLER

        private void btnAdd_Click(object sender, EventArgs e)
        {
            BattalionAddEditForm f = new BattalionAddEditForm();
            f.ShowDialog();
            LoadData();
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
                $"Bạn có chắc chắn muốn xóa \"{Name}\"? Thao tác này sẽ xóa toàn bộ các Đại đội của của tiểu đoàn này (nếu có).",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)
            {
                if (BattalionController.Instance().RemoveData(new Battalion(Id)))
                {
                    LoadData(); // Tải lại dữ liệu
                    MessageBox.Show("Xóa tiểu đoàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Xóa tiểu đoàn thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void HandleEdit(DataGridView dgv, int rowIndex)
        {
            var Id = int.Parse(dgv.Rows[rowIndex].Cells[0].Value.ToString());
            var Name = dgv.Rows[rowIndex].Cells[1].Value.ToString();
            var Description = dgv.Rows[rowIndex].Cells[2].Value.ToString();

            //if (SubjectController.Instance().checkDuplicate(subjectSymbol, subjectId, subjectName) > 0)
            //{
            //    MessageBox.Show("Ký hiệu hoặc tên môn học đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    dtgvEduProgram[columnIndex, rowIndex].Value = previousValue; // Khôi phục giá trị trước đó
            //    return;
            //}

            BattalionAddEditForm f = new BattalionAddEditForm(new Battalion(Id, Name, Description));
            f.ShowDialog();
            LoadData();

        }
        #endregion

    }
}
