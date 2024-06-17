using System;
using System.Windows.Forms;

using EduManager.Models;
using EduManager.Controllers;
using System.Text.RegularExpressions;

namespace EduManager.Views
{
    public partial class Subjects : Form
    {
        private EduProgram_Form _eduProgramForm;

        public Subjects(EduProgram_Form eduProgramForm)
        {
            _eduProgramForm = eduProgramForm;
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
            if (string.IsNullOrWhiteSpace(txbSym_Sub.Text) || string.IsNullOrWhiteSpace(txbName_Sub.Text))
            {
                MessageBox.Show("Vui lòng điền ID môn học và tên môn học hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy ID mới
            int subjectID = SubjectController.Instance().getNewID();

            if (SubjectController.Instance().checkDuplicate(txbSym_Sub.Text.ToUpper(), subjectID, txbName_Sub.Text) > 0)
            {
                MessageBox.Show("Ký hiệu hoặc tên môn học đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var subjectAdded = AddSubject(subjectID);

            // Thêm chương trình giáo dục cho các checkbox được chọn
            bool ltAdded = AddEduProgram(subjectID, 1, (int)nmLT.Value);
            bool btAdded = AddEduProgram(subjectID, 2, (int)nmBT.Value);
            bool thAdded = AddEduProgram(subjectID, 3, (int)nmTH.Value);

            // Hiển thị thông báo dựa trên kết quả của các thao tác
            if (subjectAdded && ltAdded && btAdded && thAdded)
            {
                MessageBox.Show("Thêm môn học và chương trình liên quan thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _eduProgramForm.LoadData();
            }
            else
            {
                MessageBox.Show("Thêm môn học hoặc một số chương trình thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ResetInputFields(); // Đặt lại các trường nhập liệu
        }

        private bool AddSubject(int Id_Sub) // Thêm môn học
        {
            var subject = new Subject
            {
                Id_Sub = Id_Sub,
                Name_Sub = txbName_Sub.Text,
                Sym_Sub = txbSym_Sub.Text.ToUpper()
            };

            return SubjectController.Instance().addSubject(subject);
        }

        private bool AddEduProgram(int Id_Sub, int lessonType, int numHours) // Thêm chương trình giáo dục
        {
            var eduProgram = new EduProgram
            {
                FK_Id_Sub = Id_Sub,
                FK_Id_LS = lessonType,
                NumHour = numHours
            };

            return EduProgramController.Instance().AddData(eduProgram);
        }

        private void ResetInputFields() // Đặt lại các trường nhập liệu
        {
            txbSym_Sub.Text = "";
            txbName_Sub.Text = "";
            chbLT.Checked = false;
            chbBT.Checked = false;
            chbTH.Checked = false;
            nmLT.Value = 0;
            nmBT.Value = 0;
            nmTH.Value = 0;
        }

        private void txbId_Sub_TextChanged(object sender, EventArgs e)
        {
            var regex = new Regex("^[a-zA-Z0-9,-]+$");

            // Nếu đầu vào không hợp lệ, cảnh báo hoặc điều chỉnh
            try
            {
                if (!regex.IsMatch(txbSym_Sub.Text) && txbSym_Sub.Text != "")
                {
                    MessageBox.Show("Vui lòng chỉ nhập ký tự chữ và số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Loại bỏ ký tự cuối cùng (vì nó không hợp lệ)
                    txbSym_Sub.Text = txbSym_Sub.Text.Substring(0, txbSym_Sub.Text.Length - 1);
                    txbSym_Sub.SelectionStart = txbSym_Sub.Text.Length; // Đặt con trỏ cuối cùng
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txbName_Sub_TextChanged(object sender, EventArgs e)
        {
            var regex = new Regex(@"^[\p{L}\d\s\-,]+$");

            // Nếu đầu vào không hợp lệ, cảnh báo hoặc điều chỉnh
            try
            {
                if (!regex.IsMatch(txbName_Sub.Text) && txbName_Sub.Text != "")
                {
                    MessageBox.Show("Vui lòng chỉ nhập ký tự chữ và số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Loại bỏ ký tự cuối cùng (vì nó không hợp lệ)
                    txbName_Sub.Text = txbName_Sub.Text.Substring(0, txbName_Sub.Text.Length - 1);
                    txbName_Sub.SelectionStart = txbName_Sub.Text.Length; // Đặt con trỏ cuối cùng
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
