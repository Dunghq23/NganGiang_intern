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
    public partial class BattalionForm : Form
    {
        private ConfigView configView;

        public BattalionForm()
        {
            InitializeComponent();
            configView = new ConfigView(); // Initialize ConfigView instance
            dtgv.RowTemplate.Height = 40;

        }

        private void BattalionForm_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadData()
        {
            BattalionController.Instance().ShowData(dtgv);
            // Use the ConfigView instance to configure the DataGridView
            configView.ConfigureDataGridViewReadOnly(dtgv, "ID");
            configView.ConfigureColumnHeaders(dtgv);
            configView.ConfigureColumnAlignment(dtgv, new string[] { "ID", "Name" });
            configView.AddActionColumns(dtgv);
            configView.ConfigureSelectionMode(dtgv);
        }
    }
}
