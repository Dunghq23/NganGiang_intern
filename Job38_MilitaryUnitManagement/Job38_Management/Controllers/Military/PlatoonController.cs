using Management.Models.Military;
using Management.Services;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace MilitaryUnitManagement.Controllers.Military
{
    internal class PlatoonController
    {
        private static PlatoonController instance;
        private ConnectDatabase connectDatabase = ConnectDatabase.getInstance();

        private PlatoonController() { }

        public static PlatoonController Instance()
        {
            if (instance == null)
            {
                instance = new PlatoonController();
            }
            return instance;
        }

        // Hiển thị dữ liệu
        public void ShowData(DataGridView dgv, int FK_BattalionID, int FK_CompanyID)
        {
            string query = "SELECT Platoon.ID, Platoon.Name AS [Tên], Platoon.Description AS [Mô tả] FROM Platoon INNER JOIN Company ON Platoon.FK_CompanyID = Company.ID " +
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

            if (dataTable.Rows.Count != 0)
            {
                comboBox.DataSource = dataTable;
                comboBox.DisplayMember = "Name"; // Column to display
                comboBox.ValueMember = "Id";     // Column for value
            }

        }
        
        // Thêm dữ liệu
        public bool AddData(Platoon p)
        {
            string query = "INSERT INTO Platoon(Name, Description, FK_CompanyID) VALUES (@Name, @Description, @FK_CompanyID)";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("Name", p.Name),
                new SqlParameter("Description", p.Description),
                new SqlParameter("FK_CompanyID", p.FK_CompanyID)
            };
            return connectDatabase.ExecuteNonQuery(query, para) > 0;
        }

        // Sửa dữ liệu
        public bool EditData(Platoon p)
        {
            string query = "UPDATE Platoon " +
                           "SET Name = @Name, " +
                           "Description = @Description, " +
                           "FK_CompanyID = @FK_CompanyID " +
                           "WHERE ID = @ID";

            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("ID", p.ID),
                new SqlParameter("Name", p.Name),
                new SqlParameter("Description", p.Description),
                new SqlParameter("FK_CompanyID", p.FK_CompanyID)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);
            return rowAffect > 0 ? true : false;
        }

        // Xóa dữ liệu
        public bool RemoveData(Platoon p)
        {
            string query = "DELETE FROM Platoon WHERE ID = @ID";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("ID", p.ID)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);

            return rowAffect > 0 ? true : false;
        }
    }
}
