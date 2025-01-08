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
    public partial class FormTimKiem : Form
    {
        public FormTimKiem()
        {
            InitializeComponent();
        }

        VatTuDAL _vatTuDAL = new VatTuDAL();
        BindingSource _src = new BindingSource();

        private void FormTimKiem_Load(object sender, EventArgs e)
        {
            gridData.ReadOnly = true;
            gridData.AllowUserToAddRows = false;
            gridData.DataSource = _src;
            LoadGrid();
        }

        private void LoadGrid()
        {
            var dsVT = _vatTuDAL.Get();
            _src.DataSource = dsVT;
            _src.ResetBindings(true);
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNoiDung.Text))
            {
                LoadGrid();
                return;
            }

            string searchTerm = txtNoiDung.Text.ToLower();
            var dsVT = _vatTuDAL.Get();
            dsVT = dsVT.Where(p => p.MaVT.ToLower().Contains(searchTerm)
            || p.TenVT.ToLower().Contains(searchTerm)
            || p.TenDVT.ToLower().Contains(searchTerm)
            || p.TenLoai.ToLower().Contains(searchTerm)
            || p.MoTa.ToLower().Contains(searchTerm)
            || p.TinhTrang.ToLower().Contains(searchTerm)
            ).ToList();

            _src.DataSource = dsVT;
            _src.ResetBindings(true);
        }

        private void txtNoiDung_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                btnTim_Click(null, null);
        }
    }
}
