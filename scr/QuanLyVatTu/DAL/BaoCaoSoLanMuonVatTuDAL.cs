using Dapper;
using QuanLyMuonTraVatTu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    public class BaoCaoSoLanMuonVatTuDAL
    {
        public List<BaoCaoSoLanMuonVatTuDTO> Get()
        {
            string sql = @"
                SELECT 
                    VT.MaVT,
                    VT.TenVT,
                    COUNT(DISTINCT PM.MaPM) AS SoLanMuon
                FROM CTPhieuMuon CTPM
                INNER JOIN PhieuMuon PM ON CTPM.MaPM = PM.MaPM
                INNER JOIN VatTu VT ON CTPM.MaVT = VT.MaVT
                GROUP BY VT.MaVT, VT.TenVT
                ORDER BY VT.MaVT;
            ";
            return DapperDbContext
                .Connection
                .Query<BaoCaoSoLanMuonVatTuDTO>(sql)
                .ToList();
        }
    }
}
