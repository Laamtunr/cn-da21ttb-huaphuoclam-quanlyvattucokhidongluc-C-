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
    public partial class FormNhapXuatTon : Form
    {
        public FormNhapXuatTon()
        {
            InitializeComponent();
        }

        VatTuDAL _vatTuDAL = new VatTuDAL();
        BaoCaoNhapXuatTonDAL _baoCaoNhapXuatTonDAL = new BaoCaoNhapXuatTonDAL();
        BindingSource _src = new BindingSource();

        private void FormNhapXuatTon_Load(object sender, EventArgs e)
        {
            gridData.AutoGenerateColumns = false;
            gridData.ReadOnly = true;
            gridData.AllowUserToAddRows = false;
            gridData.DataSource = _src;
            LoadComboSP();
            dtTuNgay.Value = DateTime.Now.AddMonths(-1);
            dtDenNgay.Value = DateTime.Now;
            LoadGrid();
        }

        private void LoadComboSP()
        {
            var dsVT = _vatTuDAL.Get();
            dsVT.Insert(0, new DTO.VatTuDTO { MaVT = "", TenVT = "Tất cả" });
            cboMaVT.DataSource = dsVT;
            cboMaVT.DisplayMember = "TenVT";
            cboMaVT.ValueMember = "MaVT";
            cboMaVT.SelectedIndex = 0;
        }

        private void LoadGrid()
        {
            var dsTon = _baoCaoNhapXuatTonDAL.Get(dtTuNgay.Value, dtDenNgay.Value);
            if (cboMaVT.SelectedIndex > 0)
                dsTon = dsTon.Where(p => p.MaVT == cboMaVT.SelectedValue.ToString()).ToList();
            _src.DataSource = dsTon;
            _src.ResetBindings(true);
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }
    }
}
