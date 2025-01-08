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
using TheArtOfDevHtmlRenderer.Adapters;

namespace QuanLyMuonTraVatTu.GUI
{
    public partial class FormVatTu : Form
    {
        public FormVatTu()
        {
            InitializeComponent();
        }

        VatTuDAL _dal = new VatTuDAL();
        LoaiVatTuDAL _dalLoaiVatTu = new LoaiVatTuDAL();
        DonViTinhDAL _dalDonViTinh = new DonViTinhDAL();

        BindingSource _src = new BindingSource();


        private void LoadGridData()
        {
            _src.DataSource = _dal.Get();
            _src.ResetBindings(true);
        }

        private void FormVatTu_Load(object sender, EventArgs e)
        {
            gridData.AutoGenerateColumns = true;
            gridData.AllowUserToAddRows = false;
            gridData.ReadOnly = true;
            gridData.DataSource = _src;
            LoadComboLoaiVatTu();
            LoadComboDonViTinh();
            LoadGridData();
            btnLamMoi_Click(sender, e);
        }

        private void LoadComboDonViTinh()
        {
            cboMaDVT.DataSource = _dalDonViTinh.Get();
            cboMaDVT.ValueMember = "MaDVT";
            cboMaDVT.DisplayMember = "TenDVT";
            if (cboMaDVT.Items.Count > 0)
                cboMaDVT.SelectedIndex = 0;
        }

        private void LoadComboLoaiVatTu()
        {
            cboMaLoai.DataSource = _dalLoaiVatTu.Get();
            cboMaLoai.ValueMember = "MaLoai";
            cboMaLoai.DisplayMember = "TenLoai";
            if (cboMaLoai.Items.Count > 0)
                cboMaLoai.SelectedIndex = 0;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            HienThi(new VatTuDTO()
            {
                MaVT = _dal.NewId("VT"),
                TenVT = "",
                MoTa = "",
                TonKho = 0,
                TonKhoToiThieu = 0,
                TinhTrang = "-- CHỌN --"
            });
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaVT.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã vật tư không được ");
                return;
            }

            var dm = _dal.Get(txtMaVT.Text);

            if (dm != null)
            {
                HopThoai.BaoLoi("Mã vật tư đã tồn tại !");
                return;
            }

            if (txtTenVT.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên vật tư không được để trống !");
                return;
            }

            if (cboMaLoai.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Hãy chọn loại vật tư !");
                return;
            } 

            if (cboMaDVT.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Hãy chọn đơn vị tính !");
                return;
            } 

            if (txtTonKho.Value < 0)
            {
                HopThoai.BaoLoi("Giá trị tồn kho không được nhỏ hơn 0 !");
                return;
            } 

            if (txtTonKhoToiThieu.Value < 0)
            {
                HopThoai.BaoLoi("Giá trị tồn kho tối thiểu không được nhỏ hơn 0 !");
                return;
            } 



            VatTuDTO obj = new VatTuDTO();
            obj.MaVT = txtMaVT.Text;
            obj.TenVT = txtTenVT.Text;
            obj.MaLoai = cboMaLoai.SelectedValue.ToString();
            obj.MaDVT = cboMaDVT.SelectedValue.ToString();
            obj.MoTa = txtMoTa.Text;
            obj.TonKho = txtTonKho.Value;
            obj.TonKhoToiThieu = txtTonKhoToiThieu.Value;
            if (cboTinhTrang.SelectedIndex > 0)
                obj.TinhTrang = cboTinhTrang.SelectedItem.ToString();

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Thêm mới vật tư  thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Thêm mới vật tư không thành công !");


        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaVT.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã vật tư không được ");
                return;
            }

            var dm = _dal.Get(txtMaVT.Text);

            if (dm == null)
            {
                HopThoai.BaoLoi("Không tìm thấy vật tư cần sửa !");
                return;
            }

            if (txtTenVT.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên vật tư không được để trống !");
                return;
            }

            if (cboMaLoai.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Hãy chọn loại vật tư !");
                return;
            }

            if (cboMaDVT.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Hãy chọn đơn vị tính !");
                return;
            }

            if (txtTonKho.Value < 0)
            {
                HopThoai.BaoLoi("Giá trị tồn kho không được nhỏ hơn 0 !");
                return;
            }

            if (txtTonKhoToiThieu.Value < 0)
            {
                HopThoai.BaoLoi("Giá trị tồn kho tối thiểu không được nhỏ hơn 0 !");
                return;
            }



            VatTuDTO obj = new VatTuDTO();
            obj.MaVT = txtMaVT.Text;
            obj.TenVT = txtTenVT.Text;
            obj.MaLoai = cboMaLoai.SelectedValue.ToString();
            obj.MaDVT = cboMaDVT.SelectedValue.ToString();
            obj.MoTa = txtMoTa.Text;
            obj.TonKho = txtTonKho.Value;
            obj.TonKhoToiThieu = txtTonKhoToiThieu.Value;
            if (cboTinhTrang.SelectedIndex > 0)
                obj.TinhTrang = cboTinhTrang.SelectedItem.ToString();


            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Sửa vật tư thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Sửa vật tư không thành công !");

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as VatTuDTO;

            if (obj == null)
                return;

            var answer = HopThoai.XacNhan("Bạn muốn xoá vật tư được chọn ?");

            if (!answer)
                return;

            if (_dal.Delete(obj.MaVT))
            {
                HopThoai.ThongBao("Xoá thông tin vật tư thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Xoá thông tin vật tư không thành công !");
        }

        private void gridData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as VatTuDTO;

            if (obj == null)
                return;

            HienThi(obj);
        }

        private void HienThi(VatTuDTO obj)
        {
            txtMaVT.Text = obj.MaVT;
            txtTenVT.Text = obj.TenVT;
            
            if (!string.IsNullOrEmpty(obj.MaLoai))
                cboMaLoai.SelectedValue = obj.MaLoai;
            else
                cboMaLoai.SelectedIndex = 0;
            
            if (!string.IsNullOrEmpty(obj.MaDVT))
                cboMaDVT.SelectedValue = obj.MaDVT;
            else
                cboMaDVT.SelectedIndex = 0;

            txtMoTa.Text = obj.MoTa;
            txtTonKho.Value = obj.TonKho ?? 0;
            txtTonKhoToiThieu.Value = obj.TonKhoToiThieu ?? 0;

            if (!string.IsNullOrEmpty(obj.TinhTrang))
                cboTinhTrang.SelectedItem = obj.TinhTrang;
            else
                cboTinhTrang.SelectedIndex = 0;

        }
    }
}
