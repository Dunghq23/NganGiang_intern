using EduManager.Controllers;
using EduManager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace EduManager.Views
{
    public partial class LessonSubject_Edit_Form : Form
    {
        private readonly LessonSubject_Form _lessonSubjectForm;
        private int _ltOldValue, _btOldValue, _thOldValue;

        // === Nhóm hàm khởi tạo và thiết lập ===
        public LessonSubject_Edit_Form(LessonSubject_Form lessonSubjectForm, string lesson, string lessonName, int lt, int bt, int th)
        {
            InitializeComponent();
            _lessonSubjectForm = lessonSubjectForm;
            SetInitialValues(lesson, lessonName, lt, bt, th);
        }

        private void SetInitialValues(string lesson, string lessonName, int lt, int bt, int th)
        {
            txbSym_Sub.Text = _lessonSubjectForm.GetSym_Sub();
            txbLesson.Text = lesson;
            txbLessonName.Text = lessonName;
            nmLT.Value = lt;
            nmBT.Value = bt;
            nmTH.Value = th;
        }

        private void LessonSubject_Edit_Form_Load(object sender, EventArgs e)
        {
            _ltOldValue = (int)nmLT.Value;
            _btOldValue = (int)nmBT.Value;
            _thOldValue = (int)nmTH.Value;
        }

        // === Nhóm hàm xử lý sự kiện ===
        private void btnSave_Click(object sender, EventArgs e)
        {
            var lessonUpdates = CreateLessonUpdates();

            if (ValidateHours(lessonUpdates))
            {
                bool success = lessonUpdates.All(LessonSubjectController.Instance().EditLessonSubject);

                if (success)
                {
                    _lessonSubjectForm.LoadData();
                    ShowMessage("Sửa thành công", "Thông báo", MessageBoxIcon.Information);
                }
            }
        }

        // === Nhóm hàm tiện ích ===
        private List<LessonSubject> CreateLessonUpdates()
        {
            string lessonName = txbLessonName.Text;
            string unit = txbLesson.Text;
            string symSub = txbSym_Sub.Text;

            int subId = LessonSubjectController.Instance().GetIDSub(symSub);

            return new List<LessonSubject>
            {
                new LessonSubject { Les_Name = lessonName, Les_Unit = unit, FK_Id_Sub = subId, FK_Id_LS = 1, NumHour = (int)nmLT.Value },
                new LessonSubject { Les_Name = lessonName, Les_Unit = unit, FK_Id_Sub = subId, FK_Id_LS = 2, NumHour = (int)nmBT.Value },
                new LessonSubject { Les_Name = lessonName, Les_Unit = unit, FK_Id_Sub = subId, FK_Id_LS = 3, NumHour = (int)nmTH.Value }
            };
        }

        private bool ValidateHours(List<LessonSubject> lessonUpdates)
        {
            string symSub = txbSym_Sub.Text;
            DataTable lessonData = LessonSubjectController.Instance().ShowLessonList(symSub);

            int maxLyThuyet = LessonSubjectController.Instance().GetNumHour(symSub, "Lý thuyết");
            int maxBaiTap = LessonSubjectController.Instance().GetNumHour(symSub, "Bài tập");
            int maxThucHanh = LessonSubjectController.Instance().GetNumHour(symSub, "Thực hành");

            int currentLyThuyet = CalculateColumnSum(lessonData, "Lý thuyết");
            int currentBaiTap = CalculateColumnSum(lessonData, "Bài tập");
            int currentThucHanh = CalculateColumnSum(lessonData, "Thực hành");

            bool isLTValid = (int)nmLT.Value - _ltOldValue + currentLyThuyet <= maxLyThuyet;
            bool isBTValid = (int)nmBT.Value - _btOldValue + currentBaiTap <= maxBaiTap;
            bool isTHValid = (int)nmTH.Value - _thOldValue + currentThucHanh <= maxThucHanh;

            return ValidateNewHours(isLTValid, isBTValid, isTHValid);
        }

        private static bool ValidateNewHours(bool isLTValid, bool isBTValid, bool isTHValid)
        {
            if (!isLTValid)
            {
                ShowWarning("Số giờ lý thuyết đã vượt quá giới hạn trong Chương trình đào tạo");
                return false;
            }

            if (!isBTValid)
            {
                ShowWarning("Số giờ bài tập đã vượt quá giới hạn trong Chương trình đào tạo");
                return false;
            }

            if (!isTHValid)
            {
                ShowWarning("Số giờ thực hành đã vượt quá giới hạn trong Chương trình đào tạo");
                return false;
            }

            return true;
        }

        private static void ShowWarning(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static void ShowMessage(string message, string title, MessageBoxIcon icon)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

        public static int CalculateColumnSum(DataTable dataTable, string columnName)
        {
            if (!dataTable.Columns.Contains(columnName))
            {
                throw new ArgumentException("Tên cột không hợp lệ");
            }

            return dataTable.AsEnumerable()
                            .Where(row => row[columnName] != DBNull.Value)
                            .Sum(row => Convert.ToInt32(row[columnName]));
        }
    }
}
