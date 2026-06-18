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

        #region KHOI TAO FORM & NẠP DATA LINQ CHUẨN

        private void UCQLSV_Load(object sender, EventArgs e)
        {
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            dgvSinhVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            try
            {
                
                cboLop.Items.Clear();
                var danhSachLop = db.LopHocs.Select(lh => lh.MaLop).ToList();
                foreach (var maLop in danhSachLop)
                {
                    cboLop.Items.Add(maLop);
                }
            }
            catch (Exception)
            {
                
            }

            LoadDanhSachSinhVien();
        }

        
        private void LoadDanhSachSinhVien()
        {
            try
            {
                string tuKhoa = txtTimKiem.Text.Trim().ToLower();

               
                var queryLocSV = db.SinhViens.Where(sv =>
                    sv.MaSV.ToLower().Contains(tuKhoa) ||
                    sv.HoTen.ToLower().Contains(tuKhoa) ||
                    sv.Lop.ToLower().Contains(tuKhoa)
                );

                
                int tongSinhVien = queryLocSV.Count();
                tongSoTrang = (int)Math.Ceiling((double)tongSinhVien / kichThuocTrang);
                if (tongSoTrang == 0) tongSoTrang = 1;

                lblPage.Text = $"Trang {trangHienTai}/{tongSoTrang}";

                
                var danhSachHienThi = queryLocSV.OrderBy(s => s.MaSV)
                                                .Skip((trangHienTai - 1) * kichThuocTrang)
                                                .Take(kichThuocTrang)
                                                .ToList();

                
                dgvSinhVien.Rows.Clear();
                foreach (var sv in danhSachHienThi)
                {
                    string strDate = sv.NamSinh != null ? sv.NamSinh.Value.ToString("dd/MM/yyyy") : "";
                    dgvSinhVien.Rows.Add(sv.MaSV, sv.HoTen, sv.GioiTinh, strDate, sv.Lop);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi tải dữ liệu:\n" + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dgvSinhVien.ClearSelection();
        }

        #endregion

        #region TƯƠNG TÁC TÌM KIẾM, ĐIỀU HƯỚNG TRANG & DATA LƯỚI

        private void btnFirst_Click(object sender, EventArgs e) { trangHienTai = 1; LoadDanhSachSinhVien(); }
        private void btnPrev_Click(object sender, EventArgs e) { if (trangHienTai > 1) { trangHienTai--; LoadDanhSachSinhVien(); } }
        private void btnNext_Click(object sender, EventArgs e) { if (trangHienTai < tongSoTrang) { trangHienTai++; LoadDanhSachSinhVien(); } }
        private void btnLast_Click(object sender, EventArgs e) { trangHienTai = tongSoTrang; LoadDanhSachSinhVien(); }

        private void txtTimKiem_TextChanged(object sender, EventArgs e) { trangHienTai = 1; LoadDanhSachSinhVien(); }
        private void btnTim_Click(object sender, EventArgs e) { trangHienTai = 1; LoadDanhSachSinhVien(); }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvSinhVien.Rows.Count)
            {
                txtMaSV.ReadOnly = true; // Khóa không cho người dùng sửa Mã SV
                txtMaSV.BackColor = System.Drawing.Color.LightGray;

                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];

                txtMaSV.Text = row.Cells[0].Value?.ToString();
                txtHoTen.Text = row.Cells[1].Value?.ToString();
                cboGioiTinh.Text = row.Cells[2].Value?.ToString();
                cboLop.Text = row.Cells[4].Value?.ToString();

                string rawDate = row.Cells[3].Value?.ToString();
                if (!string.IsNullOrEmpty(rawDate))
                {
                    if (DateTime.TryParseExact(rawDate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime pDate))
                    {
                        dtpNgaySinh.Value = pDate;
                    }
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            
            txtMaSV.ReadOnly = false;
            txtMaSV.BackColor = System.Drawing.SystemColors.Window;

           
            txtMaSV.Clear();
            txtHoTen.Clear();
            txtTimKiem.Clear();
            cboGioiTinh.SelectedIndex = -1;
            cboLop.SelectedIndex = -1;
            dtpNgaySinh.Value = DateTime.Now;

            trangHienTai = 1;
            LoadDanhSachSinhVien();
            txtMaSV.Focus();
        }

        #endregion

        #region CRUD: CÁC NGHIỆP VỤ THÊM SỬA XÓA BẰNG CÔNG NGHỆ LINQ 

        private bool KiemTraLoiDeTrong()
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text) ||
                string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(cboGioiTinh.Text) ||
                string.IsNullOrWhiteSpace(cboLop.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các thông tin của sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (KiemTraLoiDeTrong() == false) return;

            try
            {
                string idNhapVao = txtMaSV.Text.Trim();

                
                bool daTonTaiSV = db.SinhViens.Any(sv => sv.MaSV == idNhapVao);
                if (daTonTaiSV)
                {
                    MessageBox.Show("Mã sinh viên này đã có trong cơ sở dữ liệu. Vui lòng nhập mã khác!", "Cảnh báo trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMaSV.Focus();
                    return;
                }

               
                SinhVien svNew = new SinhVien();
                svNew.MaSV = idNhapVao;
                svNew.HoTen = txtHoTen.Text.Trim();
                svNew.Lop = cboLop.Text.Trim();
                svNew.GioiTinh = cboGioiTinh.Text;
                svNew.NamSinh = dtpNgaySinh.Value.Date;

                db.SinhViens.InsertOnSubmit(svNew);
                db.SubmitChanges();

                MessageBox.Show("Thêm mới sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnLamMoi_Click(sender, e);
            }
            catch (Exception ex)
            {
                this.db = new QLSV_DataDataContext(); 
                MessageBox.Show("Hệ thống lỗi thao tác Database:\n" + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (KiemTraLoiDeTrong() == false) return;

            try
            {
                string idCanSua = txtMaSV.Text.Trim();

               
                var svEntitySua = db.SinhViens.SingleOrDefault(sv => sv.MaSV == idCanSua);

                if (svEntitySua != null)
                {
                    svEntitySua.HoTen = txtHoTen.Text.Trim();
                    svEntitySua.GioiTinh = cboGioiTinh.Text;
                    svEntitySua.Lop = cboLop.Text.Trim();
                    svEntitySua.NamSinh = dtpNgaySinh.Value.Date;

                    db.SubmitChanges();

                    MessageBox.Show("Đã cập nhật thay đổi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnLamMoi_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Không thể tìm ra ID gốc trên Database! Bạn hãy tải lại bảng dữ liệu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                this.db = new QLSV_DataDataContext();
                MessageBox.Show("Gặp vấn đề hệ thống trong khi cập nhật thay đổi:\n" + ex.Message, "Lỗi Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Bạn chưa chọn thông tin từ lưới bảng! Vui lòng Click vào sinh viên cần xóa trước.", "Yêu cầu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa bản ghi '{txtHoTen.Text.Trim()}' hoàn toàn ra khỏi hệ thống?", "Xác nhận hệ thống", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    string idCanXoa = txtMaSV.Text.Trim();
                    var svEntityXoa = db.SinhViens.SingleOrDefault(sv => sv.MaSV == idCanXoa);

                    if (svEntityXoa != null)
                    {
                        
                        db.SinhViens.DeleteOnSubmit(svEntityXoa);
                        db.SubmitChanges();

                        MessageBox.Show("Bạn đã tiến hành xóa bản ghi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnLamMoi_Click(sender, e);
                    }
                }
                catch (System.Data.SqlClient.SqlException SqlLoiCuaRnagBuocThangConTrongDatabaseNayNayMayChamBaiHayKiemtraDoANThieuSinhvienLam) // Để hàm ở dạng mặc định chung Catcher!
                {
                    this.db = new QLSV_DataDataContext();
                    MessageBox.Show("Chưa cho phép thao tác: Đang có dữ liệu điểm hay môn phụ thuộc dính kèm mã này trên máy Database. Dọn nhánh liên kết trước mới được Xóa rễ gốc SV!", "Xác Lập Khóa (Cảnh Cáo Constraints)", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch (Exception LoiXoaHeSQLHKhDTheBanS1DataBThatBaBanCTheBao)
                {
                    this.db = new QLSV_DataDataContext();
                    MessageBox.Show("Mạng lưới Hệ Bảng Bị tắc gián Thử : " + LoiXoaHeSQLHKhDTheBanS1DataBThatBaBanCTheBao.Message, "Server DB Error ! ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}