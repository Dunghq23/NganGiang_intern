using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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

        public static EduProgramController Instance()
        {
            if (instance == null)
            {
                instance = new EduProgramController();
            }
            return instance;
        }

        public void ConfigureDataGridView(DataGridView dgv, string columnName)
        {
            DataGridViewColumn column = dgv.Columns[columnName]; // Lấy cột
            if (column != null)
            {
                column.ReadOnly = true; // Đặt cột chỉ đọc
            }
        }

        public void showData(DataGridView dgv)
        {
            // Truy vấn dữ liệu
            string query = "SELECT * FROM EduProgramView";
            dgv.Columns.Clear();
            dgv.DataSource = ConnectDatabase.getInstance().ExecuteQuery(query);

            // Thêm cột "Sửa"
            DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn();
            editColumn.HeaderText = "Sửa";
            editColumn.Text = "Sửa";
            editColumn.UseColumnTextForButtonValue = true;
            dgv.Columns.Add(editColumn);

            // Thêm cột "Xóa"
            DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
            deleteColumn.HeaderText = "Xóa";
            deleteColumn.Text = "Xóa";
            deleteColumn.UseColumnTextForButtonValue = true;
            dgv.Columns.Add(deleteColumn);

            ConfigureDataGridView(dgv, "Mã môn học");
        }

        public bool addData(EduProgram ep)
        {
            string query = "INSERT INTO EduProgram VALUES (@FK_Id_Sub, @FK_Id_LS, @NumHour)";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("FK_Id_Sub", ep.FK_Id_Sub),
                new SqlParameter("FK_Id_LS", ep.FK_Id_LS),
                new SqlParameter("NumHour", ep.NumHour)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);

            return rowAffect > 0 ? true : false;
        }

        public bool editData(EduProgram ep)
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

        public bool removeData(EduProgram ed)
        {
            string query = "DELETE FROM EduProgram WHERE Id_EP = @Id_EP";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("Id_EP", ed.Id_EP)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);
            
            return rowAffect > 0 ? true : false;
        }

        public bool removeAllData(EduProgram ed)
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
