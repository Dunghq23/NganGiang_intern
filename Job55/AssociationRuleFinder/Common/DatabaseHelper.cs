using System.Data.SqlClient;
using System.Data;

namespace Common
{
    public static class DatabaseHelper
    {
        private static string connectionString = $"Data Source={System.Environment.MachineName};Initial Catalog=SIFMES;Integrated Security=True;";

        // Phương thức để mở kết nối đến cơ sở dữ liệu
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Phương thức để thực thi một câu lệnh SQL và trả về một DataTable
        public static DataTable ExecuteQuery(string query)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        // Phương thức để thực thi câu lệnh không trả về dữ liệu (INSERT, UPDATE, DELETE)
        public static void ExecuteNonQuery(string query)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}