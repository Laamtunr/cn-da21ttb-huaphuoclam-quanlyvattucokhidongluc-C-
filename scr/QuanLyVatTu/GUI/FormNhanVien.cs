using GUI.Helpers;
using QuanLyMuonTraVatTu.DAL;
using QuanLyMuonTraVatTu.DTO;
using QuanLyMuonTraVatTu.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyMuonTraVatTu.GUI
{
    public partial class FormNhanVien : Form
    {
        public FormNhanVien()
        {
            InitializeComponent();
        }

        NhanVienDAL _dal = new NhanVienDAL();
        BindingSource _src = new BindingSource();


        private void LoadGridData()
        {
            _src.DataSource = _dal.Get();
            _src.ResetBindings(true);
        }

        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            gridData.AutoGenerateColumns = true;
            gridData.AllowUserToAddRows = false;
            gridData.ReadOnly = true;
            gridData.DataSource = _src;
            LoadGridData();
            btnLamMoi_Click(sender, e);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            HienThi(new NhanVienDTO()
            {
                MaNV = _dal.NewId("NV"),
                HoTen = "",
                GioiTinh = "Nam",
                SDT = "",
                Email = "",
                DiaChi = "",
                NgaySinh = DateTime.Now.AddYears(-16)
            });
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã nhân viên không được ");
                return;
            }

            var dm = _dal.Get(txtMaNV.Text);

            if (dm != null)
            {
                HopThoai.BaoLoi("Mã nhân viên đã tồn tại !");
                return;
            }

            if (txtHoTen.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên nhân viên không được để trống !");
                return;
            }

            if (txtSDT.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Số điện thoại không được để trống !");
                return;
            } 

            if (txtEmail.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Email không được để trống !");
                return;
            } 

            if (txtDiaChi.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Địa chỉ không được để trống !");
                return;
            } 

            if (DateTime.Now.Year - dtNgaySinh.Value.Year < 16)
            {
                HopThoai.BaoLoi("Chỉ nhận nhân viên lớn hơn 16 tuổi !");
                return;
            } 


            NhanVienDTO obj = new NhanVienDTO();
            obj.MaNV = txtMaNV.Text;
            obj.HoTen = txtHoTen.Text;

            if (rdoNam.Checked)
                obj.GioiTinh = "Nam";
            else
                obj.GioiTinh = "Nữ";

            obj.SDT = txtSDT.Text;
            obj.Email = txtEmail.Text;
            obj.DiaChi = txtDiaChi.Text;
            obj.NgaySinh = dtNgaySinh.Value;

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Thêm mới nhân viên  thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Thêm mới nhân viên không thành công !");


        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã nhân viên không được ");
                return;
            }

            var dm = _dal.Get(txtMaNV.Text);

            if (dm == null)
            {
                HopThoai.BaoLoi("Không tìm thấy nhân viên cần sửa !");
                return;
            }

            if (txtHoTen.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên nhân viên không được để trống !");
                return;
            }


            if (txtSDT.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Số điện thoại không được để trống !");
                return;
            }

            if (txtEmail.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Email không được để trống !");
                return;
            }

            if (txtDiaChi.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Địa chỉ không được để trống !");
                return;
            }

            if (DateTime.Now.Year - dtNgaySinh.Value.Year < 16)
            {
                HopThoai.BaoLoi("Chỉ nhận nhân viên lớn hơn 16 tuổi !");
                return;
            }


            NhanVienDTO obj = new NhanVienDTO();
            obj.MaNV = txtMaNV.Text;
            obj.HoTen = txtHoTen.Text;

            if (rdoNam.Checked)
                obj.GioiTinh = "Nam";
            else
                obj.GioiTinh = "Nữ";

            obj.SDT = txtSDT.Text;
            obj.Email = txtEmail.Text;
            obj.DiaChi = txtDiaChi.Text;
            obj.NgaySinh = dtNgaySinh.Value;

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Sửa nhân viên thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Sửa nhân viên không thành công !");

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as NhanVienDTO;

            if (obj == null)
                return;

            var answer = HopThoai.XacNhan("Bạn muốn xoá nhân viên được chọn ?");

            if (!answer)
                return;

            if (_dal.Delete(obj.MaNV))
            {
                HopThoai.ThongBao("Xoá thông tin nhân viên thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Xoá thông tin nhân viên không thành công !");
        }

        private void gridData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as NhanVienDTO;

            if (obj == null)
                return;

            HienThi(obj);
        }

        private void HienThi(NhanVienDTO obj)
        {
            txtMaNV.Text = obj.MaNV;
            txtHoTen.Text = obj.HoTen;
            
            if (obj.GioiTinh == "Nam")
            {
                rdoNam.Checked = true;
                rdoNu.Checked = false;
            }
            else
            {
                rdoNu.Checked = true;
                rdoNam.Checked = false;
            }

            txtSDT.Text = obj.SDT;
            txtEmail.Text = obj.Email;
            txtDiaChi.Text = obj.DiaChi;
            dtNgaySinh.Value = obj.NgaySinh;
        }
    }
}
