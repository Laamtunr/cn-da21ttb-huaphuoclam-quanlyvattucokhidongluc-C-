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
    public partial class FormDSPhieuMuon : Form
    {
        public FormDSPhieuMuon()
        {
            InitializeComponent();
        }

        PhieuMuonDAL _dal = new PhieuMuonDAL();
        CTPhieuMuonDAL _dalCTPhieuMuon = new CTPhieuMuonDAL();
        VatTuDAL _dalVatTu = new VatTuDAL();
        BindingSource _src = new BindingSource();
        BindingSource _srcCT = new BindingSource();
        private void FormDSPhieuMuon_Load(object sender, EventArgs e)
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
            FormPhieuMuon f = new FormPhieuMuon();
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
            LoadGridData();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
                return;

            var obj = gridData.CurrentRow.DataBoundItem as PhieuMuonDTO;

            if (obj == null)
                return;

            FormPhieuMuon f = new FormPhieuMuon(obj);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
            LoadGridData();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
                return;

            var obj = gridData.CurrentRow.DataBoundItem as PhieuMuonDTO;

            if (obj == null)
                return;

            var answer = HopThoai.XacNhan("Bạn muốn xoá phiếu mượn được chọn ?");

            if (!answer)
                return;

            // Xoá chi tiết phiếu mượn
            var dsCTPM = _dalCTPhieuMuon.GetBy(obj.MaPM);

            if (_dalCTPhieuMuon.DeleteBy(obj.MaPM))
            {
                for (int i = 0; i < dsCTPM.Count; i++)
                {
                    var vt = _dalVatTu.Get(dsCTPM[i].MaVT);

                    if (vt != null)
                    {
                        vt.TonKho += dsCTPM[i].SoLuong;
                        _dalVatTu.CreateOrUpdate(vt);
                    }
                }
            }

            // Xoá phiếu mượn
            if (_dal.Delete(obj.MaPM))
            {
                HopThoai.ThongBao("Xoá phiếu mượn thành công !");
                _srcCT.DataSource = new List<CTPhieuMuonDTO>();
                _srcCT.ResetBindings(true);
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Xoá phiếu mượn không thành công !");
        }

        private void gridData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
                return;

            var obj = gridData.CurrentRow.DataBoundItem as PhieuMuonDTO;

            if (obj == null)
                return;

            var dsCT = _dalCTPhieuMuon.GetBy(obj.MaPM);

            _srcCT.DataSource = dsCT;
            _srcCT.ResetBindings(true);
        }
    }
}
