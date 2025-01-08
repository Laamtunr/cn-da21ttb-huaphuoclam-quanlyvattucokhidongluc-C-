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

    public class NamHocDAL
    {
        public List<NamHocDTO> Get()
        {
            string sql = @"SELECT [MaNH], [TenNH] 
                       FROM [NamHoc]";

            return DapperDbContext.Connection.Query<NamHocDTO>(sql).ToList();
        }

        public NamHocDTO Get(string MaNH)
        {
            string sql = @"SELECT [MaNH], [TenNH] 
                       FROM [NamHoc] 
                       WHERE ([MaNH] = @MaNH OR @MaNH IS NULL)";

            return DapperDbContext.Connection.QueryFirstOrDefault<NamHocDTO>(sql, new { MaNH });
        }

        public bool CreateOrUpdate(NamHocDTO obj)
        {
            string sql = @"INSERT INTO [NamHoc] ([MaNH], [TenNH]) 
                       VALUES (@MaNH, @TenNH)";

            if (Get(obj.MaNH) != null)
            {
                sql = @"UPDATE [NamHoc] 
                    SET [TenNH] = @TenNH 
                    WHERE [MaNH] = @MaNH";
            }

            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool Delete(string MaNH)
        {
            string sql = @"DELETE FROM [NamHoc] 
                       WHERE [MaNH] = @MaNH";

            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaNH }) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string NewId(string prefix)
        {
            List<NamHocDTO> items = Get();

            int maxId = 0;

            foreach (var item in items)
            {
                if (item.MaNH.StartsWith(prefix))
                {
                    if (int.TryParse(item.MaNH.Substring(prefix.Length), out int _currentId))
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
