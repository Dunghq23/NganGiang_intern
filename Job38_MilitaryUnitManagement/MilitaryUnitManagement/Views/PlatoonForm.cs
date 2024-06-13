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
    public partial class PlatoonForm : Form
    {
        private ConfigView configView;
        private int selectedBattalionID;
        private int selectedCompanyID;

        public PlatoonForm()
        {
            InitializeComponent();
            configView = new ConfigView(); // Initialize ConfigView instance
        }

        private void PlatoonForm_Load(object sender, EventArgs e)
        {
            // Load dữ liệu vào ComboBox khi Form được tải
            PlatoonController.Instance().LoadComboBoxData_Battalion(cbBattalion);
        }

        private void loadData(int FK_BattalionID, int FK_CompanyID)
        {
            PlatoonController.Instance().ShowData(dtgv, FK_BattalionID, FK_CompanyID);
            // Sử dụng ConfigView để cấu hình DataGridView
            configView.ConfigureDataGridViewReadOnly(dtgv, "ID");
            configView.ConfigureColumnHeaders(dtgv);
            configView.ConfigureColumnAlignment(dtgv, new string[] { "ID", "Name" });
            configView.AddActionColumns(dtgv);
            configView.ConfigureSelectionMode(dtgv);
        }

        private void cbBattalion_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem selectedItem có phải là DataRowView hay không
            DataRowView selectedItem = cbBattalion.SelectedItem as DataRowView;

            if (selectedItem != null)
            {
                if (int.TryParse(selectedItem["ID"].ToString(), out selectedBattalionID))
                {
                    string selectedText = selectedItem["Name"].ToString();

                    // Gọi hàm LoadComboBoxData_Company của PlatoonController với ID của cbBattalion đang được chọn
                    PlatoonController.Instance().LoadComboBoxData_Company(cbCompany, selectedBattalionID);

                    // Tải dữ liệu với ID của cbBattalion đang được chọn
                    loadData(selectedBattalionID, selectedCompanyID);
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn ID hợp lệ.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn ID hợp lệ.");
            }
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem selectedItem có phải là DataRowView hay không
            DataRowView selectedItem = cbCompany.SelectedItem as DataRowView;

            if (selectedItem != null)
            {
                if (int.TryParse(selectedItem["ID"].ToString(), out selectedCompanyID))
                {
                    string selectedText = selectedItem["Name"].ToString();

                    // Tải dữ liệu với ID của cbCompany đang được chọn
                    loadData(selectedBattalionID, selectedCompanyID);
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn ID hợp lệ.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn ID hợp lệ.");
            }
        }
    }
}
