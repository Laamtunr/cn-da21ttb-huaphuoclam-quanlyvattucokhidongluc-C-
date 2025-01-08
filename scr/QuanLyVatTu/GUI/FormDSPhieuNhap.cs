using GUI.Helpers;
using QuanLyMuonTraVatTu.DAL;
using QuanLyMuonTraVatTu.DTO;
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
    public partial class FormDSPhieuNhap : Form
    {
        public FormDSPhieuNhap()
        {
            InitializeComponent();
        }

        PhieuNhapDAL _dal = new PhieuNhapDAL();
        CTPhieuNhapDAL _dalCTPhieuNhap = new CTPhieuNhapDAL();
        VatTuDAL _dalVatTu = new VatTuDAL();
        BindingSource _src = new BindingSource();
        BindingSource _srcCT = new BindingSource();
        private void FormDSPhieuNhap_Load(object sender, EventArgs e)
        {
            gridData.AutoGenerateColumns = true;
            gridData.AllowUserToAddRows = false;
            gridData.ReadOnly = true;
            gridData.DataSource = _src;

            gridCT.AutoGenerateColumns = true;
            gridCT.AllowUserToAddRows = false;
            gridCT.ReadOnly = true;
            gridCT.DataSource = _srcCT;

            LoadGridData();
        }

        private void LoadGridData()
        {
            _src.DataSource = _dal.Get();
            _src.ResetBindings(true);
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            FormPhieuNhap f = new FormPhieuNhap();
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
            LoadGridData();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
                return;

            var obj = gridData.CurrentRow.DataBoundItem as PhieuNhapDTO;

            if (obj == null)
                return;

            FormPhieuNhap f = new FormPhieuNhap(obj);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
            LoadGridData();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
                return;

            var obj = gridData.CurrentRow.DataBoundItem as PhieuNhapDTO;

            if (obj == null)
                return;

            var answer = HopThoai.XacNhan("Bạn muốn xoá phiếu nhập được chọn ?");

            if (!answer)
                return;

            // Xoá chi tiết phiếu nhập
            var dsCTPM = _dalCTPhieuNhap.GetBy(obj.MaPN);

            if (_dalCTPhieuNhap.DeleteBy(obj.MaPN))
            {
                for (int i = 0; i < dsCTPM.Count; i++)
                {
                    var vt = _dalVatTu.Get(dsCTPM[i].MaVT);

                    if (vt != null)
                    {
                        vt.TonKho -= dsCTPM[i].SoLuong;
                        _dalVatTu.CreateOrUpdate(vt);
                    }
                }
            }

            // Xoá phiếu nhập
            if (_dal.Delete(obj.MaPN))
            {
                HopThoai.ThongBao("Xoá phiếu nhập thành công !");
                _srcCT.DataSource = new List<CTPhieuNhapDTO>();
                _srcCT.ResetBindings(true);
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Xoá phiếu nhập không thành công !");
        }

        private void gridData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
                return;

            var obj = gridData.CurrentRow.DataBoundItem as PhieuNhapDTO;

            if (obj == null)
                return;

            var dsCT = _dalCTPhieuNhap.GetBy(obj.MaPN);

            _srcCT.DataSource = dsCT;
            _srcCT.ResetBindings(true);
        }
    }
}
