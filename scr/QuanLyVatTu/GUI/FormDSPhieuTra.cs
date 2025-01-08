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
    public partial class FormDSPhieuTra : Form
    {
        public FormDSPhieuTra()
        {
            InitializeComponent();
        }

        PhieuTraDAL _dal = new PhieuTraDAL();
        CTPhieuTraDAL _dalCTPhieuTra = new CTPhieuTraDAL();
        VatTuDAL _dalVatTu = new VatTuDAL();
        BindingSource _src = new BindingSource();
        BindingSource _srcCT = new BindingSource();
        private void FormDSPhieuTra_Load(object sender, EventArgs e)
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
            FormPhieuTra f = new FormPhieuTra();
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
            LoadGridData();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
                return;

            var obj = gridData.CurrentRow.DataBoundItem as PhieuTraDTO;

            if (obj == null)
                return;

            FormPhieuTra f = new FormPhieuTra(obj);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
            LoadGridData();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
                return;

            var obj = gridData.CurrentRow.DataBoundItem as PhieuTraDTO;

            if (obj == null)
                return;

            var answer = HopThoai.XacNhan("Bạn muốn xoá phiếu trả được chọn ?");

            if (!answer)
                return;

            // Xoá chi tiết phiếu trả
            var dsCTPM = _dalCTPhieuTra.GetBy(obj.MaPT);

            if (_dalCTPhieuTra.DeleteBy(obj.MaPT))
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

            // Xoá phiếu trả
            if (_dal.Delete(obj.MaPT))
            {
                HopThoai.ThongBao("Xoá phiếu trả thành công !");
                _srcCT.DataSource = new List<CTPhieuTraDTO>();
                _srcCT.ResetBindings(true);
                LoadGridData();
                return;
            }

            HopThoai.BaoLoi("Xoá phiếu trả không thành công !");
        }

        private void gridData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
                return;

            var obj = gridData.CurrentRow.DataBoundItem as PhieuTraDTO;

            if (obj == null)
                return;

            var dsCT = _dalCTPhieuTra.GetBy(obj.MaPT);

            _srcCT.DataSource = dsCT;
            _srcCT.ResetBindings(true);
        }
    }
}
