using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EduManager.Views;


namespace EduManager
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnEduManage_Click(object sender, EventArgs e)
        {
            EduProgram_Form f = new EduProgram_Form();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}
