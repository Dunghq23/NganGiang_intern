using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using EduManager.Controllers;
using EduManager.Models;

namespace EduManager.Views
{
    public partial class LessonSubject_Add : Form
    {
        private readonly LessonSubject_Form _lessonSubjectForm;

        public LessonSubject_Add(LessonSubject_Form lessonSubjectForm)
        {
            InitializeComponent();
            _lessonSubjectForm = lessonSubjectForm;
        }

        // ==== Sự kiện (Event Handlers) ====
        private void LessonSubject_Add_Load(object sender, EventArgs e)
        {
            txbSym_Sub.Text = _lessonSubjectForm.GetSym_Sub();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string subjectSymbol = txbSym_Sub.Text;
            int subjectId = LessonSubjectController.Instance().GetIDSub(subjectSymbol);

            if (IsOverLimit(subjectSymbol))
            {
                return; // Dừng lại nếu quá giới hạn
            }

            string lesson = txbLesson.Text;
            string lessonName = txbLessonName.Text;

            AddLessonSubjects(subjectId, lesson, lessonName);

            _lessonSubjectForm.LoadData(); // Cập nhật dữ liệu giao diện
            MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ==== Xử lý dữ liệu (Data Processing) ====
        private bool IsOverLimit(string subjectSymbol)
        {
            DataTable lessonData = LessonSubjectController.Instance().ShowLessonList(subjectSymbol);

            int totalLyThuyet = CalculateColumnSum(lessonData, "Lý thuyết");
            int totalBaiTap = CalculateColumnSum(lessonData, "Bài tập");
            int totalThucHanh = CalculateColumnSum(lessonData, "Thực hành");

            int allowedLyThuyet = LessonSubjectController.Instance().GetNumHour(subjectSymbol, "Lý thuyết");
            int allowedBaiTap = LessonSubjectController.Instance().GetNumHour(subjectSymbol, "Bài tập");
            int allowedThucHanh = LessonSubjectController.Instance().GetNumHour(subjectSymbol, "Thực hành");

            if (totalLyThuyet + nmLT.Value > allowedLyThuyet)
            {
                DisplayWarning("Lý thuyết", allowedLyThuyet);
                return true;
            }
            if (totalBaiTap + nmBT.Value > allowedBaiTap)
            {
                DisplayWarning("Bài tập", allowedBaiTap);
                return true;
            }
            if (totalThucHanh + nmTH.Value > allowedThucHanh)
            {
                DisplayWarning("Thực hành", allowedThucHanh);
                return true;
            }

            return false; // Không vượt quá giới hạn
        }

        private void AddLessonSubjects(int subjectId, string lesson, string lessonName)
        {
            var lessonTypes = new[] { "Lý thuyết", "Bài tập", "Thực hành" };
            var numHours = new[] { nmLT.Value, nmBT.Value, nmTH.Value };

            for (int i = 0; i < lessonTypes.Length; i++)
            {
                int numHour = Convert.ToInt32(numHours[i]);
                LessonSubject newLessonSubject = new LessonSubject(lesson, lessonName, subjectId, i + 1, numHour);
                LessonSubjectController.Instance().AddLessonSubject(newLessonSubject);
            }
        }

        // ==== Giao diện người dùng (UI) ====
        private void DisplayWarning(string subjectType, int limit)
        {
            MessageBox.Show(
                $"Số giờ {subjectType} đã vượt quá {limit} tiết trong Chương trình đào tạo",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        // ==== Hàm tiện ích (Utility Functions) ====
        public static int CalculateColumnSum(DataTable dataTable, string columnName)
        {
            if (!dataTable.Columns.Contains(columnName))
            {
                throw new ArgumentException($"Cột '{columnName}' không tồn tại trong bảng dữ liệu.");
            }

            return dataTable.AsEnumerable()
                .Where(row => row[columnName] != DBNull.Value)
                .Sum(row => Convert.ToInt32(row[columnName]));
        }
    }
}
