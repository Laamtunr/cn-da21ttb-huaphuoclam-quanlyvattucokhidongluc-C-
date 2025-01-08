using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DTO
{
    public class BaoCaoNhapXuatTonDTO
    {
        public string MaVT { get; set; }
        public string TenVT { get; set; }
        public string TenDVT { get; set; }
        public decimal TonDauKy { get; set; }
        public decimal NhapTrongKy { get; set; }
        public decimal XuatTrongKy { get; set; }
        public decimal TonCuoiKy { get; set; }
    }
}
