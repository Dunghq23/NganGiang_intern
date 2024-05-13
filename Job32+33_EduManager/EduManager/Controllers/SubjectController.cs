using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using EduManager.Models;
using EduManager.Services;

namespace EduManager.Controllers
{
    internal class SubjectController
    {
        private static SubjectController instance;
        private ConnectDatabase connectDatabase = ConnectDatabase.getInstance();

        private SubjectController() { }

        public static SubjectController Instance()
        {
            if (instance == null)
            {
                instance = new SubjectController();
            }
            return instance;
        }

        public int getNewID()
        {
            string query = "EXEC GetNextSubjectId"; 
            string result = connectDatabase.GetValue(query);
            int value = int.Parse(result);
            return value; 
        }

        public int checkDuplicate(String sym_sub, int subjectId)
        {
            string query = $"SELECT COUNT(*) FROM subjects WHERE Sym_Sub = '{sym_sub}' AND Id_Sub <> {subjectId};";
            
            string result = connectDatabase.GetValue(query);
            int value = int.Parse(result);
            return value;
        }

        // Thêm dữ liệu
        public bool addSubject(Subject subject)
        {
            string query = "INSERT INTO Subjects VALUES (@Id_Sub , @name_Sub, @Sym_Sub)";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("Id_Sub", subject.Id_Sub),
                new SqlParameter("name_Sub", subject.Name_Sub),
                new SqlParameter("Sym_Sub", subject.Sym_Sub)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);

            return rowAffect > 0;
        }

        // Hiển thị dữ liệu
        public void showData(DataGridView dgv)
        {
            string query = "Select \r\n\tId_Sub AS N'Mã môn học', \r\n\tName_Sub AS N'Tên môn học'\r\nFROM Subjects";
            dgv.DataSource = connectDatabase.ExecuteQuery(query);
        }

        // Sửa dữ liệu
        public bool editData(Subject s)
        {
            string query = "UPDATE Subjects SET Name_Sub = @name_Sub, Sym_Sub = @Sym_Sub WHERE Id_Sub = @Id_Sub";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("Id_Sub", s.Id_Sub),
                new SqlParameter("Sym_Sub", s.Sym_Sub),
                new SqlParameter("name_Sub", s.Name_Sub)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);
            return (rowAffect > 0 ? true : false);
        }

        // Xóa dữ liệu
        public bool RemoveData(Subject s)
        {
            string query = "DELETE FROM Subjects WHERE Id_Sub = @Id_Sub";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("Id_Sub", s.Id_Sub)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);

            return rowAffect > 0 ? true : false;
        }
    }
}
