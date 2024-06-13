using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MilitaryUnitManagement.Services
{
    internal class ConfigView
    {
        public void AddActionColumns(DataGridView dgv)
        {
            if (dgv.Columns["Edit"] == null) // Add if not exists
            {
                string editImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources", "ICON_EDIT-24x24.png");
                var editColumn = new DataGridViewImageColumn
                {
                    Name = "Edit",
                    HeaderText = "Sửa",
                    Image = Image.FromFile(editImagePath),
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                };
                dgv.Columns.Add(editColumn);
            }

            if (dgv.Columns["Delete"] == null) // Add if not exists
            {
                string deleteImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources", "ICON_TRASH-24x24.png");
                var deleteColumn = new DataGridViewImageColumn
                {
                    Name = "Delete",
                    HeaderText = "Xóa",
                    Image = Image.FromFile(deleteImagePath),
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                };
                dgv.Columns.Add(deleteColumn);
                //deleteColumn.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#f44336");
            }
        }

        public void ConfigureDataGridViewReadOnly(DataGridView dgv, string columnName)
        {
            var column = dgv.Columns[columnName];
            if (column != null)
            {
                column.ReadOnly = true; // Set column to read-only
            }
        }

        public void ConfigureColumnHeaders(DataGridView dgv)
        {
            var headerStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = SystemColors.GrayText,
                ForeColor = SystemColors.WindowText,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            dgv.ColumnHeadersDefaultCellStyle = headerStyle;
        }

        public void ConfigureColumnAlignment(DataGridView dgv, string[] columnNames)
        {
            foreach (var columnName in columnNames)
            {
                var column = dgv.Columns[columnName];
                if (column != null)
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
        }

        public void ConfigureSelectionMode(DataGridView dgv)
        {
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#d8dfdc");
            dgv.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml("#000");
        }
    }
}
