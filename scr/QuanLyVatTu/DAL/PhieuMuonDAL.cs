using Dapper;
using QuanLyMuonTraVatTu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    public class PhieuMuonDAL
    {
        public List<PhieuMuonDTO> Get()
        {
            string sql = @"SELECT PhieuMuon.[MaPM], [NgayLap], [MaSinhVien], [SoCanCuoc], [HoTenNguoiMuon], 
                              PhieuMuon.[MaLop], TenLop, PhieuMuon.[MaHK], TenHK
							  , PhieuMuon.[MaNH], TenNH, [HanTra], [TinhTrang]
							  , PhieuMuon.[MaNV], HoTen FROM PhieuMuon
LEFT JOIN LOP ON Lop.MaLop = PhieuMuon.MaLop
LEFT JOIN HocKy ON HocKy.MaHK = PhieuMuon.MaHK
LEFT JOIN NamHoc ON NamHoc.MaNH = PhieuMuon.MaNH
LEFT JOIN NhanVien ON NhanVien.MaNV = PhieuMuon.MaNV";
            return DapperDbContext.Connection.Query<PhieuMuonDTO>(sql).ToList();
        }

        public PhieuMuonDTO Get(string MaPM)
        {
            string sql = @"SELECT PhieuMuon.[MaPM], [NgayLap], [MaSinhVien], [SoCanCuoc], [HoTenNguoiMuon], 
                              PhieuMuon.[MaLop], TenLop, PhieuMuon.[MaHK], TenHK
							  , PhieuMuon.[MaNH], TenNH, [HanTra], [TinhTrang]
							  , PhieuMuon.[MaNV], HoTen FROM PhieuMuon
LEFT JOIN LOP ON Lop.MaLop = PhieuMuon.MaLop
LEFT JOIN HocKy ON HocKy.MaHK = PhieuMuon.MaHK
LEFT JOIN NamHoc ON NamHoc.MaNH = PhieuMuon.MaNH
LEFT JOIN NhanVien ON NhanVien.MaNV = PhieuMuon.MaNV 
                       WHERE PhieuMuon.[MaPM] = @MaPM";
            return DapperDbContext.Connection.QueryFirstOrDefault<PhieuMuonDTO>(sql, new { MaPM });
        }

        public bool CreateOrUpdate(PhieuMuonDTO obj)
        {
            string sql = @"INSERT INTO [PhieuMuon] ([MaPM], [NgayLap], [MaSinhVien], [SoCanCuoc], [HoTenNguoiMuon], 
                                                 [MaLop], [MaHK], [MaNH], [HanTra], [TinhTrang], [MaNV]) 
                       VALUES (@MaPM, @NgayLap, @MaSinhVien, @SoCanCuoc, @HoTenNguoiMuon, 
                               @MaLop, @MaHK, @MaNH, @HanTra, @TinhTrang, @MaNV)";
            if (Get(obj.MaPM) != null)
                sql = @"UPDATE [PhieuMuon] 
                    SET [NgayLap] = @NgayLap, [MaSinhVien] = @MaSinhVien, [SoCanCuoc] = @SoCanCuoc, 
                        [HoTenNguoiMuon] = @HoTenNguoiMuon, [MaLop] = @MaLop, [MaHK] = @MaHK, 
                        [MaNH] = @MaNH, [HanTra] = @HanTra, [TinhTrang] = @TinhTrang, [MaNV] = @MaNV 
                    WHERE [MaPM] = @MaPM";
            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool Delete(string MaPM)
        {
            string sql = @"DELETE FROM [PhieuMuon] WHERE [MaPM] = @MaPM";
            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaPM }) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string NewId(string prefix)
        {
            List<PhieuMuonDTO> items = Get();

            int maxId = 0;

            foreach (var item in items)
            {
                if (item.MaPM.StartsWith(prefix))
                {
                    if (int.TryParse(item.MaPM.Substring(prefix.Length), out int _currentId))
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

        public List<PhieuMuonDTO> GetDSPhieuMuonChuaTra()
        {
            string sql = @"SELECT PhieuMuon.[MaPM], [NgayLap], [MaSinhVien], [SoCanCuoc], [HoTenNguoiMuon], 
                              PhieuMuon.[MaLop], TenLop, PhieuMuon.[MaHK], TenHK
							  , PhieuMuon.[MaNH], TenNH, [HanTra], [TinhTrang]
							  , PhieuMuon.[MaNV], HoTen FROM PhieuMuon
LEFT JOIN (SELECT DISTINCT MAPM FROM PhieuTra) T ON T.MaPM = PhieuMuon.MaPM
LEFT JOIN LOP ON Lop.MaLop = PhieuMuon.MaLop
LEFT JOIN HocKy ON HocKy.MaHK = PhieuMuon.MaHK
LEFT JOIN NamHoc ON NamHoc.MaNH = PhieuMuon.MaNH
LEFT JOIN NhanVien ON NhanVien.MaNV = PhieuMuon.MaNV
WHERE T.MaPM IS NULL";
            return DapperDbContext.Connection.Query<PhieuMuonDTO>(sql).ToList();
        }
    }
}
