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

        private void btnCompany_Click(object sender, EventArgs e)
        {
            CompanyForm f = new CompanyForm();
            this.Hide();
            f.ShowDialog(this);
            this.Show();
        }

        private void btnPlatoon_Click(object sender, EventArgs e)
        {
            PlatoonForm f = new PlatoonForm();
            this.Hide();
            f.ShowDialog(this);
            this.Show();
        }
    }
}
