using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduManager.Services
{
    internal class ConnectDatabase
    {
        private static ConnectDatabase instance;
        private string _connectionString = @"Data Source=DUNGHAQUANG;Initial Catalog=EduManager;Integrated Security=True";

        private ConnectDatabase() { }

        public static ConnectDatabase getInstance()
        {
            if (instance == null)
            {
                instance = new ConnectDatabase();
            }
            return instance;
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parameter = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(query, sqlConnection);

                if (parameter != null)
                    cmd.Parameters.AddRange(parameter);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                data.Clear();
                adapter.Fill(data);
                sqlConnection.Close();
            }
            return data;
        }

        // Sử dụng để thêm sửa xóa
        public int ExecuteNonQuery(string query, SqlParameter[] parameter = null)
        {
            int rowsAffected = 0;
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(query, sqlConnection);

                if (parameter != null)
                    cmd.Parameters.AddRange(parameter);

                rowsAffected = cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            return rowsAffected;
        }

        public string GetValue(string query)
        {
            string value = null;
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    value = dataReader.GetValue(0).ToString();
                }
                sqlConnection.Close();
            }
            return value;
        }

        public SqlDataReader ExecuteReader(string query)
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }
    }
}
