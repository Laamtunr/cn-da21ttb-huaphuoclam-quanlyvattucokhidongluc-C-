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
    public partial class FormChonPhieuMuonChuaTra : Form
    {
        public FormChonPhieuMuonChuaTra()
        {
            InitializeComponent();
        }

        PhieuMuonDAL _dal = new PhieuMuonDAL();
        BindingSource _src = new BindingSource();
        public delegate void ChonPhieuMuonDelegate(PhieuMuonDTO obj);
        public event ChonPhieuMuonDelegate ChonPhieuMuonHandler;

        private void FormChonPhieuMuonChuaTra_Load(object sender, EventArgs e)
        {
            gridData.AutoGenerateColumns = true;
            gridData.AllowUserToAddRows = false;
            gridData.ReadOnly = true;
            gridData.DataSource = _src;
            LoadGridData();
        }

        private void LoadGridData()
        {
            _src.DataSource = _dal.GetDSPhieuMuonChuaTra();
            _src.ResetBindings(true);
        }

        private void gridData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
                return;

            PhieuMuonDTO obj = gridData.CurrentRow.DataBoundItem as PhieuMuonDTO;
            
            if (obj == null)
                return;

            if (ChonPhieuMuonHandler != null)
            {
                ChonPhieuMuonHandler(obj);
                this.Close();
            } 
        }
    }
}
