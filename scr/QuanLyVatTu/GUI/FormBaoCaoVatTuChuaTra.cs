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
    public partial class FormBaoCaoVatTuChuaTra : Form
    {
        public FormBaoCaoVatTuChuaTra()
        {
            InitializeComponent();
        }

        BaoCaoVatTuChuaTraDAL _baoCaoVatTuChuaTra = new BaoCaoVatTuChuaTraDAL();
        private void btnXem_Click(object sender, EventArgs e)
        {
            var dsVatTuChuaTra = _baoCaoVatTuChuaTra.Get();
            ReportDataSource reportDataSource = new ReportDataSource();
            //// Must match the DataSource in the RDLC
            reportDataSource.Name = "vBaoCaoVatTuChuaTra";
            reportDataSource.Value = dsVatTuChuaTra;

            this.rptViewer.ProcessingMode = ProcessingMode.Local;
            this.rptViewer.LocalReport.ReportPath = "Reports/RptBaoCaoVatTuChuaTra.rdlc";

            this.rptViewer.LocalReport.SetParameters(new ReportParameter("NGAY", DateTime.Now.ToString("dd/MM/yyyy")));

            this.rptViewer.LocalReport.DataSources.Clear();
            this.rptViewer.LocalReport.DataSources.Add(reportDataSource);
            this.rptViewer.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            this.rptViewer.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;
            this.rptViewer.ZoomPercent = 100;
            this.rptViewer.RefreshReport();
        }
    }
}
