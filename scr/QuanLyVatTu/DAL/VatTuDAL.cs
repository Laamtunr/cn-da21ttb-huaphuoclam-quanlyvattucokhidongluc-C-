using Dapper;
using QuanLyMuonTraVatTu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    public class VatTuDAL
    {
        public List<VatTuDTO> Get()
        {
            string sql = @"SELECT [MaVT], [TenVT], [MoTa], VatTu.[MaLoai], TenLoai
	, VatTu.[MaDVT], TenDVT, [TinhTrang], 
                   [TonKho], [TonKhoToiThieu] FROM [VatTu]
	LEFT JOIN LoaiVatTu ON LoaiVatTu.MaLoai = VatTu.MaLoai
	LEFT JOIN DonViTinh ON DonViTinh.MaDVT = VatTu.MaDVT";
            return DapperDbContext.Connection.Query<VatTuDTO>(sql).ToList();
        }

        public VatTuDTO Get(string MaVT)
        {
            string sql = @"SELECT [MaVT], [TenVT], [MoTa], VatTu.[MaLoai], TenLoai
	, VatTu.[MaDVT], TenDVT, [TinhTrang], 
                   [TonKho], [TonKhoToiThieu] FROM [VatTu]
	LEFT JOIN LoaiVatTu ON LoaiVatTu.MaLoai = VatTu.MaLoai
	LEFT JOIN DonViTinh ON DonViTinh.MaDVT = VatTu.MaDVT 
                       WHERE [MaVT] = @MaVT";
            return DapperDbContext.Connection.QueryFirstOrDefault<VatTuDTO>(sql, new { MaVT });
        }

        public bool CreateOrUpdate(VatTuDTO obj)
        {
            string sql = @"INSERT INTO [VatTu] ([MaVT], [TenVT], [MoTa], [MaLoai], [MaDVT], 
                                            [TinhTrang], [TonKho], [TonKhoToiThieu]) 
                       VALUES (@MaVT, @TenVT, @MoTa, @MaLoai, @MaDVT, @TinhTrang, @TonKho, @TonKhoToiThieu)";
            if (Get(obj.MaVT) != null)
                sql = @"UPDATE [VatTu] 
                    SET [TenVT] = @TenVT, [MoTa] = @MoTa, [MaLoai] = @MaLoai, 
                        [MaDVT] = @MaDVT, [TinhTrang] = @TinhTrang, 
                        [TonKho] = @TonKho, [TonKhoToiThieu] = @TonKhoToiThieu 
                    WHERE [MaVT] = @MaVT";
            return DapperDbContext.Connection.Execute(sql, obj) > 0;
        }

        public bool Delete(string MaVT)
        {
            string sql = @"DELETE FROM [VatTu] WHERE [MaVT] = @MaVT";
            try
            {
                return DapperDbContext.Connection.Execute(sql, new { MaVT }) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string NewId(string prefix)
        {
            List<VatTuDTO> items = Get();

            int maxId = 0;

            foreach (var item in items)
            {
                if (item.MaVT.StartsWith(prefix))
                {
                    if (int.TryParse(item.MaVT.Substring(prefix.Length), out int _currentId))
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
