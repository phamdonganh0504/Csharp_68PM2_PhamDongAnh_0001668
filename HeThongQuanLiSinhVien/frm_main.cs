using System;
using System.Windows.Forms;

namespace HeThongQuanLiSinhVien
{
    public partial class frm_main : Form
    {
        public frm_main()
        {
            InitializeComponent();
        }

        private void frm_main_Load(object sender, EventArgs e)
        {
            // Load UCQLSV by default
            LoadStudent();
        }

        private void quảnLýSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadStudent();
        }

        private void quảnLýLớpHọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadClass();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void LoadStudent()
        {
            try
            {
                UCQLSV sv = new UCQLSV();
                pnl_main.Controls.Clear();
                sv.Dock = DockStyle.Fill;
                pnl_main.Controls.Add(sv);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải Student Form: " + ex.Message);
            }
        }

        private void LoadClass()
        {
            try
            {
                UCQLLH lh = new UCQLLH();
                pnl_main.Controls.Clear();
                lh.Dock = DockStyle.Fill;
                pnl_main.Controls.Add(lh);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải Class Form: " + ex.Message);
            }
        }

        private void Logout()
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.Show();
        }
    }
}
