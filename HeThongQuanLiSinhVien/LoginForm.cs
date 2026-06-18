using System;
using System.Windows.Forms;

namespace HeThongQuanLiSinhVien
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void async  btnLogin_Click(object sender, EventArgs e)
        {
            
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Yêu cầu hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            if (username == "admin" && password == "123456" || username == "luongxuanhieu" && password == "123456"|| username == "phamdonganh" && password == "123456")

            {
                MessageBox.Show("Xác thực thành công. Đang truy cập hệ thống...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
                this.Hide();
                frm_main mainForm = new frm_main();

                
                mainForm.FormClosed += (s, args) => this.Close();
                mainForm.Show();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc Mật khẩu không chính xác!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
    }
}