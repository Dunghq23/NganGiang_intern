using MilitaryUnitManagement.Models;
using MilitaryUnitManagement.Services;
using System.Data.SqlClient;
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
            string query = "INSERT INTO Battalion VALUES (@Name, @Description)";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("Name", b.Name),
                new SqlParameter("Description", b.Description)
            };
            return connectDatabase.ExecuteNonQuery(query, para) > 0;
        }

        // Sửa dữ liệu
        public bool EditData(Battalion b)
        {
            string query = "UPDATE Battalion " +
                           "SET Name = @Name, " +
                           "Description = @Description " +
                           "WHERE ID = @ID";

            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("ID", b.ID),
                new SqlParameter("Name", b.Name),
                new SqlParameter("Description", b.Description)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);
            return rowAffect > 0 ? true : false;
        }

        // Xóa dữ liệu
        public bool RemoveData(Battalion b)
        {
            string query = "DELETE FROM Battalion WHERE ID = @ID";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("ID", b.ID)
            };
            int rowAffect = connectDatabase.ExecuteNonQuery(query, para);

            return rowAffect > 0 ? true : false;
        }
    }
}
