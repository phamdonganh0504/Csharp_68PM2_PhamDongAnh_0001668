using System;
using System.Linq;
using System.Windows.Forms;

namespace HeThongQuanLiSinhVien
{
    public partial class UCQLSV : UserControl
    {
        
        QLSV_DataDataContext db = new QLSV_DataDataContext();

        int trangHienTai = 1;
        int kichThuocTrang = 5;
        int tongSoTrang = 1;

        public UCQLSV()
        {
            InitializeComponent();
        }

        private void UCQLSV_Load(object sender, EventArgs e)
        {
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.CustomFormat = "dd/MM/yyyy";

            try
            {
                cboLop.Items.Clear();
                var listLopDayTuDB = db.LopHocs.Select(lh => lh.MaLop).ToList();
                foreach (var ml in listLopDayTuDB)
                {
                    cboLop.Items.Add(ml);
                }
            }
            catch (Exception)
            {
                
            }

            
            LoadSinhVien_PhanTrang();
        }

            
        //================================================

        private void LoadSinhVien_PhanTrang()
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim().ToLower();

                var svTruyVan = db.SinhViens.Where(sv =>
                sv.MaSV.ToLower().Contains(keyword) ||
                sv.HoTen.ToLower().Contains(keyword) ||
                sv.Lop.ToLower().Contains(keyword)
                );
                int tongSinhVien = svTruyVan.Count();
                tongSoTrang = (int)Math.Ceiling((double)tongSinhVien / kichThuocTrang);
                
                if (tongSoTrang == 0) tongSoTrang = 1;

                lblPage.Text = $"Trang {trangHienTai}/{tongSoTrang}";

                var ketQua = svTruyVan.OrderBy(s => s.MaSV)
                              .Skip((trangHienTai - 1) * kichThuocTrang)
                              .Take(kichThuocTrang)
                              .ToList();

                dgvSinhVien.Rows.Clear();
                foreach (var i in ketQua)
                {
                    string strDate = i.NamSinh != null ? i.NamSinh.Value.ToString("dd/MM/yyyy") : "";
                    dgvSinhVien.Rows.Add(i.MaSV, i.HoTen, i.GioiTinh, strDate, i.Lop);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi SQL " + ex.Message);
            }
        }
        //==================== NHÓM NÚT XỬ LÍ SỰ KIỆN PHÂN TRANG ============================
         
        private void btnFirst_Click(object sender, EventArgs e)
        {
            trangHienTai = 1;
            LoadSinhVien_PhanTrang();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (trangHienTai > 1)
            {
                trangHienTai--;
                LoadSinhVien_PhanTrang();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (trangHienTai < tongSoTrang)
            {
                trangHienTai++;
                LoadSinhVien_PhanTrang();
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            trangHienTai = tongSoTrang;
            LoadSinhVien_PhanTrang();
        }
        //==================== NHÓM NÚT XỬ LÍ SỰ KIỆN TÌM KIẾM ============================
        private void btnTim_Click(object sender, EventArgs e)
        {
            trangHienTai = 1;
            LoadSinhVien_PhanTrang() ;
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            trangHienTai = 1;
            LoadSinhVien_PhanTrang();
        }

        // ================================================


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
        // NÚT LÀM MỚI
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaSV.Text = ""; txtHoTen.Text = ""; txtTimKiem.Text = "";
            cboGioiTinh.SelectedIndex = -1; cboLop.SelectedIndex = -1;
            dtpNgaySinh.Value = DateTime.Now;

            trangHienTai = 1;
            LoadSinhVien_PhanTrang();

            txtMaSV.Focus();
        }

        //================================ CRUD SINH VIÊN =========================================
        //  THÊM SINH VIÊN MỚI
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

                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnLamMoi_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Trùng mã sinh viên hoặc Lỗi CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // SỬA THÔNG TIN SINH VIÊN
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Text == "")
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để sửa!", "Yêu cầu chọn dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                
                var sv = db.SinhViens.SingleOrDefault(x => x.MaSV == txtMaSV.Text);

                if (sv != null)
                {
                    
                    sv.HoTen = txtHoTen.Text;
                    sv.GioiTinh = cboGioiTinh.Text;
                    sv.Lop = cboLop.Text;
                    sv.NamSinh = dtpNgaySinh.Value.Date;

                    
                    db.SubmitChanges();

                    MessageBox.Show("Cập thông tin thành công!", "Thông báo" , MessageBoxButtons.OK, MessageBoxIcon.Information);
                     
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên này trong cơ sở dữ liệu!: \n" , "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo lỗi ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // XÓA SINH VIÊN
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Text == "")
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa!","Thông báo " , MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult thongbao = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (thongbao == DialogResult.Yes)
            {
                try
                {
                    
                    var svXoa = db.SinhViens.SingleOrDefault(x => x.MaSV == txtMaSV.Text);

                    if (svXoa != null)
                    {
                        db.SinhViens.DeleteOnSubmit(svXoa); 
                        db.SubmitChanges();                 

                        MessageBox.Show("Xóa thành công!", "Xác nhận kêt quả" , MessageBoxButtons.OK, MessageBoxIcon.Information );
                        btnLamMoi_Click(sender, e);          
                                      
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể thực hiện xoá ! " + ex.Message, "Từ chối hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}