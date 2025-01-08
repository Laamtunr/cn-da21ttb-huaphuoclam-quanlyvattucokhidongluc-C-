using Dapper;
using QuanLyMuonTraVatTu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    public class LoaiVatTuDAL
    {
        public List<LoaiVatTuDTO> Get()
        {
            string sql = @"SELECT [MaLoai], [TenLoai], LoaiVatTu.[MaNhom]
	, TenNhom
	FROM   [LoaiVatTu] 
	LEFT JOIN NhomVatTu ON NhomVatTu.MaNhom = LoaiVatTu.MaNhom";
            return DapperDbContext.Connection.Query<LoaiVatTuDTO>(sql).ToList();
        }

        public LoaiVatTuDTO Get(string MaLoai)
        {
            string sql = @"SELECT [MaLoai], [TenLoai], LoaiVatTu.[MaNhom]
	, TenNhom
	FROM   [LoaiVatTu] 
	LEFT JOIN NhomVatTu ON NhomVatTu.MaNhom = LoaiVatTu.MaNhom WHERE [MaLoai] = @MaLoai";
            return DapperDbContext.Connection.QueryFirstOrDefault<LoaiVatTuDTO>(sql, new { MaLoai });
        }

        public bool CreateOrUpdate(LoaiVatTuDTO obj)
        {
            string sql = @"INSERT INTO [LoaiVatTu] ([MaLoai], [TenLoai], [MaNhom]) 
	VALUES (@MaLoai, @TenLoai, @MaNhom)";
            if (Get(obj.MaLoai) != null)
                sql = @"UPDATE [LoaiVatTu]
	SET    [TenLoai] = @TenLoai, [MaNhom] = @MaNhom
	WHERE  [MaLoai] = @MaLoai";
            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool Delete(string MaLoai)
        {
            string sql = @"DELETE
	FROM   [LoaiVatTu]
	WHERE  [MaLoai] = @MaLoai";

            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaLoai }) > 0;
            }
            catch (Exception)
            {
                return false;
                ;
            }
        }

        public string NewId(string prefix)
        {
            List<LoaiVatTuDTO> items = Get();

            int maxId = 0;

            foreach (var item in items)
            {
                if (item.MaLoai.StartsWith(prefix))
                {
                    if (int.TryParse(item.MaLoai.Substring(prefix.Length), out int _currentId))
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
