using Dapper;
using QuanLyMuonTraVatTu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    public class BaoCaoVatTuChuaTraDAL
    {
        public List<BaoCaoVatTuChuaTraDTO> Get()
        {
            string sql = @"SELECT 
                VT.MaVT,
                VT.TenVT,
                PM.MaPM,
                PM.HoTenNguoiMuon,
                PM.MaSinhVien,
                PM.NgayLap AS NgayMuon,
                PM.HanTra,
                ISNULL(SUM(CTPM.SoLuong), 0) AS SoLuongDaMuon,
                ISNULL(
                    (SELECT SUM(CTPT.SoLuong)
                     FROM CTPhieuTra CTPT
                     INNER JOIN PhieuTra PT ON CTPT.MaPT = PT.MaPT
                     WHERE CTPT.MaVT = VT.MaVT AND PT.MaPM = PM.MaPM), 0
                ) AS SoLuongDaTra,
                ISNULL(SUM(CTPM.SoLuong), 0) -
                ISNULL(
                    (SELECT SUM(CTPT.SoLuong)
                     FROM CTPhieuTra CTPT
                     INNER JOIN PhieuTra PT ON CTPT.MaPT = PT.MaPT
                     WHERE CTPT.MaVT = VT.MaVT AND PT.MaPM = PM.MaPM), 0
                ) AS SoLuongChuaTra
            FROM CTPhieuMuon CTPM
            INNER JOIN PhieuMuon PM ON CTPM.MaPM = PM.MaPM
            INNER JOIN VatTu VT ON CTPM.MaVT = VT.MaVT
            GROUP BY VT.MaVT, VT.TenVT, PM.MaPM, PM.HoTenNguoiMuon, PM.MaSinhVien, PM.NgayLap, PM.HanTra
            HAVING 
                ISNULL(SUM(CTPM.SoLuong), 0) >
                ISNULL(
                    (SELECT SUM(CTPT.SoLuong)
                     FROM CTPhieuTra CTPT
                     INNER JOIN PhieuTra PT ON CTPT.MaPT = PT.MaPT
                     WHERE CTPT.MaVT = VT.MaVT AND PT.MaPM = PM.MaPM), 0
                )
            ORDER BY PM.NgayLap, VT.MaVT;
            ";
            return DapperDbContext
                .Connection
                .Query<BaoCaoVatTuChuaTraDTO>(sql)
                .ToList();
        }
    }
}
