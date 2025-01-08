using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.Model
{
    public class QuyenVM
    {
        public string MaQuyen { get; set; }
        public string TenQuyen { get; set; }

        public static List<QuyenVM> Get()
        {
            return new List<QuyenVM>
            {
                new QuyenVM() { MaQuyen = "SINHVIEN", TenQuyen = "Sinh Viên" },
                new QuyenVM() { MaQuyen = "GIANGVIEN", TenQuyen = "Giảng Viên" },
                new QuyenVM() { MaQuyen = "ADMIN", TenQuyen = "Quản Trị Viên" }
            };
        }
    }
}
