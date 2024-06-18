using Management.Views.EducationManagement;
using Management.Views.MilitaryUnitManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Job38_Management
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void quảnLýĐàoTạoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EduProgramForm f = new EduProgramForm();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void quảnLýPhânPhốiChươngTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LessonSubjectForm f = new LessonSubjectForm();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void tiểuĐoànToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BattalionForm f = new BattalionForm();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void đạiĐộiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompanyForm f = new CompanyForm();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void trungĐộiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlatoonForm f = new PlatoonForm();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
    }
}
