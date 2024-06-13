using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MilitaryUnitManagement.Views;

namespace MilitaryUnitManagement
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnBattalion_Click(object sender, EventArgs e)
        {
            BattalionForm f = new BattalionForm();
            this.Hide();
            f.ShowDialog(this);
            this.Show();
        }
    }
}
