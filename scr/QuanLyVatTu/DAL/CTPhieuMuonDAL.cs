using Dapper;
using QuanLyMuonTraVatTu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    public class CTPhieuMuonDAL
    {
        public List<CTPhieuMuonDTO> GetBy(string MaPM)
        {
            string sql = @"SELECT [MaPM], CTPhieuMuon.[MaVT], TenVT, [SoLuong], CTPhieuMuon.[TinhTrang] FROM [CTPhieuMuon]
LEFT JOIN VatTu ON VatTu.MaVT = CTPhieuMuon.MaVT
                WHERE MaPM = @MaPM";
            return DapperDbContext.Connection.Query<CTPhieuMuonDTO>(sql, new { MaPM }).ToList();
        }

        public CTPhieuMuonDTO Get(string MaPM, string MaVT)
        {
            string sql = @"SELECT [MaPM], CTPhieuMuon.[MaVT], TenVT, [SoLuong], CTPhieuMuon.[TinhTrang] FROM [CTPhieuMuon]
LEFT JOIN VatTu ON VatTu.MaVT = CTPhieuMuon.MaVT 
                       WHERE [MaPM] = @MaPM AND CTPhieuMuon.[MaVT] = @MaVT";
            return DapperDbContext.Connection.QueryFirstOrDefault<CTPhieuMuonDTO>(sql, new { MaPM, MaVT });
        }

        public bool CreateOrUpdate(CTPhieuMuonDTO obj)
        {
            string sql = @"INSERT INTO [CTPhieuMuon] ([MaPM], [MaVT], [SoLuong], [TinhTrang]) 
                       VALUES (@MaPM, @MaVT, @SoLuong, @TinhTrang)";
            if (Get(obj.MaPM, obj.MaVT) != null)
                sql = @"UPDATE [CTPhieuMuon] 
                    SET [SoLuong] = @SoLuong, [TinhTrang] = @TinhTrang 
                    WHERE [MaPM] = @MaPM AND [MaVT] = @MaVT";
            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool DeleteBy(string MaPM)
        {
            string sql = @"DELETE FROM [CTPhieuMuon] WHERE [MaPM] = @MaPM";
            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaPM }) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
