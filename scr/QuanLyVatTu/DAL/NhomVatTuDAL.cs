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

    public class NhomVatTuDAL
    {
        public List<NhomVatTuDTO> Get()
        {
            string sql = @"SELECT [MaNhom], [TenNhom], NhomVatTu.[MaDM], TenDM 
                       FROM [NhomVatTu]
	LEFT JOIN DanhMucVatTu ON DanhMucVatTu.MaDM = NhomVatTu.MaDM";

            return DapperDbContext.Connection.Query<NhomVatTuDTO>(sql).ToList();
        }

        public NhomVatTuDTO Get(string MaNhom)
        {
            string sql = @"SELECT [MaNhom], [TenNhom], NhomVatTu.[MaDM], TenDM 
                       FROM [NhomVatTu]
	LEFT JOIN DanhMucVatTu ON DanhMucVatTu.MaDM = NhomVatTu.MaDM 
                       WHERE ([MaNhom] = @MaNhom OR @MaNhom IS NULL)";

            return DapperDbContext.Connection.QueryFirstOrDefault<NhomVatTuDTO>(sql, new { MaNhom });
        }

        public bool CreateOrUpdate(NhomVatTuDTO obj)
        {
            string sql = @"INSERT INTO [NhomVatTu] ([MaNhom], [TenNhom], [MaDM]) 
                       VALUES (@MaNhom, @TenNhom, @MaDM)";

            if (Get(obj.MaNhom) != null)
            {
                sql = @"UPDATE [NhomVatTu] 
                    SET [TenNhom] = @TenNhom, [MaDM] = @MaDM 
                    WHERE [MaNhom] = @MaNhom";
            }

            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool Delete(string MaNhom)
        {
            string sql = @"DELETE FROM [NhomVatTu] 
                       WHERE [MaNhom] = @MaNhom";

            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaNhom }) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string NewId(string prefix)
        {
            List<NhomVatTuDTO> items = Get();

            int maxId = 0;

            foreach (var item in items)
            {
                if (item.MaNhom.StartsWith(prefix))
                {
                    if (int.TryParse(item.MaNhom.Substring(prefix.Length), out int _currentId))
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
