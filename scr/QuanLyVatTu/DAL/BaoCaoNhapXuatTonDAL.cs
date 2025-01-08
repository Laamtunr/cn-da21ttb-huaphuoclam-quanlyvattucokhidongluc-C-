using Dapper;
using QuanLyMuonTraVatTu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    public class BaoCaoNhapXuatTonDAL
    {
        public List<BaoCaoNhapXuatTonDTO> Get(DateTime tuNgay, DateTime denNgay)
        {
            string sql = @"SELECT 
                    VT.MaVT,
                    VT.TenVT,
	                DVT.TenDVT,
                    -- Tồn đầu kỳ
                    ISNULL((SELECT SUM(CTPN.SoLuong) 
                            FROM CTPhieuNhap CTPN
                            INNER JOIN PhieuNhap PN ON CTPN.MaPN = PN.MaPN
                            WHERE CTPN.MaVT = VT.MaVT AND PN.NgayLap < @TuNgay), 0)
                    -
                    ISNULL((SELECT SUM(CTPM.SoLuong)
                            FROM CTPhieuMuon CTPM
                            INNER JOIN PhieuMuon PM ON CTPM.MaPM = PM.MaPM
                            WHERE CTPM.MaVT = VT.MaVT AND PM.NgayLap < @TuNgay), 0) AS TonDauKy,

                    -- Nhập trong kỳ
                    ISNULL((SELECT SUM(CTPN.SoLuong) 
                            FROM CTPhieuNhap CTPN
                            INNER JOIN PhieuNhap PN ON CTPN.MaPN = PN.MaPN
                            WHERE CTPN.MaVT = VT.MaVT AND PN.NgayLap BETWEEN @TuNgay AND @DenNgay), 0) AS NhapTrongKy,

                    -- Xuất trong kỳ
                    ISNULL((SELECT SUM(CTPM.SoLuong)
                            FROM CTPhieuMuon CTPM
                            INNER JOIN PhieuMuon PM ON CTPM.MaPM = PM.MaPM
                            WHERE CTPM.MaVT = VT.MaVT AND PM.NgayLap BETWEEN @TuNgay AND @DenNgay), 0) AS XuatTrongKy,

                    -- Tồn cuối kỳ
                    ISNULL((SELECT SUM(CTPN.SoLuong) 
                            FROM CTPhieuNhap CTPN
                            INNER JOIN PhieuNhap PN ON CTPN.MaPN = PN.MaPN
                            WHERE CTPN.MaVT = VT.MaVT AND PN.NgayLap <= @DenNgay), 0)
                    -
                    ISNULL((SELECT SUM(CTPM.SoLuong)
                            FROM CTPhieuMuon CTPM
                            INNER JOIN PhieuMuon PM ON CTPM.MaPM = PM.MaPM
                            WHERE CTPM.MaVT = VT.MaVT AND PM.NgayLap <= @DenNgay), 0) AS TonCuoiKy

                FROM VatTu VT
                LEFT JOIN DonViTinh DVT ON VT.MaDVT = DVT.MaDVT 
                ORDER BY VT.MaVT";

            return DapperDbContext.Connection
                .Query<BaoCaoNhapXuatTonDTO>(sql, new { TuNgay = tuNgay, DenNgay = denNgay})
                .ToList();
        }
    }
}
