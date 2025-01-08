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
    public partial class FormBaoCaoSoLanMuonVatTu : Form
    {
        public FormBaoCaoSoLanMuonVatTu()
        {
            InitializeComponent();
        }

        BaoCaoSoLanMuonVatTuDAL _baoCaoSoLanMuonVatTuDAL = new BaoCaoSoLanMuonVatTuDAL();
        BindingSource _src = new BindingSource();
        private void FormBaoCaoSoLanMuonVatTu_Load(object sender, EventArgs e)
        {
            gridData.ReadOnly = true;
            gridData.AllowUserToAddRows = false;
            gridData.DataSource = _src;
        }

        private void LoadGrid()
        {
            var ds = _baoCaoSoLanMuonVatTuDAL.Get();
            _src.DataSource = ds;
            _src.ResetBindings(true);
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }
    }
}
