using MilitaryUnitManagement.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MilitaryUnitManagement.Controllers
{
    internal class PlatoonController
    {
        private static PlatoonController instance;
        private ConnectDatabase connectDatabase = ConnectDatabase.getInstance();

        private PlatoonController() { }

        // Khởi tạo một instance mới nếu chưa tồn tại, ngược lại trả về instance hiện tại
        public static PlatoonController Instance()
        {
            if (instance == null)
            {
                instance = new PlatoonController();
            }
            return instance;
        }

        // Hiển thị dữ liệu
        public void ShowData(DataGridView dgv, int FK_BattalionID,  int FK_CompanyID)
        {
            string query = "SELECT Platoon.ID, Platoon.Name, Platoon.Description FROM Platoon INNER JOIN Company ON Platoon.FK_CompanyID = Company.ID "+
                           "WHERE Company.FK_BattalionID = @FK_BattalionID AND Platoon.FK_CompanyID = @FK_CompanyID";

            // Tạo các tham số SqlParameter
            SqlParameter parameter1 = new SqlParameter("@FK_BattalionID", FK_BattalionID);
            SqlParameter parameter2 = new SqlParameter("@FK_CompanyID", FK_CompanyID);

            // Gửi tham số vào hàm ExecuteQuery và nhận lại DataTable
            dgv.Columns.Clear();
            dgv.DataSource = connectDatabase.ExecuteQuery(query, new SqlParameter[] { parameter1, parameter2 });
        }

        // Load data into ComboBox
        public void LoadComboBoxData_Battalion(ComboBox comboBox)
        {
            string query = "SELECT ID, Name FROM Battalion";
            DataTable dataTable = connectDatabase.ExecuteQuery(query);

            comboBox.DataSource = dataTable;
            comboBox.DisplayMember = "Name"; // Column to display
            comboBox.ValueMember = "Id";     // Column for value
        }

        public void LoadComboBoxData_Company(ComboBox comboBox, int selectedBattalion)
        {
            string query = "SELECT ID, Name FROM Company WHERE FK_BattalionID = @FK_BattalionID";
            SqlParameter parameter = new SqlParameter("@FK_BattalionID", selectedBattalion);
            DataTable dataTable = connectDatabase.ExecuteQuery(query, new SqlParameter[] { parameter });

            comboBox.DataSource = dataTable;
            comboBox.DisplayMember = "Name"; // Column to display
            comboBox.ValueMember = "Id";     // Column for value
        }
    }
}
