using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DTO
{
    public class BaoCaoVatTuChuaTraDTO
    {
        public string MaVT { get; set; }
        public string TenVT { get; set; }
        public string MaPM { get; set; }
        public string HoTenNguoiMuon { get; set; }
        public string MaSinhVien { get; set; }
        public DateTime NgayMuon { get; set; }
        public DateTime HanTra { get; set; }
        public decimal SoLuongDaMuon { get; set; }
        public decimal SoLuongDaTra { get; set; }
        public decimal SoLuongChuaTra { get; set; }
    }
}
