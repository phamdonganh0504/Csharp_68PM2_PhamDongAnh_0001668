using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeThongQuanLiSinhVien
{
    public partial class StudentForm : Form
    {
        public StudentForm()
        {
            InitializeComponent();
            
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void StudentForm_Load(object sender, EventArgs e)
        {
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;

            dtpNgaySinh.CustomFormat = "dd/MM/yyyy";

            dgvSinhVien.Rows.Add("1", "Nguyễn Văn A", "Nam", "15/05/2005", "68PM1");

            dgvSinhVien.Rows.Add("2", "Trần Văn B", "Nam", "20/08/2005", "68PM2");
        }

        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];

                txtMaSV.Text = row.Cells[0].Value.ToString();

                txtHoTen.Text = row.Cells[1].Value.ToString();

                cboGioiTinh.Text = row.Cells[2].Value.ToString();

                        if (DateTime.TryParseExact(
                row.Cells[3].Value.ToString(),
                "dd/MM/yyyy",
                null,
                System.Globalization.DateTimeStyles.None,
                out DateTime parsedDate))
                        {
                            dtpNgaySinh.Value = parsedDate;
                        }

                cboLop.Text = row.Cells[4].Value.ToString();
            }
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];

                txtMaSV.Text = row.Cells[0].Value.ToString();

                txtHoTen.Text = row.Cells[1].Value.ToString();

                cboGioiTinh.Text = row.Cells[2].Value.ToString();

                    if (DateTime.TryParseExact(
                row.Cells[3].Value.ToString(),
                "dd/MM/yyyy",
                null,
                System.Globalization.DateTimeStyles.None,
                out DateTime parsedDate))
                    {
                        dtpNgaySinh.Value = parsedDate;
                }

                cboLop.Text = row.Cells[4].Value.ToString();
            }
        }

        private void txtMaSV_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            dgvSinhVien.Rows.Add(
            txtMaSV.Text,
            txtHoTen.Text,
            cboGioiTinh.Text,
            dtpNgaySinh.Value.ToString("dd/MM/yyyy"),
            cboLop.Text
            );

             MessageBox.Show("Thêm sinh viên thành công!");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.CurrentRow != null)
            {
                dgvSinhVien.CurrentRow.Cells[0].Value = txtMaSV.Text;

                dgvSinhVien.CurrentRow.Cells[1].Value = txtHoTen.Text;

                dgvSinhVien.CurrentRow.Cells[2].Value = cboGioiTinh.Text;

                dgvSinhVien.CurrentRow.Cells[3].Value =
                    dtpNgaySinh.Value.ToString("dd/MM/yyyy");

                dgvSinhVien.CurrentRow.Cells[4].Value = cboLop.Text;

                MessageBox.Show("Sửa thành công!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.CurrentRow != null)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc muốn xóa sinh viên này?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dgvSinhVien.Rows.RemoveAt(dgvSinhVien.CurrentRow.Index);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaSV.Clear();

            txtHoTen.Clear();

            cboGioiTinh.SelectedIndex = -1;

            cboLop.SelectedIndex = -1;

            dtpNgaySinh.Value = DateTime.Now;

            txtMaSV.Focus();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string tukhoa = txtTimKiem.Text.ToLower();

            foreach (DataGridViewRow row in dgvSinhVien.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string maSV = row.Cells[0].Value.ToString().ToLower();

                    string hoTen = row.Cells[1].Value.ToString().ToLower();

                    string lop = row.Cells[4].Value.ToString().ToLower();

                    if (maSV.Contains(tukhoa) ||
                        hoTen.Contains(tukhoa) ||
                        lop.Contains(tukhoa))
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
            }
        }
    }
}
