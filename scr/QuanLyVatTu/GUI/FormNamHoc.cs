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
    public partial class FormNamHoc : Form
    {
        public FormNamHoc()
        {
            InitializeComponent();
        }

        NamHocDAL _dal = new NamHocDAL();
        BindingSource _src = new BindingSource();


        private void LoadGridData()
        {
            _src.DataSource = _dal.Get();
            _src.ResetBindings(true);
        }

        private void FormNamHoc_Load(object sender, EventArgs e)
        {
            gridData.AutoGenerateColumns = true;
            gridData.AllowUserToAddRows = false;
            gridData.ReadOnly = true;
            gridData.DataSource = _src;
            LoadGridData();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            HienThi(new NamHocDTO()
            {
                MaNH = _dal.NewId("NH"),
                TenNH = ""
            });
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaNH.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã năm học không được ");
                return;
            }

            var dm = _dal.Get(txtMaNH.Text);

            if (dm != null)
            {
                HopThoai.BaoLoi("Mã năm học đã tồn tại !");
                return;
            }

            if (txtTenNH.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên năm học không được để trống !");
                return;
            }

            NamHocDTO obj = new NamHocDTO();
            obj.MaNH = txtMaNH.Text;
            obj.TenNH = txtTenNH.Text;

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Thêm mới năm học  thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Thêm mới năm học  không thành công !");


        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaNH.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã năm học không được ");
                return;
            }

            var dm = _dal.Get(txtMaNH.Text);

            if (dm == null)
            {
                HopThoai.BaoLoi("Không tìm thấy năm học cần sửa !");
                return;
            }

            if (txtTenNH.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên năm học không được để trống !");
                return;
            }

            NamHocDTO obj = new NamHocDTO();
            obj.MaNH = txtMaNH.Text;
            obj.TenNH = txtTenNH.Text;

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Sửa năm học  thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Sửa năm học  không thành công !");

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as NamHocDTO;

            if (obj == null)
                return;

            var answer = HopThoai.XacNhan("Bạn muốn xoá năm học được chọn ?");

            if (!answer)
                return;

            if (_dal.Delete(obj.MaNH))
            {
                HopThoai.ThongBao("Xoá thông tin năm học thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Xoá thông tin năm học không thành công !");
        }

        private void gridData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as NamHocDTO;

            if (obj == null)
                return;

            HienThi(obj);
        }

        private void HienThi(NamHocDTO obj)
        {
            txtMaNH.Text = obj.MaNH;
            txtTenNH.Text = obj.TenNH;
        }
    }
}
