using Guna.UI2.WinForms;
using QuanLyMuonTraVatTu.DTO;
using QuanLyMuonTraVatTu.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyMuonTraVatTu
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        public FormMain(TaiKhoanDTO user)
            : this()
        {
            User = user;
        }

        public static TaiKhoanDTO User { get; set; }


        public void LoadForm(Panel content, Form frm)
        {
            if (frm == null)
                return;

            foreach (Control control in content.Controls)
                content.Controls.Remove(control);

            lblTitle.Text = frm.Text;
            frm.TopLevel = false;
            content.Controls.Add(frm);
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            frm.BackColor = Color.FromArgb(210, 233, 255);
            frm.Show();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            PhanQuyen();
            LoadForm(pnlMain, new FormVatTu());
        }

        private void PhanQuyen()
        {
            btnDanhMucVatTu.Enabled = false;
            btnTonKho.Enabled = false;
            btnNhapXuatVatTu.Enabled = false;
            btnLopHKNamHoc.Enabled = false;
            btnLichSuSuDung.Enabled = false;
            btnNguoiDung.Enabled = false;
            btnThongKeBaoCao.Enabled = false;
            btnTimKiem.Enabled = false;

            if (User.Quyen == "ADMIN")
            {
                btnDanhMucVatTu.Enabled = true;
                btnTonKho.Enabled = true;
                btnNhapXuatVatTu.Enabled = true;
                btnLopHKNamHoc.Enabled = true;
                btnLichSuSuDung.Enabled = true;
                btnNguoiDung.Enabled = true;
                btnThongKeBaoCao.Enabled = true;
                btnTimKiem.Enabled = true;
            }

            if (User.Quyen == "GIANGVIEN")
            {
                btnDanhMucVatTu.Enabled = false;
                btnTonKho.Enabled = false;
                btnNhapXuatVatTu.Enabled = true;
                btnLopHKNamHoc.Enabled = false;
                btnLichSuSuDung.Enabled = true;
                btnNguoiDung.Enabled = false;
                btnThongKeBaoCao.Enabled = true;
                btnTimKiem.Enabled = true;
            }

            if (User.Quyen == "SINHVIEN")
            {
                btnDanhMucVatTu.Enabled = false;
                btnTonKho.Enabled = false;
                btnNhapXuatVatTu.Enabled = false;
                btnLopHKNamHoc.Enabled = false;
                btnLichSuSuDung.Enabled = false;
                btnNguoiDung.Enabled = false;
                btnThongKeBaoCao.Enabled = false;
                btnTimKiem.Enabled = false;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuDSVatTu_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormDanhMucVatTu());
        }

        private void mnuNhomVatTu_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormNhomVatTu());
        }

        private void mnuLoaiVatTu_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormLoaiVatTu());
        }

        private void mnuDVT_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormDonViTinh());
        }

        private void mnuVatTu_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormVatTu());
        }

        private void mnuPhieuNhap_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormDSPhieuNhap());
        }

        private void mnuPhieuMuon_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormDSPhieuMuon());
        }

        private void mnuPhieuTra_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormDSPhieuTra());
        }

        private void mnuNamHoc_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormNamHoc());
        }

        private void mnuHocKy_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormHocKy());
        }

        private void mnuLop_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormLop());
        }

        private void mnuNhanVien_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormNhanVien());
        }

        private void mnuTaiKhoan_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormTaiKhoan());
        }

        private void btnDanhMucVatTu_Click(object sender, EventArgs e)
        {
            Guna2Button btnSender = (Guna2Button)sender;
            mnuDanhMuc.Show(btnSender.Left + btnSender.Width + this.Left, btnSender.Top + this.Top);
        }

        private void btnTonKho_Click(object sender, EventArgs e)
        {
            //Guna2Button btnSender = (Guna2Button)sender;
            //mnuTonKho.Show(btnSender.Left + btnSender.Width + this.Left, btnSender.Top + this.Top);
            LoadForm(pnlMain, new FormNhapXuatTon());
        }

        private void btnNhapXuatVatTu_Click(object sender, EventArgs e)
        {
            Guna2Button btnSender = (Guna2Button)sender;
            mnuNhapXuatVatTu.Show(btnSender.Left + btnSender.Width + this.Left, btnSender.Top + this.Top);
        }

        private void btnLopHKNamHoc_Click(object sender, EventArgs e)
        {
            Guna2Button btnSender = (Guna2Button)sender;
            mnuLopHKNamHoc.Show(btnSender.Left + btnSender.Width + this.Left, btnSender.Top + this.Top);
        }

        private void btnLichSuSuDung_Click(object sender, EventArgs e)
        {
            //Guna2Button btnSender = (Guna2Button)sender;
            //mnuLichSuSuDung.Show(btnSender.Left + btnSender.Width + this.Left, btnSender.Top + this.Top);
            LoadForm(pnlMain, new FormLichSuSuDung());
        }

        private void btnNguoiDung_Click(object sender, EventArgs e)
        {
            Guna2Button btnSender = (Guna2Button)sender;
            mnuNguoiDung.Show(btnSender.Left + btnSender.Width + this.Left, btnSender.Top + this.Top);
        }

        private void btnThongKeBaoCao_Click(object sender, EventArgs e)
        {
            Guna2Button btnSender = (Guna2Button)sender;
            mnuThongKeBaoCao.Show(btnSender.Left + btnSender.Width + this.Left, btnSender.Top + this.Top);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormTimKiem());
        }

        private void mnuBaoCaoNhapXuatTon_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormBaoCaoNhapXuatTon());
        }

        private void mnuBaoCaoVatTuChuaTra_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormBaoCaoVatTuChuaTra());
        }

        private void báoCáoSốLầnMượnVậtTưToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm(pnlMain, new FormBaoCaoSoLanMuonVatTu());
        }
    }
}
