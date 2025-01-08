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

    public class LopDAL
    {
        public List<LopDTO> Get()
        {
            string sql = @"SELECT [MaLop], [TenLop] 
                       FROM [Lop]";

            return DapperDbContext.Connection.Query<LopDTO>(sql).ToList();
        }

        public LopDTO Get(string MaLop)
        {
            string sql = @"SELECT [MaLop], [TenLop] 
                       FROM [Lop] 
                       WHERE ([MaLop] = @MaLop OR @MaLop IS NULL)";

            return DapperDbContext.Connection.QueryFirstOrDefault<LopDTO>(sql, new { MaLop });
        }

        public bool CreateOrUpdate(LopDTO obj)
        {
            string sql = @"INSERT INTO [Lop] ([MaLop], [TenLop]) 
                       VALUES (@MaLop, @TenLop)";

            if (Get(obj.MaLop) != null)
            {
                sql = @"UPDATE [Lop] 
                    SET [TenLop] = @TenLop 
                    WHERE [MaLop] = @MaLop";
            }

            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool Delete(string MaLop)
        {
            string sql = @"DELETE FROM [Lop] 
                       WHERE [MaLop] = @MaLop";

            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaLop }) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string NewId(string prefix)
        {
            List<LopDTO> items = Get();

            int maxId = 0;

            foreach (var item in items)
            {
                if (item.MaLop.StartsWith(prefix))
                {
                    if (int.TryParse(item.MaLop.Substring(prefix.Length), out int _currentId))
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
