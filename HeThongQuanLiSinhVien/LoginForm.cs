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

            if (username == "admin" && password == "")
            {
                MessageBox.Show("Đăng nhập thành công!");

                this.Hide();

                frm_main main = new frm_main();

                main.FormClosed += (s, args) => this.Close();

                main.Show();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại!");
            }
        }
    }
}