using Dapper;
using QuanLyMuonTraVatTu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    public class PhieuNhapDAL
    {
        public List<PhieuNhapDTO> Get()
        {
            string sql = @"SELECT [MaPN], [NgayLap], PhieuNhap.[MaNV], HoTen, [GhiChu] 
	FROM   [PhieuNhap] 
LEFT JOIN NhanVien ON NhanVien.MaNV = PhieuNhap.MaNV";
            return DapperDbContext.Connection.Query<PhieuNhapDTO>(sql).ToList();
        }

        public PhieuNhapDTO Get(string MaPN)
        {
            string sql = @"SELECT [MaPN], [NgayLap], PhieuNhap.[MaNV], HoTen, [GhiChu] 
	FROM   [PhieuNhap] 
LEFT JOIN NhanVien ON NhanVien.MaNV = PhieuNhap.MaNV 
	WHERE  ([MaPN] = @MaPN OR @MaPN IS NULL) ";
            return DapperDbContext.Connection.QueryFirstOrDefault<PhieuNhapDTO>(sql, new { MaPN });
        }

        public bool CreateOrUpdate(PhieuNhapDTO obj)
        {
            string sql = @"INSERT INTO [PhieuNhap] ([MaPN], [NgayLap], [MaNV], [GhiChu]) 
	VALUES (@MaPN, @NgayLap, @MaNV, @GhiChu)";
            if (Get(obj.MaPN) != null)
                sql = @"UPDATE [PhieuNhap]
	SET    [NgayLap] = @NgayLap, [MaNV] = @MaNV, [GhiChu] = @GhiChu
	WHERE  [MaPN] = @MaPN";
            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool Delete(string MaPN)
        {
            string sql = @"DELETE FROM [PhieuNhap] WHERE [MaPN] = @MaPN";
            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaPN }) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string NewId(string prefix)
        {
            List<PhieuNhapDTO> items = Get();

            int maxId = 0;

            foreach (var item in items)
            {
                if (item.MaPN.StartsWith(prefix))
                {
                    if (int.TryParse(item.MaPN.Substring(prefix.Length), out int _currentId))
                    {
                        if (_currentId > maxId)
                            maxId = _currentId;
                    }
                }
            }

            return string.Format("{0}{1}", prefix, (maxId + 1)
                .ToString()
                .PadLeft(3, '0'));
        }
    }

}
