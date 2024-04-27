using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EduManager.Controllers;
using EduManager.Models;
using EduManager.Services;
using static System.Net.Mime.MediaTypeNames;

namespace EduManager.Views
{
    public partial class EduProgram_Form : Form
    {
        public EduProgram_Form()
        {
            InitializeComponent();
        }

        private void EduProgram_Load(object sender, EventArgs e)
        {
            EduProgramController.Instance().showData(dtgvEduProgram);
        }

        private void dtgvEduProgram_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            DataGridView dgv = sender as DataGridView;
            if (e.RowIndex >= 0 && (dgv.Columns[e.ColumnIndex].HeaderText == "Xóa" || dgv.Columns[e.ColumnIndex].HeaderText == "Sửa"))
            {
                string Id_Sub = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();

                if (dgv.Columns[e.ColumnIndex].HeaderText == "Xóa")
                {
                    EduProgram ed = new EduProgram(Id_Sub);
                    Subject s = new Subject(Id_Sub);

                    bool result1 = EduProgramController.Instance().removeAllData(ed);
                    bool result2 = SubjectController.Instance().removeData(s);

                    if (result1 && result2)
                    {
                        EduProgramController.Instance().showData(dtgvEduProgram);
                        MessageBox.Show("Xóa môn học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Xóa môn học thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (dgv.Columns[e.ColumnIndex].HeaderText == "Sửa")
                {
                    string tenMonHoc = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                    int lyThuyet = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells[2].Value.ToString());
                    int baiTap = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells[3].Value.ToString());
                    int thucHanh = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells[4].Value.ToString());

                    Subject s = new Subject(Id_Sub, tenMonHoc);
                    EduProgram ed1 = new EduProgram(Id_Sub, 1, lyThuyet);
                    EduProgram ed2 = new EduProgram(Id_Sub, 2, baiTap);
                    EduProgram ed3 = new EduProgram(Id_Sub, 3, thucHanh);

                    bool isUpdateSubject = SubjectController.Instance().editData(s);
                    bool isUpdateLyThuyet =EduProgramController.Instance().editData(ed1);
                    bool isUpdateBaiTap = EduProgramController.Instance().editData(ed2);
                    bool isUpdateThucHanh = EduProgramController.Instance().editData(ed3);

                    if (isUpdateSubject && isUpdateLyThuyet && isUpdateBaiTap && isUpdateThucHanh)
                    {
                        EduProgramController.Instance().showData(dtgvEduProgram);
                        MessageBox.Show("Sửa môn học thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Sửa môn học thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }

        }


        private void btnAddSubject_Click(object sender, EventArgs e)
        {
            Subjects f = new Subjects();
            // this.Hide();
            f.ShowDialog();
            // this.Show();
            EduProgramController.Instance().showData(dtgvEduProgram);
        }

    }
}
