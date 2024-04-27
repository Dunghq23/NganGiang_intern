using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool addSubject(Subject subject)
        {
            // query
            string query = "INSERT INTO Subjects VALUES (@Id_Sub , @name_Sub)";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("Id_Sub", subject.Id_Sub),
                new SqlParameter("name_Sub", subject.Name_Sub)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);
            // ExecuteNonQuery sẽ trả về số dòng được thay đổi tại Database. Ví dụ không có dữ liệu nào được thay đổi hay là thay đổi lỗi thì số dòng thay đổi sẽ = 0 => Lỗi

            return rowAffect > 0 ? true : false;
        }

        public void showData(DataGridView dgv)
        {
            string query = "Select \r\n\tId_Sub AS N'Mã môn học', \r\n\tName_Sub AS N'Tên môn học'\r\nFROM Subjects";
            dgv.DataSource = connectDatabase.ExecuteQuery(query);
        }

        public bool editData(Subject s)
        {
            string query = "UPDATE Subjects SET Name_Sub = @name_Sub WHERE Id_Sub = @Id_Sub";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("Id_Sub", s.Id_Sub),
                new SqlParameter("name_Sub", s.Name_Sub)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);
            return (rowAffect > 0 ? true : false);
        }

        public bool removeData(Subject s)
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
