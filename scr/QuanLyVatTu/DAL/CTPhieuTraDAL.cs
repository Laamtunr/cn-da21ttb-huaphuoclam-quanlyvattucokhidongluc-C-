using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    using Dapper;
    using QuanLyMuonTraVatTu.DTO;
    using System.Collections.Generic;
    using System.Linq;

    public class CTPhieuTraDAL
    {
        public List<CTPhieuTraDTO> GetBy(string MaPT)
        {
            string sql = @"SELECT [MaPT], CTPhieuTra.[MaVT], TenVT, [SoLuong], CTPhieuTra.[TinhTrang]
	FROM [CTPhieuTra] LEFT JOIN VatTu ON VatTu.MaVT = CTPhieuTra.MaVT
        WHERE MaPT = @MaPT";

            return DapperDbContext.Connection.Query<CTPhieuTraDTO>(sql, new { MaPT })
                .ToList();
        }

        public CTPhieuTraDTO Get(string MaPT, string MaVT)
        {
            string sql = @"SELECT [MaPT], CTPhieuTra.[MaVT], TenVT, [SoLuong], CTPhieuTra.[TinhTrang]
	FROM [CTPhieuTra] LEFT JOIN VatTu ON VatTu.MaVT = CTPhieuTra.MaVT
                       WHERE ([MaPT] = @MaPT)
                         AND (CTPhieuTra.[MaVT] = @MaVT)";

            return DapperDbContext.Connection.QueryFirstOrDefault<CTPhieuTraDTO>(sql, new { MaPT, MaVT });
        }

        public bool CreateOrUpdate(CTPhieuTraDTO obj)
        {
            string sql = @"INSERT INTO [CTPhieuTra] ([MaPT], [MaVT], [SoLuong], [TinhTrang])
                       VALUES (@MaPT, @MaVT, @SoLuong, @TinhTrang)";

            if (Get(obj.MaPT, obj.MaVT) != null)
            {
                sql = @"UPDATE [CTPhieuTra]
                    SET [SoLuong] = @SoLuong, [TinhTrang] = @TinhTrang
                    WHERE [MaPT] = @MaPT AND [MaVT] = @MaVT";
            }

            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool DeleteBy(string MaPT)
        {
            string sql = @"DELETE FROM [CTPhieuTra]
                       WHERE [MaPT] = @MaPT";

            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaPT }) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

}
