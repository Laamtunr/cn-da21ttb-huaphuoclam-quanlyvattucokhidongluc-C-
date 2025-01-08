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

    public class HocKyDAL
    {
        public List<HocKyDTO> Get()
        {
            string sql = @"SELECT [MaHK], [TenHK], HocKy.[MaNH], TenNH
                       FROM [HocKy]
	LEFT JOIN NamHoc ON NamHoc.MaNH = HocKy.MaNH";

            return DapperDbContext.Connection.Query<HocKyDTO>(sql).ToList();
        }

        public HocKyDTO Get(string MaHK)
        {
            string sql = @"SELECT [MaHK], [TenHK], HocKy.[MaNH], TenNH
                       FROM [HocKy]
	LEFT JOIN NamHoc ON NamHoc.MaNH = HocKy.MaNH
                       WHERE ([MaHK] = @MaHK OR @MaHK IS NULL)";

            return DapperDbContext.Connection.QueryFirstOrDefault<HocKyDTO>(sql, new { MaHK });
        }

        public bool CreateOrUpdate(HocKyDTO obj)
        {
            string sql = @"INSERT INTO [HocKy] ([MaHK], [TenHK], [MaNH])
                       VALUES (@MaHK, @TenHK, @MaNH)";

            if (Get(obj.MaHK) != null)
            {
                sql = @"UPDATE [HocKy]
                    SET [TenHK] = @TenHK, [MaNH] = @MaNH
                    WHERE [MaHK] = @MaHK";
            }

            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool Delete(string MaHK)
        {
            string sql = @"DELETE FROM [HocKy]
                       WHERE [MaHK] = @MaHK";

            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaHK }) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string NewId(string prefix)
        {
            List<HocKyDTO> items = Get();

            int maxId = 0;

            foreach (var item in items)
            {
                if (item.MaHK.StartsWith(prefix))
                {
                    if (int.TryParse(item.MaHK.Substring(prefix.Length), out int _currentId))
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
