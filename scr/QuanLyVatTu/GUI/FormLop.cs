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
    public partial class FormLop : Form
    {
        public FormLop()
        {
            InitializeComponent();
        }

        LopDAL _dal = new LopDAL();
        BindingSource _src = new BindingSource();


        private void LoadGridData()
        {
            _src.DataSource = _dal.Get();
            _src.ResetBindings(true);
        }

        private void FormLop_Load(object sender, EventArgs e)
        {
            gridData.AutoGenerateColumns = true;
            gridData.AllowUserToAddRows = false;
            gridData.ReadOnly = true;
            gridData.DataSource = _src;
            LoadGridData();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            HienThi(new LopDTO()
            {
                MaLop = _dal.NewId("L"),
                TenLop = ""
            });
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaLop.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã lớp không được ");
                return;
            }

            var dm = _dal.Get(txtMaLop.Text);

            if (dm != null)
            {
                HopThoai.BaoLoi("Mã lớp đã tồn tại !");
                return;
            }

            if (txtTenLop.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên lớp không được để trống !");
                return;
            }

            LopDTO obj = new LopDTO();
            obj.MaLop = txtMaLop.Text;
            obj.TenLop = txtTenLop.Text;

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Thêm mới lớp  thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Thêm mới lớp  không thành công !");


        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaLop.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã lớp không được ");
                return;
            }

            var dm = _dal.Get(txtMaLop.Text);

            if (dm == null)
            {
                HopThoai.BaoLoi("Không tìm thấy lớp cần sửa !");
                return;
            }

            if (txtTenLop.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên lớp không được để trống !");
                return;
            }

            LopDTO obj = new LopDTO();
            obj.MaLop = txtMaLop.Text;
            obj.TenLop = txtTenLop.Text;

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Sửa lớp  thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Sửa lớp  không thành công !");

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as LopDTO;

            if (obj == null)
                return;

            var answer = HopThoai.XacNhan("Bạn muốn xoá lớp được chọn ?");

            if (!answer)
                return;

            if (_dal.Delete(obj.MaLop))
            {
                HopThoai.ThongBao("Xoá thông tin lớp thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Xoá thông tin lớp không thành công !");
        }

        private void gridData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as LopDTO;

            if (obj == null)
                return;

            HienThi(obj);
        }

        private void HienThi(LopDTO obj)
        {
            txtMaLop.Text = obj.MaLop;
            txtTenLop.Text = obj.TenLop;
        }
    }
}
