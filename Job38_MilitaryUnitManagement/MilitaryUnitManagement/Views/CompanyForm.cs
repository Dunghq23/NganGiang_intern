using MilitaryUnitManagement.Controllers;
using MilitaryUnitManagement.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MilitaryUnitManagement.Views
{
    public partial class CompanyForm : Form
    {

        private ConfigView configView;
        private int selectedValue;
        private string selectedText;

        public CompanyForm()
        {
            InitializeComponent();
            configView = new ConfigView(); // Initialize ConfigView instance
            dtgv.RowTemplate.Height = 40;
        }

        private void CompanyForm_Load(object sender, EventArgs e)
        {
            
            CompanyController.Instance().LoadComboBoxData(cb);
        }

        private void loadData(int ID)
        {
            CompanyController.Instance().ShowData(dtgv, ID);
            // Use the ConfigView instance to configure the DataGridView
            configView.ConfigureDataGridViewReadOnly(dtgv, "ID");
            configView.ConfigureColumnHeaders(dtgv);
            configView.ConfigureColumnAlignment(dtgv, new string[] { "ID", "Name" });
            configView.AddActionColumns(dtgv);
            configView.ConfigureSelectionMode(dtgv);
        }

        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem selectedItem có phải là DataRowView hay không
            DataRowView selectedItem = cb.SelectedItem as DataRowView;

            if (selectedItem != null)
            {
                // Trích xuất giá trị ID từ DataRowView
                if (int.TryParse(selectedItem["ID"].ToString(), out selectedValue))
                {
                    // Lấy tên từ DataRowView
                    selectedText = selectedItem["Name"].ToString();

                    // Load dữ liệu dựa trên selectedValue
                    loadData(selectedValue);
                }
                else
                {
                    // Xử lý trường hợp không thể chuyển đổi giá trị ID thành số nguyên
                    MessageBox.Show("Vui lòng chọn ID hợp lệ.");
                }
            }
            else
            {
                // Xử lý khi không có item được chọn hoặc item không phải là DataRowView
                MessageBox.Show("Vui lòng chọn ID hợp lệ.");
            }
        }
    }
}
