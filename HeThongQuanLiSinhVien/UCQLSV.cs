using System;
using System.Linq;
using System.Windows.Forms;

namespace HeThongQuanLiSinhVien
{
    public partial class UCQLSV : UserControl
    {
        
        QLSV_DataDataContext db = new QLSV_DataDataContext();

        public UCQLSV()
        {
            InitializeComponent();
        }

        private void UCQLSV_Load(object sender, EventArgs e)
        {
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.CustomFormat = "dd/MM/yyyy";

            cboLop.Items.Clear();
            cboLop.Items.Add("68PM1");
            cboLop.Items.Add("68PM2");
            cboLop.Items.Add("68PM3");
            cboLop.Items.Add("68PM4");

            DisplayStudentList();
        }

        // DANH SACH SINH VIEN HIEN THI LEN DATAGRIDVIEW
        private void DisplayStudentList()
        {
            try
            {
                dgvSinhVien.Rows.Clear();

                var dsSinhVien = db.SinhViens.ToList();

                foreach (var item in dsSinhVien)
                {

                    string ngayDisplay = "";
                    if (item.NamSinh != null)
                    {
                        ngayDisplay = item.NamSinh.Value.ToString("dd/MM/yyyy");
                    }

                    dgvSinhVien.Rows.Add(item.MaSV, item.HoTen, item.GioiTinh, ngayDisplay, item.Lop);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sinh viên: " + ex.Message);
            }
        }


        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0 && e.RowIndex < dgvSinhVien.Rows.Count)
            {
                
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];

                
                txtMaSV.Text = row.Cells[0].Value?.ToString();
                txtHoTen.Text = row.Cells[1].Value?.ToString();
                cboGioiTinh.Text = row.Cells[2].Value?.ToString();
                cboLop.Text = row.Cells[4].Value?.ToString();

                string dateString = row.Cells[3].Value?.ToString();
                if (!string.IsNullOrEmpty(dateString))
                {
                    if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime pDate))
                    {
                        dtpNgaySinh.Value = pDate;
                    }
                }
            }
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                
                SinhVien svNew = new SinhVien();
                svNew.MaSV = txtMaSV.Text;
                svNew.HoTen = txtHoTen.Text;
                svNew.Lop = cboLop.Text;
                svNew.GioiTinh = cboGioiTinh.Text;
                svNew.NamSinh = dtpNgaySinh.Value.Date; 

                
                db.SinhViens.InsertOnSubmit(svNew);

                
                db.SubmitChanges();

                MessageBox.Show("Thêm sinh viên thành công!");
                DisplayStudentList();
                btnLamMoi_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Trùng mã sinh viên hoặc Lỗi CSDL: " + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {

        }


        private void btnSua_Click(object sender, EventArgs e)
        {
                            
        }

        
        private void btnXoa_Click(object sender, EventArgs e)
        {
            
        }

         
        private void btnTim_Click(object sender, EventArgs e)
        {
            
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            btnTim_Click(sender, e);
        }

        
        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}