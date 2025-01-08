using Dapper;
using QuanLyMuonTraVatTu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    public class LichSuSuDungDAL
    {
        public List<LichSuSuDungDTO> Get()
        {
            string sql = @"SELECT 
                N'Nhập' AS LoaiGiaoDich,
                PN.NgayLap AS NgayGiaoDich,
                VT.MaVT,
                VT.TenVT,
                CTPN.SoLuong AS SoLuong,
                PN.MaPN AS MaPhieu,
                NULL AS MaSinhVien,
	            NULL AS SoCanCuoc,
	            NULL AS TenSinhVien,
                PN.MaNV AS MaNguoiThucHien,
                PN.GhiChu
            FROM CTPhieuNhap CTPN
            INNER JOIN PhieuNhap PN ON CTPN.MaPN = PN.MaPN
            INNER JOIN VatTu VT ON CTPN.MaVT = VT.MaVT

            UNION ALL

            SELECT 
                N'Mượn' AS LoaiGiaoDich,
                PM.NgayLap AS NgayGiaoDich,
                VT.MaVT,
                VT.TenVT,
                CTPM.SoLuong AS SoLuong,
                PM.MaPM AS MaPhieu,
                PM.MaSinhVien AS MaSinhVien,
	            PM.SoCanCuoc AS SoCanCuoc,
	            PM.HoTenNguoiMuon AS TenSinhVien,
                PM.MaNV AS MaNguoiThucHien,
                NULL AS GhiChu
            FROM CTPhieuMuon CTPM
            INNER JOIN PhieuMuon PM ON CTPM.MaPM = PM.MaPM
            INNER JOIN VatTu VT ON CTPM.MaVT = VT.MaVT
            UNION ALL

            SELECT 
                N'Trả' AS LoaiGiaoDich,
                PT.NgayLap AS NgayGiaoDich,
                VT.MaVT,
                VT.TenVT,
                CTPTR.SoLuong AS SoLuong,
                PT.MaPT AS MaPhieu,
                NULL AS MaSinhVien,
	            NULL AS SoCanCuoc,
	            NULL AS TenSinhVien,
                PT.MaNV AS MaNguoiThucHien,
                PT.GhiChu
            FROM CTPhieuTra CTPTR
            INNER JOIN PhieuTra PT ON CTPTR.MaPT = PT.MaPT
            INNER JOIN VatTu VT ON CTPTR.MaVT = VT.MaVT

            ORDER BY NgayGiaoDich, LoaiGiaoDich, MaVT";
            return DapperDbContext.Connection.Query<LichSuSuDungDTO>(sql).ToList();
        }
    }
}
