using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace HeThongQuanLiSinhVien
{
    public partial class UCQLLH : UserControl
    {
        QLSV_DataDataContext db = new QLSV_DataDataContext(); 
        int trangHienTai = 1; 
        int kichThuocTrang = 5; 
        int tongSoTrang = 1; 
        public UCQLLH()
        {
            InitializeComponent();
            KhoiTaoSuKien();
        }

        //======== HÀM KHỞI TẠO SỰ KIỆN CHO CÁC CONTROL TRONG USER CONTROL ========
        private void KhoiTaoSuKien()
        {
            this.btnFirst.Click += btnFirst_Click;
            this.btnPrev.Click += btnPrev_Click;
            this.btnNext.Click += btnNext_Click;
            this.btnLast.Click += btnLast_Click;

            this.btnAdd.Click += btnAdd_Click;
            this.btnEdit.Click += btnEdit_Click;
            this.btnDelete.Click += btnDelete_Click;
            this.btnRefresh.Click += btnRefresh_Click;

            this.btnSearch.Click += btnSearch_Click;
            this.txtSearch.TextChanged += txtSearch_TextChanged;

            this.dgvClass.CellClick += dgvClass_CellClick;
             this.btnViewStudent.Click += btnViewStudent_Click;
        }

        //============ CHỨC NĂNG TÌM KIẾM VÀ PHÂN TRANG ==============
        private void LoadDuLieuLopHoc()
        {
            try
            {
                string tuKhoa = txtSearch.Text.Trim().ToLower();

            
                var queryLopHoc = db.LopHocs.Where(lh =>
                    lh.MaLop.ToLower().Contains(tuKhoa) ||
                    lh.TenLop.ToLower().Contains(tuKhoa));


                int tongBanGhi = queryLopHoc.Count();
                tongSoTrang = (int)Math.Ceiling((double)tongBanGhi / kichThuocTrang);
                if (tongSoTrang == 0) tongSoTrang = 1;
                lblPage.Text = $"Trang {trangHienTai}/{tongSoTrang} | {tongBanGhi} bản ghi";


                var danhSachHienThi = queryLopHoc.OrderBy(lh => lh.MaLop)
                                      .Skip((trangHienTai - 1) * kichThuocTrang)
                                      .Take(kichThuocTrang)
                                      .Select(lh => new
                                      {
                                          MaLop = lh.MaLop,
                                          TenLop = lh.TenLop,
                                          GhiChu = lh.GhiChu
                                      }).ToList();

                dgvClass.DataSource = danhSachHienThi;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi truy vấn Cơ sở dữ liệu: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UCQLLH_Load(object sender, EventArgs e)
        {
            dgvClass.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClass.ReadOnly = true;
            txtClassId.ReadOnly = true;
            LoadDuLieuLopHoc();
        }

        //======================
        private void txtSearch_TextChanged(object sender, EventArgs e) 
        { 
            trangHienTai = 1; LoadDuLieuLopHoc(); 
        }
        private void btnSearch_Click(object sender, EventArgs e) 
        {
            trangHienTai = 1; LoadDuLieuLopHoc(); 
        }

        private void btnFirst_Click(object sender, EventArgs e) 
        {
            trangHienTai = 1; LoadDuLieuLopHoc();
        }
        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (trangHienTai > 1)
            { 
                trangHienTai--; LoadDuLieuLopHoc(); 
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (trangHienTai < tongSoTrang) 
            {
                trangHienTai++; LoadDuLieuLopHoc(); 
            }
        }
        private void btnLast_Click(object sender, EventArgs e)
        {
            trangHienTai = tongSoTrang; LoadDuLieuLopHoc(); 
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtClassId.Text = "";
            txtClassCode.Text = "";
            txtClassName.Text = "";
            txtNote.Text = "";
            txtSearch.Text = "";

            trangHienTai = 1;
            LoadDuLieuLopHoc();
            txtClassCode.Focus();
        }

        //======== SỰ KIỆN CELL CLICK TRÊN DATAGRIDVIEW ========
        private void dgvClass_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.RowIndex < dgvClass.Rows.Count)
            {
                DataGridViewRow row = dgvClass.Rows[e.RowIndex];


                txtClassCode.Text = row.Cells["MaLop"].Value?.ToString();
                txtClassName.Text = row.Cells["TenLop"].Value?.ToString();
                txtNote.Text = row.Cells["GhiChu"].Value?.ToString();

                txtClassId.Text = txtClassCode.Text;
            }
        }
        //================== HÀM KIỂM TRA ĐẦU VÀO TRỐNG ==================
        private bool KiemTraNhapLieu()
        {

            if (string.IsNullOrWhiteSpace(txtClassCode.Text) || string.IsNullOrWhiteSpace(txtClassName.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã lớp và Tên lớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtClassCode.Focus();
                return false;
            }
            return true;
        }

        // ======== SỰ KIỆN THÊM MỚI LỚP HỌC ========
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (KiemTraNhapLieu() == false) 
                return;

            try
            {
                string maLopMoi = txtClassCode.Text.Trim();

                bool kiemTraTonTai = db.LopHocs.Any(l => l.MaLop == maLopMoi);
                if (kiemTraTonTai == true)
                {
                    MessageBox.Show($"Mã lớp '{maLopMoi}' đã tồn tại! Vui lòng nhập mã khác.", "Lỗi trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtClassCode.Focus();
                    return;
                }


                LopHoc lopNew = new LopHoc();
                lopNew.MaLop = maLopMoi;
                lopNew.TenLop = txtClassName.Text.Trim();
                lopNew.GhiChu = txtNote.Text.Trim();

                db.LopHocs.InsertOnSubmit(lopNew);
                db.SubmitChanges();

                MessageBox.Show("Thêm mới lớp học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);


                btnRefresh_Click(sender, e);
            }
            catch (Exception ex)
            {
                this.db = new QLSV_DataDataContext();
                MessageBox.Show("Lỗi hệ thống khi thêm dữ liệu: \n" + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ======== SỰ KIỆN CẬP NHẬT LỚP HỌC ========
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (KiemTraNhapLieu() == false) 
                return;

            try
            {
                string maLopCanSua = txtClassCode.Text.Trim();

                var lopUpdate = db.LopHocs.SingleOrDefault(l => l.MaLop == maLopCanSua);

                if (lopUpdate != null)
                {

                    lopUpdate.TenLop = txtClassName.Text.Trim();
                    lopUpdate.GhiChu = txtNote.Text.Trim();

                    db.SubmitChanges();

                    MessageBox.Show("Cập nhật thông tin lớp học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDuLieuLopHoc();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy mã lớp này trên CSDL! Có thể bạn đang nhập sai mã .", "Lỗi tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống khi sửa dữ liệu:\n " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ======== SỰ KIỆN XÓA LỚP HỌC ========
        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtClassCode.Text))
            {
                MessageBox.Show("Vui lòng click chọn một lớp học trên bảng để tiến hành xóa!", "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string maLopXoa = txtClassCode.Text.Trim();


            DialogResult cauHoiXacNhan = MessageBox.Show($"Bạn có chắc chắn muốn xóa vĩnh viễn Lớp '{txtClassName.Text}' ra khỏi hệ thống?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (cauHoiXacNhan == DialogResult.Yes)
            {
                try
                {
                    var lopHocDaChon = db.LopHocs.SingleOrDefault(l => l.MaLop == maLopXoa);

                    if (lopHocDaChon != null)
                    {
                        db.LopHocs.DeleteOnSubmit(lopHocDaChon);
                        db.SubmitChanges();

                        MessageBox.Show("Đã xóa lớp học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnRefresh_Click(sender, e);
                    }
                }
                catch (System.Data.SqlClient.SqlException)
                {
                    MessageBox.Show("Lỗi rành buộc Dữ Liệu: Bạn KHÔNG THỂ XÓA lớp này!\nLý do: Đang có sinh viên thuộc biên chế lớp này trong hệ thống. Vui lòng chuyển hoặc xóa các sinh viên đó trước.",
                                    "Khóa hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ======================= XEM DANH SÁCH SINH VIÊN TRONG LỚP =======================
        private void btnViewStudent_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(txtClassCode.Text))
            {
                MessageBox.Show("Vui lòng click chọn một Lớp học ở bảng dữ liệu bên dưới để xem sỹ số!", "Yêu cầu cung cấp dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string parseCodeMaLopTruyCap = txtClassCode.Text.Trim();
            string parseCodeTenLop = txtClassName.Text.Trim(); 

            
            frm_DS_SinhVienLop hienThiCuaSoChiTietDS_TheoMlop = new frm_DS_SinhVienLop(parseCodeMaLopTruyCap, parseCodeTenLop);

             
            hienThiCuaSoChiTietDS_TheoMlop.ShowDialog();
        }
    }

}