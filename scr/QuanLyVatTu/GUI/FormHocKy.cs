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
    public partial class FormHocKy : Form
    {
        public FormHocKy()
        {
            InitializeComponent();
        }


        HocKyDAL _dal = new HocKyDAL();
        NamHocDAL _dalNamHoc = new NamHocDAL();
        BindingSource _src = new BindingSource();


        private void LoadGridData()
        {
            _src.DataSource = _dal.Get();
            _src.ResetBindings(true);
        }

        private void FormHocKy_Load(object sender, EventArgs e)
        {
            gridData.AutoGenerateColumns = true;
            gridData.AllowUserToAddRows = false;
            gridData.ReadOnly = true;
            gridData.DataSource = _src;
            LoadComboNamHoc();
            LoadGridData();
        }

        private void LoadComboNamHoc()
        {
            cboMaNH.DataSource = _dalNamHoc.Get();
            cboMaNH.ValueMember = "MaNH";
            cboMaNH.DisplayMember = "TenNH";
            if (cboMaNH.Items.Count > 0)
            {
                cboMaNH.SelectedIndex = 0;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            HienThi(new HocKyDTO()
            {
                MaHK = _dal.NewId("HK"),
                TenHK = ""
            });
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaHK.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã học kỳ không được ");
                return;
            }

            var dm = _dal.Get(txtMaHK.Text);

            if (dm != null)
            {
                HopThoai.BaoLoi("Mã học kỳ đã tồn tại !");
                return;
            }

            if (txtTenHK.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên học kỳ không được để trống !");
                return;
            }

            if (cboMaNH.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn năm học !");
                return;
            }

            HocKyDTO obj = new HocKyDTO();
            obj.MaHK = txtMaHK.Text;
            obj.TenHK = txtTenHK.Text;
            obj.MaNH = cboMaNH.SelectedValue.ToString();

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Thêm mới học kỳ thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Thêm mới học kỳ không thành công !");


        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaHK.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã học kỳ không được ");
                return;
            }

            var dm = _dal.Get(txtMaHK.Text);

            if (dm == null)
            {
                HopThoai.BaoLoi("Không tìm thấy học kỳ cần sửa !");
                return;
            }

            if (txtTenHK.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Tên học kỳ không được để trống !");
                return;
            }

            if (cboMaNH.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn năm học !");
                return;
            }

            HocKyDTO obj = new HocKyDTO();
            obj.MaHK = txtMaHK.Text;
            obj.TenHK = txtTenHK.Text;
            obj.MaNH = cboMaNH.SelectedValue.ToString();

            if (_dal.CreateOrUpdate(obj))
            {
                HopThoai.ThongBao("Sửa học kỳ thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Sửa học kỳ không thành công !");

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as HocKyDTO;

            if (obj == null)
                return;

            var answer = HopThoai.XacNhan("Bạn muốn xoá học kỳ được chọn ?");

            if (!answer)
                return;

            if (_dal.Delete(obj.MaHK))
            {
                HopThoai.ThongBao("Xoá thông tin học kỳ thành công !");
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Xoá thông tin học kỳ không thành công !");
        }

        private void gridData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
            {
                return;
            }

            var obj = gridData.CurrentRow.DataBoundItem as HocKyDTO;

            if (obj == null)
                return;

            HienThi(obj);
        }

        private void HienThi(HocKyDTO obj)
        {
            txtMaHK.Text = obj.MaHK;
            txtTenHK.Text = obj.TenHK;

            if (!string.IsNullOrEmpty(obj.MaNH))
                cboMaNH.SelectedValue = obj.MaNH;
            else
                cboMaNH.SelectedIndex = 0;
        }
    }
}
