using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace HeThongQuanLiSinhVien
{
    public partial class frm_DS_SinhVienLop : Form
    {
        
        QLSV_DataDataContext db = new QLSV_DataDataContext();

        
        private string maLopCanTraCuu;

        
        public frm_DS_SinhVienLop(string maLopDaChon, string tenLop)
        {
            InitializeComponent();

            
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Tra cứu sinh viên hệ lớp " + maLopDaChon;

            
            lblTieuDe.Text = $"DANH SÁCH SINH VIÊN - MÃ LỚP: {maLopDaChon} \n({tenLop})";
            this.maLopCanTraCuu = maLopDaChon;
        }

        private void frm_DS_SinhVienLop_Load(object sender, EventArgs e)
        {
            dgvDSSV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDSSV.ReadOnly = true;
            dgvDSSV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            try
            {
                
                var dsThoTuDB = db.SinhViens.Where(sv => sv.Lop == maLopCanTraCuu).ToList();

                
                if (dsThoTuDB.Count == 0)
                {
                    MessageBox.Show("Khảo sát cơ sở dữ liệu: Hệ lớp này hiện tại chưa có dữ liệu sinh viên nào ghi danh!", "CSDL Rỗng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return; 
                }

                
                var duLieuHienThi = dsThoTuDB.Select(sv => new
                {
                    Ma_SV = sv.MaSV,
                    Ho_Va_Ten = sv.HoTen,
                    Gioi_Tinh = sv.GioiTinh,
                   
                    Ngay_Sinh = sv.NamSinh != null ? sv.NamSinh.Value.ToString("dd/MM/yyyy") : "",
                    Truc_Thuoc = sv.Lop
                }).ToList();

                
                dgvDSSV.DataSource = duLieuHienThi;

                dgvDSSV.Columns["Ma_SV"].HeaderText = "Mã Số Sinh Viên";
                dgvDSSV.Columns["Ho_Va_Ten"].HeaderText = "Họ Và Tên";
                dgvDSSV.Columns["Gioi_Tinh"].HeaderText = "Giới Tính";
                dgvDSSV.Columns["Ngay_Sinh"].HeaderText = "Ngày/Tháng/Năm Sinh";
                dgvDSSV.Columns["Truc_Thuoc"].HeaderText = "Lớp Trực Thuộc";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mã CSDL xảy ra gián đoạn truyền phát list:\n " + ex.Message, "Rút trích Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}