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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyMuonTraVatTu.GUI
{
    public partial class FormDanhMucVatTu : Form
    {
        public FormDanhMucVatTu()
        {
            InitializeComponent();
        }

        DanhMucVatTuDAL _dal = new DanhMucVatTuDAL();
        BindingSource _src = new BindingSource();


        private void LoadGridData()
        {
            _src.DataSource = _dal.Get();
            _src.ResetBindings(true);
        }

        private void FormDanhMucVatTu_Load(object sender, EventArgs e)
        {
            gridData.AutoGenerateColumns = true;
            gridData.AllowUserToAddRows = false;
            gridData.ReadOnly = true;
            gridData.DataSource = _src;
            LoadGridData();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            HienThi(new DanhMucVatTuDTO()
            {
                MaDM = _dal.NewId("DM"),
                TenDM = ""
            });
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaDM.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã danh mục không được ");
                return;
            }

            var dm = _dal.Get(txtMaDM.Text);

            if (dm != null)
            {
                HopThoai.BaoLoi("Mã danh mục đã tồn tại !");
                return;
            } 

            if (txtTenDM.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên danh mục không được để trống !");
                return;
            }

            DanhMucVatTuDTO obj = new DanhMucVatTuDTO();
            obj.MaDM = txtMaDM.Text;
            obj.TenDM = txtTenDM.Text;

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Thêm mới danh mục vật tư thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Thêm mới danh mục vật tư không thành công !");


        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaDM.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã danh mục không được ");
                return;
            }

            var dm = _dal.Get(txtMaDM.Text);

            if (dm == null)
            {
                HopThoai.BaoLoi("Không tìm thấy danh mục cần sửa !");
                return;
            }

            if (txtTenDM.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên danh mục không được để trống !");
                return;
            }

            DanhMucVatTuDTO obj = new DanhMucVatTuDTO();
            obj.MaDM = txtMaDM.Text;
            obj.TenDM = txtTenDM.Text;

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Sửa danh mục vật tư thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Sửa danh mục vật tư không thành công !");

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as DanhMucVatTuDTO;

            if (obj == null)
                return;

            var answer = HopThoai.XacNhan("Bạn muốn xoá danh mục được chọn ?");

            if (!answer)
                return;

            if (_dal.Delete(obj.MaDM))
            {
                HopThoai.ThongBao("Xoá thông tin danh mục thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Xoá thông tin danh mục không thành công !");
        }

        private void gridData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as DanhMucVatTuDTO;

            if (obj == null)
                return;

            HienThi(obj);
        }

        private void HienThi(DanhMucVatTuDTO obj)
        {
            txtMaDM.Text = obj.MaDM;
            txtTenDM.Text = obj.TenDM;
        }
    }
}
