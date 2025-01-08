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
    public partial class FormLoaiVatTu : Form
    {
        public FormLoaiVatTu()
        {
            InitializeComponent();
        }


        LoaiVatTuDAL _dal = new LoaiVatTuDAL();
        NhomVatTuDAL _dalNhomVatTu = new NhomVatTuDAL();
        BindingSource _src = new BindingSource();


        private void LoadGridData()
        {
            _src.DataSource = _dal.Get();
            _src.ResetBindings(true);
        }

        private void FormLoaiVatTu_Load(object sender, EventArgs e)
        {
            gridData.AutoGenerateColumns = true;
            gridData.AllowUserToAddRows = false;
            gridData.ReadOnly = true;
            gridData.DataSource = _src;
            LoadComboNhomVatTu();
            LoadGridData();
        }

        private void LoadComboNhomVatTu()
        {
            cboMaNhom.DataSource = _dalNhomVatTu.Get();
            cboMaNhom.ValueMember = "MaNhom";
            cboMaNhom.DisplayMember = "TenNhom";
            if (cboMaNhom.Items.Count > 0)
            {
                cboMaNhom.SelectedIndex = 0;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            HienThi(new LoaiVatTuDTO()
            {
                MaLoai = _dal.NewId("L"),
                TenLoai = ""
            });
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaLoai.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã loại không được để trống");
                return;
            }

            var dm = _dal.Get(txtMaLoai.Text);

            if (dm != null)
            {
                HopThoai.BaoLoi("Mã loại đã tồn tại !");
                return;
            }

            if (txtTenLoai.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên loại không được để trống !");
                return;
            }

            if (cboMaNhom.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn nhóm vật tư !");
                return;
            }

            LoaiVatTuDTO obj = new LoaiVatTuDTO();
            obj.MaLoai = txtMaLoai.Text;
            obj.TenLoai = txtTenLoai.Text;
            obj.MaNhom = cboMaNhom.SelectedValue.ToString();

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Thêm mới loại vật tư thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Thêm mới loại vật tư không thành công !");


        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaLoai.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã loại không được để trống ");
                return;
            }

            var dm = _dal.Get(txtMaLoai.Text);

            if (dm == null)
            {
                HopThoai.BaoLoi("Không tìm thấy loại cần sửa !");
                return;
            }

            if (txtTenLoai.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên loại không được để trống !");
                return;
            }

            if (cboMaNhom.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn nhóm vật tư !");
                return;
            }

            LoaiVatTuDTO obj = new LoaiVatTuDTO();
            obj.MaLoai = txtMaLoai.Text;
            obj.TenLoai = txtTenLoai.Text;
            obj.MaNhom = cboMaNhom.SelectedValue.ToString();

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Sửa loại vật tư thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Sửa loại vật tư không thành công !");

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as LoaiVatTuDTO;

            if (obj == null)
                return;

            var answer = HopThoai.XacNhan("Bạn muốn xoá loại được chọn ?");

            if (!answer)
                return;

            if (_dal.Delete(obj.MaLoai))
            {
                HopThoai.ThongBao("Xoá thông tin loại thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Xoá thông tin loại không thành công !");
        }

        private void gridData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as LoaiVatTuDTO;

            if (obj == null)
                return;

            HienThi(obj);
        }

        private void HienThi(LoaiVatTuDTO obj)
        {
            txtMaLoai.Text = obj.MaLoai;
            txtTenLoai.Text = obj.TenLoai;

            if (!string.IsNullOrEmpty(obj.MaNhom))
                cboMaNhom.SelectedValue = obj.MaNhom;
            else
                cboMaNhom.SelectedIndex = 0;
        }
    }
}
