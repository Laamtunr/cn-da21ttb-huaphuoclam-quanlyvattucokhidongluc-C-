using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DTO
{
    public class VatTuDTO
    {
        public string MaVT { get; set; }
        public string TenVT { get; set; }
        public string MoTa { get; set; }
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public string MaDVT { get; set; }
        public string TenDVT { get; set; }
        public string TinhTrang { get; set; }
        public decimal? TonKho { get; set; }
        public decimal? TonKhoToiThieu { get; set; }
    }

}
