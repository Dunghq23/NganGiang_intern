using Management.Controllers.Education;
using Management.Models.Education;
using Management.Services;
using Management.Views.EducationManagement;
using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Management.Views.MilitaryUnitManagement
{
    public partial class EduProgramForm : Form
    {
        private ConfigView configView = new ConfigView(); // Khai báo biến static
        private object previousValue;
        private int rowIndex, columnIndex;
        public EduProgramForm()
        {
            InitializeComponent();
            dtgvEduProgram.RowTemplate.Height = 40;
        }

        private void EduProgramForm_Load(object sender, EventArgs e)
        {
            LoadData();
            // Chọn dòng toàn bộ khi click
            dtgvEduProgram.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvEduProgram.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#d8dfdc");
            dtgvEduProgram.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml("#000");

            //btnAddSubject.Image = Properties.Resources.ICON_ADD_24x24;
            btnAddSubject.TextImageRelation = TextImageRelation.ImageBeforeText;

        }
        #region LOAD DATA
        public void LoadData()
        {
            EduProgramController.Instance().ShowData(dtgvEduProgram);
            configView.ConfigureDataGridViewReadOnly(dtgvEduProgram, "Mã môn học");
            configView.ConfigureColumnHeaders(dtgvEduProgram);
            configView.ConfigureColumnAlignment(dtgvEduProgram, new string[] { "Mã môn học", "Ký hiệu", "Lý thuyết", "Bài tập", "Thực hành" });
            configView.AddActionColumns(dtgvEduProgram);
        }
        #endregion
        private void dtgvEduProgram_CellClick(object sender, DataGridViewCellEventArgs e)
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
            int subjectId = int.Parse(dgv.Rows[rowIndex].Cells[0].Value.ToString());
            var subjectName = dgv.Rows[rowIndex].Cells[1].Value.ToString();

            // Xác nhận trước khi xóa
            var confirmation = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa môn \"{subjectName}\"? Thao tác này sẽ xóa toàn bộ bài học của môn học (nếu có).",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question // Thay đổi biểu tượng để cho biết đây là một câu hỏi xác nhận
            );

            if (confirmation == DialogResult.Yes)
            {
                var isDeleted = EduProgramController.Instance().RemoveAllData(new EduProgram(subjectId)) &&
                                SubjectController.Instance().RemoveData(new Subject(subjectId));


                if (isDeleted)
                {
                    LoadData(); // Tải lại dữ liệu
                    MessageBox.Show("Xóa môn học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Xóa môn học thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void HandleEdit(DataGridView dgv, int rowIndex)
        {
            var subjectId = int.Parse(dgv.Rows[rowIndex].Cells[0].Value.ToString());
            var subjectName = dgv.Rows[rowIndex].Cells[1].Value.ToString();
            var subjectSymbol = dgv.Rows[rowIndex].Cells[2].Value.ToString();

            var lectureHours = Convert.ToInt32(dgv.Rows[rowIndex].Cells[3].Value);
            var exerciseHours = Convert.ToInt32(dgv.Rows[rowIndex].Cells[4].Value);
            var practiceHours = Convert.ToInt32(dgv.Rows[rowIndex].Cells[5].Value);

            if (SubjectController.Instance().checkDuplicate(subjectSymbol, subjectId, subjectName) > 0)
            {
                MessageBox.Show("Ký hiệu hoặc tên môn học đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtgvEduProgram[columnIndex, rowIndex].Value = previousValue; // Khôi phục giá trị trước đó
                return;
            }

            var subject = new Subject(subjectId, subjectName, subjectSymbol.ToUpper());
            var eduProgram1 = new EduProgram(subjectId, 1, lectureHours);
            var eduProgram2 = new EduProgram(subjectId, 2, exerciseHours);
            var eduProgram3 = new EduProgram(subjectId, 3, practiceHours);

            var isUpdated = SubjectController.Instance().editData(subject) &&
                            EduProgramController.Instance().EditData(eduProgram1) &&
                            EduProgramController.Instance().EditData(eduProgram2) &&
                            EduProgramController.Instance().EditData(eduProgram3);

            if (isUpdated)
            {
                LoadData();
                MessageBox.Show("Sửa môn học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Sửa môn học thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddSubject_Click(object sender, EventArgs e)
        {
            var subjectForm = new SubjectAddForm(this);
            subjectForm.ShowDialog();
        }

        #region VALIDATION

        private void dtgvEduProgram_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            previousValue = dtgvEduProgram[e.ColumnIndex, e.RowIndex].Value;
        }

        private void dtgvEduProgram_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var currentValue = dtgvEduProgram[e.ColumnIndex, e.RowIndex].Value;
            rowIndex = e.RowIndex;
            columnIndex = e.ColumnIndex;

            // Kiểm tra nếu giá trị rỗng hoặc chỉ có dấu cách
            if (currentValue == null || string.IsNullOrWhiteSpace(currentValue.ToString()))
            {
                dtgvEduProgram[e.ColumnIndex, e.RowIndex].Value = previousValue; // Khôi phục giá trị trước đó
            }
            else
            {
                // Nếu giá trị thay đổi so với trước đó
                if (!currentValue.Equals(previousValue))
                {
                    dtgvEduProgram[e.ColumnIndex, e.RowIndex].Style.ForeColor = System.Drawing.Color.Blue; // Đánh dấu màu xanh để cho biết ô đã được chỉnh sửa
                }

                // Kiểm tra tính hợp lệ với regex cho các cột khác không phải cột số
                var regex = new Regex(@"^[\p{L}\d\s\-,]+$");
                if (!regex.IsMatch(currentValue.ToString()))
                {
                    MessageBox.Show("Dữ liệu không hợp lệ. Vui lòng chỉ nhập chữ, số, dấu cách, gạch ngang, và phẩy.", "Thông báo");
                    dtgvEduProgram[e.ColumnIndex, e.RowIndex].Value = previousValue; // Khôi phục lại giá trị trước đó nếu không hợp lệ
                }
            }
        }

        // Chỉ bật phím Số cho các cột có chỉ số
        private void dtgvEduProgram_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var textBox = e.Control as TextBox;

            if (textBox != null)
            {
                textBox.KeyPress -= TextBox_KeyPress;

                int columnIndex = dtgvEduProgram.CurrentCell.ColumnIndex;
                int[] numericColumns = { 3, 4, 5 };

                if (numericColumns.Contains(columnIndex))
                {
                    textBox.KeyPress += TextBox_KeyPress;
                }
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                lbMessage.Text = "Vui lòng chỉ nhập số!";
                lbMessage.ForeColor = Color.Red;
            }
            else
            {
                lbMessage.Text = "";
            }
        }
        #endregion
    }
}
