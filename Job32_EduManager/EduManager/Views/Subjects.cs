using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EduManager.Models;
using EduManager.Controllers;

namespace EduManager.Views
{
    public partial class Subjects : Form
    {
        public Subjects()
        {
            InitializeComponent();
        }

        private void chbLT_CheckedChanged(object sender, EventArgs e)
        {
            nmLT.Visible = chbLT.Checked ? true : false;
        }

        private void chbBT_CheckedChanged(object sender, EventArgs e)
        {
            nmBT.Visible = chbBT.Checked ? true : false;
        }

        private void chbTH_CheckedChanged(object sender, EventArgs e)
        {
            nmTH.Visible = chbTH.Checked ? true : false;
        }

        private void btnAddSubject_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu các trường bắt buộc là rỗng
            if (string.IsNullOrWhiteSpace(txbId_Sub.Text) || string.IsNullOrWhiteSpace(txbName_Sub.Text))
            {
                MessageBox.Show("Vui lòng điền ID môn học và tên môn học hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thêm môn học
            var subjectAdded = AddSubject();

            // Thêm chương trình giáo dục cho các checkbox được chọn
            bool ltAdded = AddEduProgram(1, (int)nmLT.Value);
            bool btAdded = AddEduProgram(2, (int)nmBT.Value);
            bool thAdded = AddEduProgram(3, (int)nmTH.Value);

            // Hiển thị thông báo dựa trên kết quả của các thao tác
            if (subjectAdded && ltAdded && btAdded && thAdded)
            {
                MessageBox.Show("Thêm môn học và chương trình liên quan thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Thêm môn học hoặc một số chương trình thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ResetInputFields(); // Đặt lại các trường nhập liệu
        }

        private bool AddSubject() // Thêm môn học
        {
            var subject = new Subject
            {
                Id_Sub = txbId_Sub.Text,
                Name_Sub = txbName_Sub.Text
            };

            return SubjectController.Instance().addSubject(subject);
        }

        private bool AddEduProgram(int lessonType, int numHours) // Thêm chương trình giáo dục
        {
            var eduProgram = new EduProgram
            {
                FK_Id_Sub = txbId_Sub.Text,
                FK_Id_LS = lessonType,
                NumHour = numHours
            };

            return EduProgramController.Instance().AddData(eduProgram);
        }

        private void ResetInputFields() // Đặt lại các trường nhập liệu
        {
            txbId_Sub.Text = "";
            txbName_Sub.Text = "";
            chbLT.Checked = false;
            chbBT.Checked = false;
            chbTH.Checked = false;
            nmLT.Value = 0;
            nmBT.Value = 0;
            nmTH.Value = 0;
        }
    }
}
