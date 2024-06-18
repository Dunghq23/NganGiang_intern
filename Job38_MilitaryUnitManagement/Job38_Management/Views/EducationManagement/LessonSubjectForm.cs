using Management.Controllers.Education;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Management.Views.EducationManagement
{
    public partial class LessonSubjectForm : Form
    {
        // Khai báo dữ liệu
        private string selectedSubjectSymbol = string.Empty;

        // Khởi tạo form
        public LessonSubjectForm()
        {
            InitializeComponent();
            SetupDataGridViewDefaults();
        }

        private void LessonSubjectForm_Load(object sender, EventArgs e)
        {
            LoadData();
            ConfigureDataGridViewStyles();
        }

        // Cài đặt mặc định cho DataGridView
        private void SetupDataGridViewDefaults()
        {
            dtgvLessonList.RowTemplate.Height = 40;
            dtgvSubjectsList.RowTemplate.Height = 40;

            dtgvLessonList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvSubjectsList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        // Nhóm hàm thiết lập cấu hình cho DataGridView
        private void ConfigureDataGridViewStyles()
        {
            ConfigureDataGridView(dtgvLessonList, "#d8dfdc", "#000");
            ConfigureDataGridView(dtgvSubjectsList, "#d8dfdc", "#000");

            ConfigureColumnWeights();
            ConfigureColumnHeaders();
        }

        private void ConfigureDataGridView(DataGridView dataGridView, string selectionBackColor, string selectionForeColor)
        {
            dataGridView.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml(selectionBackColor);
            dataGridView.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml(selectionForeColor);
        }

        private void ConfigureColumnWeights()
        {
            dtgvSubjectsList.Columns["Ký hiệu"].FillWeight = 20;
            dtgvSubjectsList.Columns["Tên môn học"].FillWeight = 80;

            dtgvLessonList.Columns["Ký hiệu môn học"].FillWeight = 10;
            dtgvLessonList.Columns["Bài học"].FillWeight = 10;
            dtgvLessonList.Columns["Tên bài học"].FillWeight = 50;
            dtgvLessonList.Columns["Lý thuyết"].FillWeight = 10;
            dtgvLessonList.Columns["Bài tập"].FillWeight = 10;
            dtgvLessonList.Columns["Thực hành"].FillWeight = 10;

            dtgvLessonList.Sort(dtgvLessonList.Columns["Bài học"], ListSortDirection.Ascending);
        }

        private void ConfigureColumnHeaders()
        {
            var headerStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = SystemColors.GrayText,
                ForeColor = SystemColors.WindowText,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            dtgvSubjectsList.ColumnHeadersDefaultCellStyle = headerStyle;
            dtgvLessonList.ColumnHeadersDefaultCellStyle = headerStyle;
        }

        // Quản lý dữ liệu
        public void LoadData()
        {
            var controller = LessonSubjectController.Instance();
            controller.ShowSubjectsList(dtgvSubjectsList);
            controller.ShowLessonList(dtgvLessonList, selectedSubjectSymbol);
        }

        private void dtgvSubjectsList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedSubjectSymbol = dtgvSubjectsList.Rows[e.RowIndex].Cells[0].Value?.ToString();
                LessonSubjectController.Instance().ShowLessonList(dtgvLessonList, selectedSubjectSymbol);
            }
        }

        private void btnAddLessonSubject_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectedSubjectSymbol))
            {
                MessageBox.Show("Vui lòng chọn một môn học", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var addForm = new LessonSubjectAddForm(this);
            addForm.ShowDialog();
        }

        private void dtgvLessonList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var headerText = dtgvLessonList.Columns[e.ColumnIndex].HeaderText;

            switch (headerText)
            {
                case "Xóa":
                    HandleDelete(e.RowIndex);
                    break;
                case "Sửa":
                    HandleEdit(e.RowIndex);
                    break;
            }
        }

        private void HandleDelete(int rowIndex)
        {
            var dgvRow = dtgvLessonList.Rows[rowIndex];
            var lessonSymbol = dgvRow.Cells[1].Value?.ToString();
            var lessonName = dgvRow.Cells[2].Value?.ToString();

            var confirmation = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa \"{lessonSymbol} - {lessonName}\"?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmation == DialogResult.Yes)
            {
                var isDeleted = LessonSubjectController.Instance().DeleteLessonSubject(lessonSymbol);

                if (isDeleted)
                {
                    LoadData();
                    MessageBox.Show("Xóa bài học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Xóa bài học thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void HandleEdit(int rowIndex)
        {
            var dgvRow = dtgvLessonList.Rows[rowIndex];
            var lessonSymbol = dgvRow.Cells[1].Value?.ToString();
            var lessonName = dgvRow.Cells[2].Value?.ToString();
            var lt = Convert.ToInt32(dgvRow.Cells[3].Value);
            var bt = Convert.ToInt32(dgvRow.Cells[4].Value);
            var th = Convert.ToInt32(dgvRow.Cells[5].Value);

            var editForm = new LessonSubjectEditForm(this, lessonSymbol, lessonName, lt, bt, th);
            editForm.ShowDialog();
        }

        // Nhóm hàm hỗ trợ
        public string GetSym_Sub()
        {
            return selectedSubjectSymbol;
        }
    }
}
