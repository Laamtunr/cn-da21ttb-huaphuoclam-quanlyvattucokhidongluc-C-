using Dapper;
using QuanLyMuonTraVatTu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    public class CTPhieuNhapDAL
    {
        public List<CTPhieuNhapDTO> GetBy(string MaPN)
        {
            string sql = @"SELECT [MaPN], CTPhieuNhap.[MaVT], TenVT, [SoLuong] FROM [CTPhieuNhap]
LEFT JOIN VatTu ON VatTu.MaVT = CTPhieuNhap.MaVT
                WHERE MaPN = @MaPN";
            return DapperDbContext.Connection.Query<CTPhieuNhapDTO>(sql,new { MaPN }).ToList();
        }

        public CTPhieuNhapDTO Get(string MaPN, string MaVT)
        {
            string sql = @"SELECT [MaPN], CTPhieuNhap.[MaVT], TenVT, [SoLuong] FROM [CTPhieuNhap]
LEFT JOIN VatTu ON VatTu.MaVT = CTPhieuNhap.MaVT 
                       WHERE [MaPN] = @MaPN AND CTPhieuNhap.[MaVT] = @MaVT";
            return DapperDbContext.Connection.QueryFirstOrDefault<CTPhieuNhapDTO>(sql, new { MaPN, MaVT });
        }

        public bool CreateOrUpdate(CTPhieuNhapDTO obj)
        {
            string sql = @"INSERT INTO [CTPhieuNhap] ([MaPN], [MaVT], [SoLuong]) 
                       VALUES (@MaPN, @MaVT, @SoLuong)";
            if (Get(obj.MaPN, obj.MaVT) != null)
                sql = @"UPDATE [CTPhieuNhap] 
                    SET [SoLuong] = @SoLuong 
                    WHERE [MaPN] = @MaPN AND [MaVT] = @MaVT";
            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool DeleteBy(string MaPN)
        {
            string sql = @"DELETE FROM [CTPhieuNhap] WHERE [MaPN] = @MaPN";
            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaPN }) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

}
