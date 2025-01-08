using Dapper;
using QuanLyMuonTraVatTu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    public class TaiKhoanDAL
    {
        public List<TaiKhoanDTO> Get()
        {
            string sql = @"SELECT [MaTK], [MatKhau], [Quyen], TaiKhoan.[MaNV], HoTen 
	FROM   [TaiKhoan]
LEFT JOIN NhanVien ON NhanVien.MaNV = TaiKhoan.MaNV";

            return DapperDbContext.Connection.Query<TaiKhoanDTO>(sql)
                .ToList();
        }

        public TaiKhoanDTO Get(string MaTK)
        {
            string sql = @"SELECT [MaTK], [MatKhau], [Quyen], TaiKhoan.[MaNV], HoTen 
	FROM   [TaiKhoan]
LEFT JOIN NhanVien ON NhanVien.MaNV = TaiKhoan.MaNV 
	WHERE  ([MaTK] = @MaTK OR @MaTK IS NULL)";

            return DapperDbContext.Connection.QueryFirstOrDefault<TaiKhoanDTO>(sql
                , new { MaTK });
        }

        public bool CreateOrUpdate(TaiKhoanDTO obj)
        {
            string sql = @"INSERT INTO [TaiKhoan] ([MaTK], [MatKhau], [Quyen], [MaNV]) 
	VALUES (@MaTK, @MatKhau, @Quyen, @MaNV)";

            if (Get(obj.MaTK) != null)
                sql = @"UPDATE [TaiKhoan]
	SET    [MatKhau] = @MatKhau, [Quyen] = @Quyen, [MaNV] = @MaNV
	WHERE  [MaTK] = @MaTK";

            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool Delete(string MaTK)
        {
            string sql = @"DELETE
	FROM   [TaiKhoan]
	WHERE  [MaTK] = @MaTK";

            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaTK }) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
