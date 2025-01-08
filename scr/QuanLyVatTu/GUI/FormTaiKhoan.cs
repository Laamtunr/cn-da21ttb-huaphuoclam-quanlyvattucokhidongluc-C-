using GUI.Helpers;
using QuanLyMuonTraVatTu.DAL;
using QuanLyMuonTraVatTu.DTO;
using QuanLyMuonTraVatTu.Helpers;
using QuanLyMuonTraVatTu.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyMuonTraVatTu.GUI
{
    public partial class FormTaiKhoan : Form
    {
        public FormTaiKhoan()
        {
            InitializeComponent();
        }

        NhanVienDAL _nhanVienDAL = new NhanVienDAL();
        TaiKhoanDAL _taiKhoanDAL = new TaiKhoanDAL();
        BindingSource _src = new BindingSource();
        private void FormTaiKhoan_Load(object sender, EventArgs e)
        {
            gridData.DataSource = _src;
            LoadComboQuyen();
            LoadComboNhanVien();
            LoadGrid();
        }

        private void LoadGrid()
        {
            var dsTK = _taiKhoanDAL.Get();

            if (FormMain.User.Quyen != "ADMIN")
            {
                dsTK = dsTK.Where(x => x.MaTK == FormMain.User.MaTK)
                    .ToList();
            }

            _src.DataSource = dsTK;
            _src.ResetBindings(true);
        }

        private void LoadComboNhanVien()
        {
            var ds = _nhanVienDAL.Get();
            ds.Insert(0, new DTO.NhanVienDTO()
            {
                MaNV = "",
                HoTen = "-- CHỌN --"
            });
            cboMaNV.DataSource = ds;
            cboMaNV.ValueMember = "MaNV";
            cboMaNV.DisplayMember = "HoTen";
            if (cboMaNV.Items.Count > 0)
                cboMaNV.SelectedIndex = 0;
        }

        private void LoadComboQuyen()
        {
            cboQuyen.DataSource = QuyenVM.Get();
            cboQuyen.ValueMember = "MaQuyen";
            cboQuyen.DisplayMember = "TenQuyen";
            if (cboQuyen.Items.Count > 0)
                cboQuyen.SelectedIndex = 0;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaTK.Text = "";
            txtMatKhau.Text = "";
            cboQuyen.SelectedIndex = 0;
            cboMaNV.SelectedIndex = 0;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaTK.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã tài khoản không được để trống !");
                return;
            }

            var tk = _taiKhoanDAL.Get(txtMaTK.Text);

            if (tk != null)
            {
                HopThoai.BaoLoi("Mã tài khoản đã tồn tại !");
                return;
            } 

            if (txtMatKhau.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mật khẩu không được để trống !");
                return;
            } 

            if (cboQuyen.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn quyền !");
                return;
            }

            TaiKhoanDTO obj = new TaiKhoanDTO();
            obj.MaTK = txtMaTK.Text;
            obj.MatKhau = txtMatKhau.Text;
            obj.Quyen = cboQuyen.SelectedValue.ToString();

            if (cboMaNV.SelectedIndex > 0)
            {
                obj.MaNV = cboMaNV.SelectedValue.ToString();
            } 

            if (_taiKhoanDAL.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Thêm mới tài khoản thành công !");
                LoadGrid();
                return;
            }

            HopThoai.BaoLoi("Thêm mới tài khoản không thành công !");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaTK.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã tài khoản không được để trống !");
                return;
            }

            var tk = _taiKhoanDAL.Get(txtMaTK.Text);

            if (tk == null)
            {
                HopThoai.BaoLoi("Không tìm thấy tài khoản cần sửa !");
                return;
            }

            if (txtMatKhau.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mật khẩu không được để trống !");
                return;
            }

            if (cboQuyen.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn quyền !");
                return;
            }

            TaiKhoanDTO obj = new TaiKhoanDTO();
            obj.MaTK = txtMaTK.Text;
            obj.MatKhau = txtMatKhau.Text;
            obj.Quyen = cboQuyen.SelectedValue.ToString();

            if (cboMaNV.SelectedIndex > 0)
            {
                obj.MaNV = cboMaNV.SelectedValue.ToString();
            }

            if (_taiKhoanDAL.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Sửa thông tin tài khoản thành công !");
                LoadGrid();
                return;
            }

            HopThoai.BaoLoi("Sửa thông tin tài khoản không thành công !");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
                return;

            var obj = gridData.CurrentRow.DataBoundItem as TaiKhoanDTO;

            if (obj == null)
                return;

            if (FormMain.User.Quyen != "ADMIN")
            {
                HopThoai.BaoLoi("Bạn không có quyền xoá tài khoản !");
                return;
            }

            if (obj.MaTK == FormMain.User.MaTK)
            {
                HopThoai.BaoLoi("Không thể xoá tài khoản đang đăng nhập !");
                return;
            } 

            var answer = HopThoai.XacNhan("Bạn muốn xoá tài khoản được chọn ?");

            if (answer)
            {
                if (_taiKhoanDAL.Delete(obj.MaTK))
                {
                    HopThoai.ThongBao("Xoá thông tin tài khoản thành công !");
                    LoadGrid();
                    return;
                }

                HopThoai.BaoLoi("Xoá thông tin tài khoản không thành công !");
            } 

        }

        private void gridData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
                return;

            TaiKhoanDTO obj = gridData.CurrentRow.DataBoundItem as TaiKhoanDTO;
            if (obj == null) return;
            HienThi(obj);
        }

        private void HienThi(TaiKhoanDTO obj)
        {
            txtMaTK.Text = obj.MaTK;
            txtMatKhau.Text = obj.MatKhau;
            if (!string.IsNullOrEmpty(obj.Quyen))
                cboQuyen.SelectedItem = obj.Quyen;
            else
                cboQuyen.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(obj.MaNV))
                cboMaNV.SelectedValue = obj.MaNV;
            else
                cboMaNV.SelectedIndex = 0;
        }
    }
}
