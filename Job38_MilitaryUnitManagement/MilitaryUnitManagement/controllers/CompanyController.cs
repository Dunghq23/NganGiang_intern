using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MilitaryUnitManagement.Services;

namespace MilitaryUnitManagement.Controllers
{
    internal class CompanyController
    {
        private static CompanyController instance;
        private ConnectDatabase connectDatabase = ConnectDatabase.getInstance();

        private CompanyController() { }

        // Khởi tạo một instance mới nếu chưa tồn tại, ngược lại trả về instance hiện tại
        public static CompanyController Instance()
        {
            if (instance == null)
            {
                instance = new CompanyController();
            }
            return instance;
        }

        // Hiển thị dữ liệu
        public void ShowData(DataGridView dgv, int ID)
        {
            string query = $"SELECT * FROM Company WHERE FK_BattalionID = @FK_BattalionID";
            SqlParameter parameter = new SqlParameter("@FK_BattalionID", ID);
            dgv.Columns.Clear();
            dgv.DataSource = connectDatabase.ExecuteQuery(query, new SqlParameter[] { parameter });
        }


        // Load data into ComboBox
        public void LoadComboBoxData(ComboBox comboBox)
        {
            string query = "SELECT ID, Name FROM Battalion"; 
            DataTable dataTable = connectDatabase.ExecuteQuery(query);

            comboBox.DataSource = dataTable;
            comboBox.DisplayMember = "Name"; // Column to display
            comboBox.ValueMember = "Id";     // Column for value
        }
    }
}
