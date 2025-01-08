using Dapper;
using QuanLyMuonTraVatTu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    public class NhanVienDAL
    {
        public List<NhanVienDTO> Get()
        {
            string sql = @"SELECT [MaNV], [HoTen], [NgaySinh], [GioiTinh], [SDT], [Email], [DiaChi] 
	FROM   [NhanVien]";

            return DapperDbContext.Connection.Query<NhanVienDTO>(sql)
                .ToList();
        }

        public NhanVienDTO Get(string MaNV)
        {
            string sql = @"SELECT [MaNV], [HoTen], [NgaySinh], [GioiTinh], [SDT], [Email], [DiaChi] 
	FROM   [NhanVien] 
	WHERE  ([MaNV] = @MaNV OR @MaNV IS NULL) ";

            return DapperDbContext.Connection.QueryFirstOrDefault<NhanVienDTO>(sql
                , new { MaNV });
        }

        public bool CreateOrUpdate(NhanVienDTO obj)
        {
            string sql = @"INSERT INTO [NhanVien] ([MaNV], [HoTen], [NgaySinh], [GioiTinh], [SDT], [Email], [DiaChi]) 
	VALUES (@MaNV, @HoTen, @NgaySinh, @GioiTinh, @SDT, @Email, @DiaChi)";

            if (Get(obj.MaNV) != null)
                sql = @"UPDATE [NhanVien]
	SET    [HoTen] = @HoTen, [NgaySinh] = @NgaySinh, [GioiTinh] = @GioiTinh, [SDT] = @SDT, [Email] = @Email, [DiaChi] = @DiaChi
	WHERE  [MaNV] = @MaNV";

            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool Delete(string MaNV)
        {
            string sql = @"DELETE
	FROM   [NhanVien]
	WHERE  [MaNV] = @MaNV";

            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaNV }) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string NewId(string prefix)
        {
            List<NhanVienDTO> items = Get();

            int maxId = 0;

            foreach (var item in items)
            {
                if (item.MaNV.StartsWith(prefix))
                {
                    if (int.TryParse(item.MaNV.Substring(prefix.Length), out int _currentId))
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
