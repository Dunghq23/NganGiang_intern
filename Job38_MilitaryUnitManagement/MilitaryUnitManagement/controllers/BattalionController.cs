using MilitaryUnitManagement.Models;
using MilitaryUnitManagement.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MilitaryUnitManagement.Controllers
{
    internal class BattalionController
    {

        private static BattalionController instance;
        private ConnectDatabase connectDatabase = ConnectDatabase.getInstance();

        private BattalionController() { }

        // Khởi tạo một instance mới nếu chưa tồn tại, ngược lại trả về instance hiện tại
        public static BattalionController Instance()
        {
            if (instance == null)
            {
                instance = new BattalionController();
            }
            return instance;
        }

        // Hiển thị dữ liệu
        public void ShowData(DataGridView dgv)
        {
            string query = "SELECT * FROM Battalion";
            dgv.Columns.Clear();
            dgv.DataSource = ConnectDatabase.getInstance().ExecuteQuery(query);
        }

        // Thêm dữ liệu
        public bool AddData(Battalion b)
        {
            string query = "INSERT INTO EduProgram VALUES (@FK_Id_Sub, @FK_Id_LS, @NumHour)";
            SqlParameter[] para = new SqlParameter[]
            {
                //new SqlParameter("FK_Id_Sub", ep.FK_Id_Sub),
                //new SqlParameter("FK_Id_LS", ep.FK_Id_LS),
                //new SqlParameter("NumHour", ep.NumHour)
            };
            return connectDatabase.ExecuteNonQuery(query, para) > 0;
        }

        // Sửa dữ liệu
        //public bool EditData(Battalion ep)
        //{
        //    string query = "UPDATE EduProgram " +
        //                   "SET NumHour = @NumHour " +
        //                   "WHERE FK_Id_Sub = @FK_Id_Sub AND FK_Id_LS = @FK_Id_LS;";
        //    SqlParameter[] para = new SqlParameter[]
        //    {
        //        new SqlParameter("FK_Id_Sub", ep.FK_Id_Sub),
        //        new SqlParameter("FK_Id_LS", ep.FK_Id_LS),
        //        new SqlParameter("NumHour", ep.NumHour)
        //    };
        //    int rowAffect = connectDatabase.ExecuteNonQuery(query, para);
        //    return rowAffect > 0 ? true : false;
        //}

        // Xóa dữ liệu
        //public bool RemoveData(EduProgram ed)
        //{
        //    string query = "DELETE FROM EduProgram WHERE Id_EP = @Id_EP";
        //    SqlParameter[] para = new SqlParameter[]
        //    {
        //        new SqlParameter("Id_EP", ed.Id_EP)
        //    };
        //    int rowAffect = connectDatabase.ExecuteNonQuery(query, para);

        //    return rowAffect > 0 ? true : false;
        //}

        // Xóa tất cả dữ liệu
        //public bool RemoveAllData(EduProgram ed)
        //{

        //    string query = "DELETE FROM EduProgram WHERE FK_Id_Sub = @FK_Id_Sub";
        //    SqlParameter[] para = new SqlParameter[]
        //    {
        //        new SqlParameter("FK_Id_Sub", ed.FK_Id_Sub)
        //    };
        //    int rowAffect = connectDatabase.ExecuteNonQuery(query, para);
        //    return rowAffect > 0 ? true : false;
        //}
    }
}
