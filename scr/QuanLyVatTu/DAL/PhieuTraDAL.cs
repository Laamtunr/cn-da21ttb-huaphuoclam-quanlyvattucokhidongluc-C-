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

    public class PhieuTraDAL
    {
        public List<PhieuTraDTO> Get()
        {
            string sql = @"SELECT [MaPT], PhieuTra.[MaPM], MaSinhVien, SoCanCuoc, HoTenNguoiMuon
	, TenLop, TenHK, TenNH, PhieuTra.[NgayLap], PhieuTra.[MaNV], HoTen, [GhiChu] 
                       FROM [PhieuTra]
LEFT JOIN PhieuMuon ON PhieuMuon.MaPM = PhieuTra.MaPM
LEFT JOIN Lop ON Lop.MaLop = PhieuMuon.MaLop
LEFT JOIN HocKy ON HocKy.MaHK = PhieuMuon.MaHK
LEFT JOIN NamHoc ON NamHoc.MaNH = PhieuMuon.MaNH
LEFT JOIN NhanVien ON NhanVien.MaNV = PhieuMuon.MaNV";

            return DapperDbContext.Connection.Query<PhieuTraDTO>(sql).ToList();
        }

        public PhieuTraDTO Get(string MaPT)
        {
            string sql = @"SELECT [MaPT], PhieuTra.[MaPM], MaSinhVien, SoCanCuoc, HoTenNguoiMuon
	, TenLop, TenHK, TenNH, PhieuTra.[NgayLap], PhieuTra.[MaNV], HoTen, [GhiChu] 
                       FROM [PhieuTra]
LEFT JOIN PhieuMuon ON PhieuMuon.MaPM = PhieuTra.MaPM
LEFT JOIN Lop ON Lop.MaLop = PhieuMuon.MaLop
LEFT JOIN HocKy ON HocKy.MaHK = PhieuMuon.MaHK
LEFT JOIN NamHoc ON NamHoc.MaNH = PhieuMuon.MaNH
LEFT JOIN NhanVien ON NhanVien.MaNV = PhieuMuon.MaNV 
                       WHERE ([MaPT] = @MaPT OR @MaPT IS NULL)";

            return DapperDbContext.Connection.QueryFirstOrDefault<PhieuTraDTO>(sql, new { MaPT });
        }

        public bool CreateOrUpdate(PhieuTraDTO obj)
        {
            string sql = @"INSERT INTO [PhieuTra] ([MaPT], [MaPM], [NgayLap], [MaNV], [GhiChu]) 
                       VALUES (@MaPT, @MaPM, @NgayLap, @MaNV, @GhiChu)";

            if (Get(obj.MaPT) != null)
            {
                sql = @"UPDATE [PhieuTra] 
                    SET [MaPM] = @MaPM, [NgayLap] = @NgayLap, [MaNV] = @MaNV, [GhiChu] = @GhiChu 
                    WHERE [MaPT] = @MaPT";
            }

            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool Delete(string MaPT)
        {
            string sql = @"DELETE FROM [PhieuTra] 
                       WHERE [MaPT] = @MaPT";

            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaPT }) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string NewId(string prefix)
        {
            List<PhieuTraDTO> items = Get();

            int maxId = 0;

            foreach (var item in items)
            {
                if (item.MaPT.StartsWith(prefix))
                {
                    if (int.TryParse(item.MaPT.Substring(prefix.Length), out int _currentId))
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
