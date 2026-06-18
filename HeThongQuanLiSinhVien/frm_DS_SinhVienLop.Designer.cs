namespace HeThongQuanLiSinhVien
{
    partial class frm_DS_SinhVienLop
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.dgvDSSV = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDSSV)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.BackColor = System.Drawing.Color.MidnightBlue;
            this.lblTieuDe.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTieuDe.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.lblTieuDe.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblTieuDe.Location = new System.Drawing.Point(0, 0);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(1000, 75);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "DANH SÁCH CHI TIẾT SỸ SỐ CỦA LỚP";
            this.lblTieuDe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvDSSV
            // 
            this.dgvDSSV.AllowUserToAddRows = false;
            this.dgvDSSV.AllowUserToDeleteRows = false;
            this.dgvDSSV.AllowUserToResizeColumns = false;
            this.dgvDSSV.AllowUserToResizeRows = false;
            this.dgvDSSV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDSSV.BackgroundColor = System.Drawing.Color.White;
            this.dgvDSSV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDSSV.ColumnHeadersHeight = 45;
            this.dgvDSSV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDSSV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDSSV.Location = new System.Drawing.Point(0, 75);
            this.dgvDSSV.Name = "dgvDSSV";
            this.dgvDSSV.ReadOnly = true;
            this.dgvDSSV.RowHeadersVisible = false;
            this.dgvDSSV.RowHeadersWidth = 51;
            this.dgvDSSV.RowTemplate.Height = 35;
            this.dgvDSSV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDSSV.Size = new System.Drawing.Size(1000, 525);
            this.dgvDSSV.TabIndex = 1;
            // 
            // frm_DS_SinhVienLop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.dgvDSSV);
            this.Controls.Add(this.lblTieuDe);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_DS_SinhVienLop";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cơ sở CSDL  | Module Liên Bảng";
            this.Load += new System.EventHandler(this.frm_DS_SinhVienLop_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDSSV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.DataGridView dgvDSSV;
    }
}