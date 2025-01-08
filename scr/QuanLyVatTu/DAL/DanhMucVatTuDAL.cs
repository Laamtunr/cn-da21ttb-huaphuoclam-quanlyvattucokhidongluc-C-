using Dapper;
using QuanLyMuonTraVatTu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    public class DanhMucVatTuDAL
    {
        public List<DanhMucVatTuDTO> Get()
        {
            string sql = @"SELECT [MaDM], [TenDM] FROM [DanhMucVatTu]";
            return DapperDbContext.Connection.Query<DanhMucVatTuDTO>(sql).ToList();
        }

        public DanhMucVatTuDTO Get(string MaDM)
        {
            string sql = @"SELECT [MaDM], [TenDM] FROM [DanhMucVatTu] WHERE [MaDM] = @MaDM";
            return DapperDbContext.Connection.QueryFirstOrDefault<DanhMucVatTuDTO>(sql, new { MaDM });
        }

        public bool CreateOrUpdate(DanhMucVatTuDTO obj)
        {
            string sql = @"INSERT INTO [DanhMucVatTu] ([MaDM], [TenDM]) VALUES (@MaDM, @TenDM)";
            if (Get(obj.MaDM) != null)
                sql = @"UPDATE [DanhMucVatTu] SET [TenDM] = @TenDM WHERE [MaDM] = @MaDM";
            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool Delete(string MaDM)
        {
            string sql = @"DELETE FROM [DanhMucVatTu] WHERE [MaDM] = @MaDM";

            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaDM }) > 0;
            } 
            catch (Exception)
            {
                return false;
            }
        }

        public string NewId(string prefix)
        {
            List<DanhMucVatTuDTO> items = Get();

            int maxId = 0;

            foreach (var item in items)
            {
                if (item.MaDM.StartsWith(prefix))
                {
                    if (int.TryParse(item.MaDM.Substring(prefix.Length), out int _currentId))
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
