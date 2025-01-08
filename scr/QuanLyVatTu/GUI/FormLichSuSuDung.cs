using QuanLyMuonTraVatTu.DAL;
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
    public partial class FormLichSuSuDung : Form
    {
        public FormLichSuSuDung()
        {
            InitializeComponent();
        }

        VatTuDAL _vatTuDAL = new VatTuDAL();
        LichSuSuDungDAL _lichSuSuDungDAL = new LichSuSuDungDAL();
        BindingSource _src = new BindingSource();

        private void LoadComboSP()
        {
            var dsVT = _vatTuDAL.Get();
            dsVT.Insert(0, new DTO.VatTuDTO { MaVT = "", TenVT = "-- TẤT CẢ --" });
            cboMaVT.DataSource = dsVT;
            cboMaVT.DisplayMember = "TenVT";
            cboMaVT.ValueMember = "MaVT";
            cboMaVT.SelectedIndex = 0;
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            var dsLichSu = _lichSuSuDungDAL.Get();
            if (cboMaVT.SelectedIndex > 0)
                dsLichSu = dsLichSu.Where(p => p.MaVT == cboMaVT.SelectedValue.ToString()).ToList();
            dsLichSu = dsLichSu.Where(p => p.NgayGiaoDich.Date >= dtTuNgay.Value.Date && p.NgayGiaoDich.Date <= dtDenNgay.Value.Date)
                .ToList();
            _src.DataSource = dsLichSu;
            _src.ResetBindings(true);
        }

        private void FormLichSuSuDung_Load(object sender, EventArgs e)
        {
            gridData.ReadOnly = true;
            gridData.AllowUserToAddRows = false;
            gridData.DataSource = _src;
            LoadComboSP();
            dtTuNgay.Value = DateTime.Now.AddMonths(-1);
            dtDenNgay.Value = DateTime.Now;
        }
    }
}
