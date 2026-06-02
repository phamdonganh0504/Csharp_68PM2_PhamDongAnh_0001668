using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeThongQuanLiSinhVien
{
    public partial class UCQLSV : UserControl
    {
        SqlConnection conn = new SqlConnection(
            @"Data Source=DESKTOP-M0VLVFF\SQLEXPRESS03;
              Initial Catalog=HeThongQLSV;
              Integrated Security=True");
        public UCQLSV()
        {
            InitializeComponent();

        }
        private void DisplayStudentList()
        {
            try
            {
                dgvSinhVien.Rows.Clear();

                conn.Open();

                string sql = "SELECT * FROM SinhVien";

                SqlCommand cmd =
                    new SqlCommand(sql, conn);

                SqlDataReader dr =
                    cmd.ExecuteReader();

                while (dr.Read())
                {
                    dgvSinhVien.Rows.Add(
                        dr["MaSV"].ToString(),
                        dr["HoTen"].ToString(),
                        dr["GioiTinh"].ToString(),
                        dr["NamSinh"].ToString(),
                        dr["Lop"].ToString()
                    );
                }

                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void UCQLSV_Load(object sender, EventArgs e)
        {
            dtpNgaySinh.Format =
                DateTimePickerFormat.Custom;

            dtpNgaySinh.CustomFormat =
                "dd/MM/yyyy";

            cboLop.Items.Clear();

            cboLop.Items.Add("CNTT1");
            cboLop.Items.Add("CNTT2");
            cboLop.Items.Add("CNTT3");
            cboLop.Items.Add("CNTT4");

            DisplayStudentList();
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

        private void btnThem_Click(object sender,EventArgs e)
        {
            try
            {
                conn.Open();

                string sql =
                @"INSERT INTO SinhVien
        (
            MaSV,
            HoTen,
            Lop,
            GioiTinh,
            NamSinh
        )
        VALUES
        (
            @MaSV,
            @HoTen,
            @Lop,
            @GioiTinh,
            @NamSinh
        )";

                SqlCommand cmd =
                    new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue(
                    "@MaSV",
                    txtMaSV.Text);

                cmd.Parameters.AddWithValue(
                    "@HoTen",
                    txtHoTen.Text);

                cmd.Parameters.AddWithValue(
                    "@Lop",
                    cboLop.Text);

                cmd.Parameters.AddWithValue(
                    "@GioiTinh",
                    cboGioiTinh.Text);

                cmd.Parameters.AddWithValue(
                    "@NamSinh",
                    dtpNgaySinh.Value.Year);

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show(
                    "Thêm sinh viên thành công!");

                DisplayStudentList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
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
