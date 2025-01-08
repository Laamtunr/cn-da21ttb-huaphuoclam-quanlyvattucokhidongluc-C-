using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DTO
{
    public class LichSuSuDungDTO
    {
        public string LoaiGiaoDich { get; set; }
        public DateTime NgayGiaoDich { get; set; }
        public string MaVT { get; set; }
        public string TenVT { get; set; }
        public decimal SoLuong { get; set; }
        public string MaPhieu { get; set; }
        public string MaSinhVien { get; set; }
        public string SoCanCuoc { get; set; }
        public string TenSinhVien { get; set; }
        public string MaNguoiThucHien { get; set; }
        public string GhiChu { get; set; }
    }
}
