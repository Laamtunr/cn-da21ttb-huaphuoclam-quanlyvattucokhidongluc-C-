using Microsoft.Reporting.WinForms;
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
    public partial class FormBaoCaoNhapXuatTon : Form
    {
        public FormBaoCaoNhapXuatTon()
        {
            InitializeComponent();
        }

        VatTuDAL _vatTuDAL = new VatTuDAL();
        BaoCaoNhapXuatTonDAL _baoCaoNhapXuatTonDAL = new BaoCaoNhapXuatTonDAL();

        private void FormBaoCaoNhapXuatTon_Load(object sender, EventArgs e)
        {
            dtTuNgay.Value = DateTime.Now.AddMonths(-1);
            dtDenNgay.Value = DateTime.Now;
            this.rptViewer.RefreshReport();
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            var dsTon = _baoCaoNhapXuatTonDAL.Get(dtTuNgay.Value, dtDenNgay.Value);
 
            ReportDataSource reportDataSource = new ReportDataSource();
            //// Must match the DataSource in the RDLC
            reportDataSource.Name = "vBaoCaoNhapXuatTon";
            reportDataSource.Value = dsTon;

            this.rptViewer.ProcessingMode = ProcessingMode.Local;
            this.rptViewer.LocalReport.ReportPath = "Reports/RptNhapXuatTon.rdlc";

            this.rptViewer.LocalReport.SetParameters(new ReportParameter("TU_NGAY", dtTuNgay.Value.ToString("dd/MM/yyyy")));
            this.rptViewer.LocalReport.SetParameters(new ReportParameter("DEN_NGAY", dtDenNgay.Value.ToString("dd/MM/yyyy")));

            this.rptViewer.LocalReport.DataSources.Clear();
            this.rptViewer.LocalReport.DataSources.Add(reportDataSource);
            this.rptViewer.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            this.rptViewer.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;
            this.rptViewer.ZoomPercent = 100;
            this.rptViewer.RefreshReport();
        }
    }
}
