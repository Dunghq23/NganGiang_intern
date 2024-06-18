using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Management.Models.Military;
using Management.Services;

namespace MilitaryUnitManagement.Controllers.Military
{
    internal class CompanyController
    {
        private static CompanyController instance;
        private ConnectDatabase connectDatabase = ConnectDatabase.getInstance();

        private CompanyController() { }

        public static CompanyController Instance()
        {
            if (instance == null)
            {
                instance = new CompanyController();
            }
            return instance;
        }

        public void ShowData(DataGridView dgv, int ID)
        {
            string query = $"SELECT  ID, Name AS [Tên], Description AS [Mô tả] FROM Company WHERE FK_BattalionID = @FK_BattalionID";
            SqlParameter parameter = new SqlParameter("@FK_BattalionID", ID);
            dgv.Columns.Clear();
            dgv.DataSource = connectDatabase.ExecuteQuery(query, new SqlParameter[] { parameter });
        }


        public void LoadComboBoxData(ComboBox comboBox)
        {
            string query = "SELECT ID, Name FROM Battalion";
            DataTable dataTable = connectDatabase.ExecuteQuery(query);

            comboBox.DataSource = dataTable;
            comboBox.DisplayMember = "Name"; // Column to display
            comboBox.ValueMember = "Id";     // Column for value
        }


        // Thêm dữ liệu
        public bool AddData(Company c)
        {
            string query = "INSERT INTO Company VALUES (@Name, @Description, @FK_BattalionID)";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("Name", c.Name),
                new SqlParameter("FK_BattalionID", c.Description),
                new SqlParameter("Description", c.FK_BattalionID)
            };
            return connectDatabase.ExecuteNonQuery(query, para) > 0;
        }

        // Sửa dữ liệu
        public bool EditData(Company c)
        {
            string query = "UPDATE Company " +
                           "SET Name = @Name, " +
                           "Description = @Description, " +
                           "FK_BattalionID = @FK_BattalionID " +
                           "WHERE ID = @ID";

            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("ID", c.ID),
                new SqlParameter("Name", c.Name),
                new SqlParameter("Description", c.Description),
                new SqlParameter("FK_BattalionID", c.FK_BattalionID)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);
            return rowAffect > 0 ? true : false;
        }

        // Xóa dữ liệu
        public bool RemoveData(Company c)
        {
            string query = "DELETE FROM Company WHERE ID = @ID";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("ID", c.ID)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);

            return rowAffect > 0 ? true : false;
        }
    }
}
