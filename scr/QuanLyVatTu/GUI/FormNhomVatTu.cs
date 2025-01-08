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
    public partial class FormNhomVatTu : Form
    {
        public FormNhomVatTu()
        {
            InitializeComponent();
        }


        NhomVatTuDAL _dal = new NhomVatTuDAL();
        DanhMucVatTuDAL _dalDanhMucVatTu = new DanhMucVatTuDAL();
        BindingSource _src = new BindingSource();


        private void LoadGridData()
        {
            _src.DataSource = _dal.Get();
            _src.ResetBindings(true);
        }

        private void FormNhomVatTu_Load(object sender, EventArgs e)
        {
            gridData.AutoGenerateColumns = true;
            gridData.AllowUserToAddRows = false;
            gridData.ReadOnly = true;
            gridData.DataSource = _src;
            LoadComboDanhMucVatTu();
            LoadGridData();
        }

        private void LoadComboDanhMucVatTu()
        {
            cboMaDM.DataSource = _dalDanhMucVatTu.Get();
            cboMaDM.ValueMember = "MaDM";
            cboMaDM.DisplayMember = "TenDM";
            if (cboMaDM.Items.Count > 0)
            {
                cboMaDM.SelectedIndex = 0;
            } 
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            HienThi(new NhomVatTuDTO()
            {
                MaNhom = _dal.NewId("N"),
                TenNhom = ""
            });
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaNhom.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã nhóm không được để trống ! ");
                return;
            }

            var dm = _dal.Get(txtMaNhom.Text);

            if (dm != null)
            {
                HopThoai.BaoLoi("Mã nhóm đã tồn tại !");
                return;
            }

            if (txtTenNhom.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên nhóm không được để trống !");
                return;
            }

            if (cboMaDM.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn danh mục vật tư !");
                return;
            } 

            NhomVatTuDTO obj = new NhomVatTuDTO();
            obj.MaNhom = txtMaNhom.Text;
            obj.TenNhom = txtTenNhom.Text;
            obj.MaDM = cboMaDM.SelectedValue.ToString();

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Thêm mới nhóm vật tư thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Thêm mới nhóm vật tư không thành công !");


        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaNhom.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã nhóm không được để trống !");
                return;
            }

            var dm = _dal.Get(txtMaNhom.Text);

            if (dm == null)
            {
                HopThoai.BaoLoi("Không tìm thấy nhóm cần sửa !");
                return;
            }

            if (txtTenNhom.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên nhóm không được để trống !");
                return;
            }

            if (cboMaDM.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn danh mục vật tư !");
                return;
            } 

            NhomVatTuDTO obj = new NhomVatTuDTO();
            obj.MaNhom = txtMaNhom.Text;
            obj.TenNhom = txtTenNhom.Text;
            obj.MaDM = cboMaDM.SelectedValue.ToString();

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Sửa nhóm vật tư thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Sửa nhóm vật tư không thành công !");

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as NhomVatTuDTO;

            if (obj == null)
                return;

            var answer = HopThoai.XacNhan("Bạn muốn xoá nhóm được chọn ?");

            if (!answer)
                return;

            if (_dal.Delete(obj.MaNhom))
            {
                HopThoai.ThongBao("Xoá thông tin nhóm thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Xoá thông tin nhóm không thành công !");
        }

        private void gridData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as NhomVatTuDTO;

            if (obj == null)
                return;

            HienThi(obj);
        }

        private void HienThi(NhomVatTuDTO obj)
        {
            txtMaNhom.Text = obj.MaNhom;
            txtTenNhom.Text = obj.TenNhom;

            if (!string.IsNullOrEmpty(obj.MaDM))
                cboMaDM.SelectedValue = obj.MaDM;
            else
                cboMaDM.SelectedIndex = 0;
        }
    }
}
