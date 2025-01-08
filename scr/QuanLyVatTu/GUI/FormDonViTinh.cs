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
    public partial class FormDonViTinh : Form
    {
        public FormDonViTinh()
        {
            InitializeComponent();
        }


        DonViTinhDAL _dal = new DonViTinhDAL();
        BindingSource _src = new BindingSource();


        private void LoadGridData()
        {
            _src.DataSource = _dal.Get();
            _src.ResetBindings(true);
        }

        private void FormDonViTinh_Load(object sender, EventArgs e)
        {
            gridData.AutoGenerateColumns = true;
            gridData.AllowUserToAddRows = false;
            gridData.ReadOnly = true;
            gridData.DataSource = _src;
            LoadGridData();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            HienThi(new DonViTinhDTO()
            {
                MaDVT = _dal.NewId("D"),
                TenDVT = ""
            });
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaDVT.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã đơn vị không được ");
                return;
            }

            var dm = _dal.Get(txtMaDVT.Text);

            if (dm != null)
            {
                HopThoai.BaoLoi("Mã đơn vị đã tồn tại !");
                return;
            }

            if (txtTenDVT.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên đơn vị không được để trống !");
                return;
            }

            DonViTinhDTO obj = new DonViTinhDTO();
            obj.MaDVT = txtMaDVT.Text;
            obj.TenDVT = txtTenDVT.Text;

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Thêm mới đơn vị  thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Thêm mới đơn vị không thành công !");


        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaDVT.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã đơn vị không được ");
                return;
            }

            var dm = _dal.Get(txtMaDVT.Text);

            if (dm == null)
            {
                HopThoai.BaoLoi("Không tìm thấy đơn vị cần sửa !");
                return;
            }

            if (txtTenDVT.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên đơn vị không được để trống !");
                return;
            }

            DonViTinhDTO obj = new DonViTinhDTO();
            obj.MaDVT = txtMaDVT.Text;
            obj.TenDVT = txtTenDVT.Text;

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Sửa đơn vị thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Sửa đơn vị không thành công !");

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as DonViTinhDTO;

            if (obj == null)
                return;

            var answer = HopThoai.XacNhan("Bạn muốn xoá đơn vị được chọn ?");

            if (!answer)
                return;

            if (_dal.Delete(obj.MaDVT))
            {
                HopThoai.ThongBao("Xoá thông tin đơn vị thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Xoá thông tin đơn vị không thành công !");
        }

        private void gridData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as DonViTinhDTO;

            if (obj == null)
                return;

            HienThi(obj);
        }

        private void HienThi(DonViTinhDTO obj)
        {
            txtMaDVT.Text = obj.MaDVT;
            txtTenDVT.Text = obj.TenDVT;
        }
    }
}
