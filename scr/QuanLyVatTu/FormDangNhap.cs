using QuanLyMuonTraVatTu.DAL;
using QuanLyMuonTraVatTu.DTO;
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
    public partial class FormDangNhap : Form
    {
        public FormDangNhap()
        {
            InitializeComponent();
        }

        TaiKhoanDAL _taiKhoanDaO = new TaiKhoanDAL();
        private void btnThoat_Click(object sender, EventArgs e)
        {
            // Hỏi người dùng có muốn thoát ứng dụng
            // nếu người dùng nhấn chọn yes thì đóng ứng dụng

            var kq = MessageBox.Show("Bạn muốn thoát ứng dụng ?", "Thoát"
                , MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (kq == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            // Kiểm tra người dùng đã nhập đầy đủ thông tin đăng nhập
            if (string.IsNullOrEmpty(txtTaiKhoan.Text))
            {
                MessageBox.Show("Tài khoản không được để trống !", "Thông báo");
                return;
            }

            if (string.IsNullOrEmpty(txtMatKhau.Text))
            {
                MessageBox.Show("Mật khẩu không được để trống !", "Thông báo");
                return;
            }

            // Lấy thông tin tài khoản theo mã tài khoản được nhập
            TaiKhoanDTO obj = _taiKhoanDaO.Get(txtTaiKhoan.Text);

            // Nếu không lấy được thì hiển thị thông báo lỗi và kết thúc
            if (obj == null)
            {
                MessageBox.Show("Đăng nhập không thành công !");
                return;
            }

            // Nếu lấy được nhưng mật khẩu không đúng thì cũng hiện thông
            // báo và kết thúc
            if (obj.MatKhau != txtMatKhau.Text)
            {
                MessageBox.Show("Đăng nhập không thành công !");
                return;
            }

            // Ngược lại ẩn form đăng nhập và hiển thị form chính
            // với tham số truyền vào form chính là đối tượng người dùng
            // đăng nhập hiện tại
            this.Hide();
            FormMain f = new FormMain(obj);
            // Hiển thị form chính ra màn hình
            f.ShowDialog();
            this.Show();
        }

        private void FormDangNhap_Load(object sender, EventArgs e)
        {
            // Ẩn ký tự ở textbox mật khẩu bằng ký tự thay thế
            // dấu sao
            txtMatKhau.PasswordChar = '*';
        }
    }
}
