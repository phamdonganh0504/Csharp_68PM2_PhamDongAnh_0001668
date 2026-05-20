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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (username == "68PM2_phamdonganh_0001668@gmail.com" && password == "123456")
            {
                MessageBox.Show("Đăng nhập thành công!");
                // Mở frm_main và ẩn LoginForm
                frm_main main = new frm_main();
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại!");
            }
        }
    }
}