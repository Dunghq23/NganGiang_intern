using Management.Models.Education;
using Management.Services;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Management.Controllers.Education
{
    internal class LessonSubjectController
    {
        private static LessonSubjectController instance;
        private ConnectDatabase connectDatabase = ConnectDatabase.getInstance();

        private LessonSubjectController() { }

        // Khởi tạo một instance mới nếu chưa tồn tại, ngược lại trả về instance hiện tại
        public static LessonSubjectController Instance()
        {
            if (instance == null)
            {
                instance = new LessonSubjectController();
            }
            return instance;
        }

        #region == Nhóm hàm thao tác CRUD (CREATE, READ, UPDATE, DELETE) ==
        public bool AddLessonSubject(LessonSubject leSub)
        {
            //les_Unit, int les_Name, int fK_Id_Sub, int fK_Id_LS, int numHour
            string query = "INSERT INTO LessonSub(Les_Unit, Les_Name, FK_Id_Sub, FK_Id_LS, NumHour) VALUES (@Les_Unit, @Les_Name, @FK_Id_Sub, @FK_Id_LS, @NumHour)";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("Les_Unit", leSub.Les_Unit),
                new SqlParameter("Les_Name", leSub.Les_Name),
                new SqlParameter("FK_Id_Sub", leSub.FK_Id_Sub),
                new SqlParameter("FK_Id_LS", leSub.FK_Id_LS),
                new SqlParameter("NumHour", leSub.NumHour)
            };
            return connectDatabase.ExecuteNonQuery(query, para) > 0;
        }
        public bool EditLessonSubject(LessonSubject leSub, string oldLes_Unit)
        {
            string query = "UPDATE LessonSub SET Les_Unit = @Les_Unit, Les_Name = @Les_Name, FK_Id_Sub = @FK_Id_Sub, FK_Id_LS = @FK_Id_LS, NumHour = @NumHour " +
                            $"WHERE Les_Unit = N'{oldLes_Unit}' AND FK_Id_LS = @FK_Id_LS";
            SqlParameter[] para = new SqlParameter[]
            {
        new SqlParameter("Les_Name", leSub.Les_Name),
        new SqlParameter("FK_Id_Sub", leSub.FK_Id_Sub),
        new SqlParameter("FK_Id_LS", leSub.FK_Id_LS),
        new SqlParameter("NumHour", leSub.NumHour),
        new SqlParameter("Les_Unit", leSub.Les_Unit)
            };
            return connectDatabase.ExecuteNonQuery(query, para) > 0;
        }
        public bool DeleteLessonSubject(string lesson)
        {
            string query = $"DELETE FROM LessonSub WHERE Les_Unit = '{lesson}'";
            return connectDatabase.ExecuteNonQuery(query) > 0;
        }


        #endregion

        #region == Nhóm hàm đọc dữ liệu ==
        public void ShowSubjectsList(DataGridView dgv)
        {
            string query = "SELECT Sym_Sub AS [Ký hiệu], Name_Sub AS [Tên môn học] FROM Subjects ";
            dgv.Columns.Clear();
            dgv.DataSource = ConnectDatabase.getInstance().ExecuteQuery(query);
        }
        public void ShowLessonList(DataGridView dgv, String sym_sub)
        {
            dgv.Columns.Clear();
            string query = $"SELECT * FROM LessonSubjectView WHERE \"Ký hiệu môn học\" = '{sym_sub}'";
            DataTable dataTable = ConnectDatabase.getInstance().ExecuteQuery(query);
            dgv.DataSource = dataTable;

            if (dgv.Columns["Edit"] == null) // Chỉ thêm nếu chưa tồn tại
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources", "ICON_EDIT-24x24.png");
                var deleteColumn = new DataGridViewImageColumn
                {
                    Name = "Edit",
                    HeaderText = "Sửa",
                    Image = Image.FromFile(imagePath),
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                };
                dgv.Columns.Add(deleteColumn);
            }
            if (dgv.Columns["Delete"] == null) // Chỉ thêm nếu chưa tồn tại
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources", "ICON_TRASH-24x24.png");
                var deleteColumn = new DataGridViewImageColumn
                {
                    Name = "Delete",
                    HeaderText = "Xóa",
                    Image = Image.FromFile(imagePath),
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                };
                dgv.Columns.Add(deleteColumn);
            }

        }
        public DataTable ShowLessonList(String sym_sub)
        {
            string query = $"SELECT * FROM LessonSubjectView WHERE \"Ký hiệu môn học\" = '{sym_sub}'";
            DataTable dataTable = ConnectDatabase.getInstance().ExecuteQuery(query);
            return dataTable;
        }
        #endregion

        #region == Nhóm hàm khác ==
        public int GetIDSub(string sym_sub)
        {
            string query = $"SELECT Id_Sub FROM Subjects WHERE Sym_Sub = '{sym_sub}'";
            string result = ConnectDatabase.getInstance().GetValue(query);
            return Convert.ToInt32(result);
        }
        public int GetNumHour(String sym_sub, String learningStyle)
        {
            string query = $"SELECT [{learningStyle}] FROM EduProgramView WHERE [Ký hiệu] = '{sym_sub}'";
            string result = ConnectDatabase.getInstance().GetValue(query);
            return Convert.ToInt32(result);
        }

        public int CheckDuplicateLessonUnit(string symSub, string lessonUnit)
        {
            string query = "SELECT COUNT(*) FROM LessonSub AS L " +
                           "INNER JOIN Subjects AS S ON L.FK_Id_Sub = S.Id_Sub " +
                          $"WHERE Les_Unit LIKE N'{lessonUnit}' " +
                          $"AND S.Sym_Sub = '{symSub}';";
            string result = connectDatabase.GetValue(query);
            int value = int.Parse(result);
            return value;
        }

        public int CheckDuplicateLessonName(string symSub, string lessonName)
        {
            string query = "SELECT COUNT(*) FROM LessonSub AS L " +
                           "INNER JOIN Subjects AS S ON L.FK_Id_Sub = S.Id_Sub " +
                          $"WHERE Les_Name LIKE N'{lessonName}' " +
                          $"AND S.Sym_Sub = '{symSub}';";
            string result = connectDatabase.GetValue(query);
            int value = int.Parse(result);
            return value;
        }
        #endregion
    }
}
