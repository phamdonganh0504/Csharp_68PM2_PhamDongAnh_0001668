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

        //==================== HÀM LÕI PHÂN TRANG ============================
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
                MessageBox.Show("Đã xảy ra sự cố tải dữ liệu: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dgvSinhVien.ClearSelection();
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
            LoadSinhVien_PhanTrang();
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
                txtMaSV.ReadOnly = true;
                txtMaSV.BackColor = System.Drawing.Color.LightGray;

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

        //=========================== NÚT LÀM MỚI ===========================
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaSV.ReadOnly = false;
            txtMaSV.BackColor = System.Drawing.SystemColors.Window;

            txtMaSV.Text = "";
            txtHoTen.Text = "";
            txtTimKiem.Text = "";
            cboGioiTinh.SelectedIndex = -1;
            cboLop.SelectedIndex = -1;
            dtpNgaySinh.Value = DateTime.Now.AddYears(-21);

            trangHienTai = 1;
            LoadSinhVien_PhanTrang();

            txtMaSV.Focus();
        }

        //================================ CRUD SINH VIÊN =========================================
        private bool KiemTraDuLieuDauVao()
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text) ||
                string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(cboGioiTinh.Text) ||
                string.IsNullOrWhiteSpace(cboLop.Text))
            {
                MessageBox.Show("Bạn chưa nhập đầy đủ thông tin sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        //  THÊM SINH VIÊN MỚI
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (KiemTraDuLieuDauVao() == false)
            {
                return;
            }

            try
            {
                string maKiemTra = txtMaSV.Text.Trim();

                bool kTraTonTai = db.SinhViens.Any(sv => sv.MaSV == maKiemTra);
                if (kTraTonTai == true)
                {
                    MessageBox.Show("Mã sinh viên này đã tồn tại trong hệ thống! Vui lòng chọn mã khác.", "Trùng mã dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMaSV.Focus();
                    return;
                }

                SinhVien svNew = new SinhVien();
                svNew.MaSV = txtMaSV.Text.Trim();
                svNew.HoTen = txtHoTen.Text.Trim();
                svNew.Lop = cboLop.Text.Trim();
                svNew.GioiTinh = cboGioiTinh.Text;
                svNew.NamSinh = dtpNgaySinh.Value.Date;

                db.SinhViens.InsertOnSubmit(svNew);
                db.SubmitChanges();

                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnLamMoi_Click(sender, e);
            }
            catch (Exception ex)
            {
                this.db = new QLSV_DataDataContext();
                MessageBox.Show("Thêm dữ liệu gặp lỗi hệ thống CSDL: \n" + ex.Message, "Lỗi Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // SỬA THÔNG TIN SINH VIÊN
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (KiemTraDuLieuDauVao() == false)
            {
                return;
            }

            try
            {
                string maSvSua = txtMaSV.Text.Trim();

                var sv = db.SinhViens.SingleOrDefault(x => x.MaSV == maSvSua);

                if (sv != null)
                {
                    sv.HoTen = txtHoTen.Text.Trim();
                    sv.GioiTinh = cboGioiTinh.Text;
                    sv.Lop = cboLop.Text.Trim();
                    sv.NamSinh = dtpNgaySinh.Value.Date;

                    db.SubmitChanges();

                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnLamMoi_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy mã sinh viên này trong hệ thống. Vui lòng thử ấn Làm Mới!", "Cảnh báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                this.db = new QLSV_DataDataContext();
                MessageBox.Show("Quá trình ghi dữ liệu cập nhật gặp sự cố: \n" + ex.Message, "Lỗi Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // XOÁ THÔNG TIN SINH VIÊN
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng chọn một sinh viên trên bảng dữ liệu để tiến hành xóa!", "Yêu cầu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult thongbao = MessageBox.Show($"Bạn có chắc chắn muốn xóa bản ghi '{txtMaSV.Text.Trim()}' vĩnh viễn khỏi hệ thống không?", "Xác nhận xóa dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (thongbao == DialogResult.Yes)
            {
                try
                {
                    var svXoa = db.SinhViens.SingleOrDefault(x => x.MaSV == txtMaSV.Text.Trim());

                    if (svXoa != null)
                    {
                        db.SinhViens.DeleteOnSubmit(svXoa);
                        db.SubmitChanges();

                        MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnLamMoi_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy dữ liệu sinh viên để tiến hành xóa. Có thể dữ liệu đã được dọn dẹp trước đó!", "Cảnh báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    this.db = new QLSV_DataDataContext();
                    MessageBox.Show("Lỗi: Không thể thực hiện lệnh xóa do tồn tại các thông tin ràng buộc tại bảng dữ liệu khác.\n" + ex.Message, "Ràng buộc hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}