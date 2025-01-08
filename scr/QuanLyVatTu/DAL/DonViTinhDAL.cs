using Dapper;
using QuanLyMuonTraVatTu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    public class DonViTinhDAL
    {
        public List<DonViTinhDTO> Get()
        {
            string sql = @"SELECT [MaDVT], [TenDVT] FROM [DonViTinh]";
            return DapperDbContext.Connection.Query<DonViTinhDTO>(sql).ToList();
        }

        public DonViTinhDTO Get(string MaDVT)
        {
            string sql = @"SELECT [MaDVT], [TenDVT] FROM [DonViTinh] WHERE [MaDVT] = @MaDVT";
            return DapperDbContext.Connection.QueryFirstOrDefault<DonViTinhDTO>(sql, new { MaDVT });
        }

        public bool CreateOrUpdate(DonViTinhDTO obj)
        {
            string sql = @"INSERT INTO [DonViTinh] ([MaDVT], [TenDVT]) VALUES (@MaDVT, @TenDVT)";
            if (Get(obj.MaDVT) != null)
                sql = @"UPDATE [DonViTinh] SET [TenDVT] = @TenDVT WHERE [MaDVT] = @MaDVT";
            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool Delete(string MaDVT)
        {
            string sql = @"DELETE FROM [DonViTinh] WHERE [MaDVT] = @MaDVT";
            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaDVT }) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string NewId(string prefix)
        {
            List<DonViTinhDTO> items = Get();

            int maxId = 0;

            foreach (var item in items)
            {
                if (item.MaDVT.StartsWith(prefix))
                {
                    if (int.TryParse(item.MaDVT.Substring(prefix.Length), out int _currentId))
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
