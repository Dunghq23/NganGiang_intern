using System;
using System.Data.SqlClient;
using System.Windows.Forms;

using EduManager.Models;
using EduManager.Services;

namespace EduManager.Controllers
{
    internal class EduProgramController
    {
        private static EduProgramController instance;
        private ConnectDatabase connectDatabase = ConnectDatabase.getInstance();

        private EduProgramController() { }

        // Khởi tạo một instance mới nếu chưa tồn tại, ngược lại trả về instance hiện tại
        public static EduProgramController Instance()
        {
            if (instance == null)
            {
                instance = new EduProgramController();
            }
            return instance;
        }

        // Hiển thị dữ liệu
        public void ShowData(DataGridView dgv)
        {
            string query = "SELECT * FROM EduProgramView";
            dgv.Columns.Clear();
            dgv.DataSource = ConnectDatabase.getInstance().ExecuteQuery(query);
        }


        

        // Thêm dữ liệu
        public bool AddData(EduProgram ep)
        {
            string query = "INSERT INTO EduProgram VALUES (@FK_Id_Sub, @FK_Id_LS, @NumHour)";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("FK_Id_Sub", ep.FK_Id_Sub),
                new SqlParameter("FK_Id_LS", ep.FK_Id_LS),
                new SqlParameter("NumHour", ep.NumHour)
            };
            return connectDatabase.ExecuteNonQuery(query, para) > 0;
        }

        // Sửa dữ liệu
        public bool EditData(EduProgram ep)
        {
            string query = "UPDATE EduProgram " +
                           "SET NumHour = @NumHour " +
                           "WHERE FK_Id_Sub = @FK_Id_Sub AND FK_Id_LS = @FK_Id_LS;";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("FK_Id_Sub", ep.FK_Id_Sub),
                new SqlParameter("FK_Id_LS", ep.FK_Id_LS),
                new SqlParameter("NumHour", ep.NumHour)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);
            return rowAffect > 0 ? true : false;
        }

        // Xóa dữ liệu
        public bool RemoveData(EduProgram ed)
        {
            string query = "DELETE FROM EduProgram WHERE Id_EP = @Id_EP";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("Id_EP", ed.Id_EP)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);

            return rowAffect > 0 ? true : false;
        }

        // Xóa tất cả dữ liệu
        public bool RemoveAllData(EduProgram ed)
        {

            string query = "DELETE FROM EduProgram WHERE FK_Id_Sub = @FK_Id_Sub";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("FK_Id_Sub", ed.FK_Id_Sub)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);
            return rowAffect > 0 ? true : false;
        }
    }
}
