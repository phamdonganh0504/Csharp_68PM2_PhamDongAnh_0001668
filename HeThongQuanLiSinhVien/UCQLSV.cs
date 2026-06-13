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

        // DANH SACH SINH VIEN
        private void DisplayStudentList()
        {
            dgvSinhVien.Rows.Clear();

            var dsSinhVien = (from sv in db.SinhViens
                              select sv).ToList();

            foreach (var item in dsSinhVien)
            {
                // Kiểm tra DateTime. Do lúc trước cấu trúc bảng cho null nên ép kiểu an toàn
                string ngayDisplay = "";
                if (item.NamSinh != null)
                {
                    ngayDisplay = item.NamSinh.Value.ToString("dd/MM/yyyy");
                }

                dgvSinhVien.Rows.Add(item.MaSV, item.HoTen, item.GioiTinh, ngayDisplay, item.Lop);
            }
        }

        // HÀM CLICK ĐƯA DATA TỪ DATAGRIDVIEW LÊN FORM
        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];

                txtMaSV.Text = row.Cells[0].Value?.ToString();
                txtHoTen.Text = row.Cells[1].Value?.ToString();
                cboGioiTinh.Text = row.Cells[2].Value?.ToString();
                cboLop.Text = row.Cells[4].Value?.ToString();

                string dateString = row.Cells[3].Value?.ToString();
                if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                {
                    dtpNgaySinh.Value = parsedDate;
                }
            }
        }

        // HÀM THÊM MỚI BẰNG LINQ TO SQL (CREATE)
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Bước 1: Khởi tạo một đối tượng SinhVien (Class này DBML tự sinh)
                SinhVien svNew = new SinhVien();
                svNew.MaSV = txtMaSV.Text;
                svNew.HoTen = txtHoTen.Text;
                svNew.Lop = cboLop.Text;
                svNew.GioiTinh = cboGioiTinh.Text;
                svNew.NamSinh = dtpNgaySinh.Value.Date; // SQL đã fix lỗi lấy đúng ngày

                // Bước 2: Gọi phương thức báo cho DBML biết cần chèn bản ghi
                db.SinhViens.InsertOnSubmit(svNew);

                // Bước 3: Đồng bộ dữ liệu thật xuống SQL Server (Thực thi)
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

        // HÀM CẬP NHẬT BẰNG LINQ TO SQL (UPDATE)
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                // Bước 1: LINQ - Truy vấn ra ĐÚNG sinh viên mang cái MaSV ở TextBox
                // Cú pháp Lambda Expression rút gọn của LINQ:
                var svSua = db.SinhViens.SingleOrDefault(sv => sv.MaSV == txtMaSV.Text);

                if (svSua != null)
                {
                    // Bước 2: Cập nhật properties
                    svSua.HoTen = txtHoTen.Text;
                    svSua.GioiTinh = cboGioiTinh.Text;
                    svSua.Lop = cboLop.Text;
                    svSua.NamSinh = dtpNgaySinh.Value.Date;

                    // Bước 3: Đẩy xuống SQL 
                    db.SubmitChanges();

                    MessageBox.Show("Sửa thành công!");
                    DisplayStudentList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // HÀM XÓA BẰNG LINQ TO SQL (DELETE)
        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Xác nhận xóa SV: {txtHoTen.Text}?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Lọc tìm đúng sinh viên đó
                    var svXoa = db.SinhViens.SingleOrDefault(sv => sv.MaSV == txtMaSV.Text);

                    if (svXoa != null)
                    {
                        // Hàm xóa mặc định của LINQ to SQL
                        db.SinhViens.DeleteOnSubmit(svXoa);
                        db.SubmitChanges();

                        MessageBox.Show("Xóa thành công!");
                        btnLamMoi_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi CSDL khi xóa: " + ex.Message);
                }
            }
        }

        // TÌM KIẾM ĐỈNH CAO NHỜ LINQ 
        private void btnTim_Click(object sender, EventArgs e)
        {
            string tukhoa = txtTimKiem.Text.ToLower().Trim();

            // Nếu ô trống thì Load tất cả
            if (string.IsNullOrEmpty(tukhoa))
            {
                DisplayStudentList();
                return;
            }

            // *** LINQ To SQL : SELECT LIKE '%...%' ***
            // Bạn có thể viết SQL cực kỳ gọn dưới dạng Method syntax:
            var danhSachLoc = db.SinhViens.Where(sv =>
                                    sv.MaSV.Contains(tukhoa) ||
                                    sv.HoTen.Contains(tukhoa) ||
                                    sv.Lop.Contains(tukhoa)
                              ).ToList();

            // Làm sạch lưới để đắp cái mới
            dgvSinhVien.Rows.Clear();

            // Duyệt và vẽ lên DataGridView thôi
            foreach (var item in danhSachLoc)
            {
                string ngayDisplay = "";
                if (item.NamSinh != null) ngayDisplay = item.NamSinh.Value.ToString("dd/MM/yyyy");

                dgvSinhVien.Rows.Add(item.MaSV, item.HoTen, item.GioiTinh, ngayDisplay, item.Lop);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaSV.Clear();
            txtHoTen.Clear();
            cboGioiTinh.SelectedIndex = -1;
            cboLop.SelectedIndex = -1;
            dtpNgaySinh.Value = DateTime.Now;
            txtTimKiem.Clear();

            txtMaSV.Focus();
            DisplayStudentList();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            btnTim_Click(sender, e);
        }

        // Để rỗng mấy sự kiện lỗi chuột nhầm này nhé 
        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}