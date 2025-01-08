using GUI.Helpers;
using QuanLyMuonTraVatTu.DAL;
using QuanLyMuonTraVatTu.DTO;
using QuanLyMuonTraVatTu.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyMuonTraVatTu.GUI
{
    public partial class FormPhieuMuon : Form
    {
        public FormPhieuMuon()
        {
            InitializeComponent();
        }

        public FormPhieuMuon(PhieuMuonDTO obj)
            : this()
        {
            Obj = obj;
        }

        PhieuMuonDAL _dal = new PhieuMuonDAL();
        CTPhieuMuonDAL _dalCTPhieuMuon = new CTPhieuMuonDAL();
        LopDAL _dalLop = new LopDAL();
        NamHocDAL _dalNamHoc = new NamHocDAL();
        HocKyDAL _dalHocKy = new HocKyDAL();
        NhanVienDAL _dalNhanVien = new NhanVienDAL();
        VatTuDAL _dalVatTu = new VatTuDAL();

        BindingSource _src = new BindingSource();

        public PhieuMuonDTO Obj { get; }

        private void FormPhieuMuon_Load(object sender, EventArgs e)
        {
            gridData.AutoGenerateColumns = true;
            gridData.ReadOnly = true;
            gridData.AllowUserToAddRows = false;
            gridData.DataSource = _src;
            LoadComboLop();
            LoadComboNamHoc();
            LoadComboNhanVien();
            LoadComboVatTu();
            LoadGridData();

            if (Obj != null)
                HienThi(Obj);
            else
                btnLamMoi_Click(sender, e);
        }

        private void LoadComboVatTu()
        {
            cboMaVT.DataSource = _dalVatTu.Get();
            cboMaVT.ValueMember = "MaVT";
            cboMaVT.DisplayMember = "TenVT";
            if (cboMaVT.Items.Count > 0)
                cboMaVT.SelectedIndex = 0;
        }

        private void LoadComboNhanVien()
        {
            cboMaNV.DataSource = _dalNhanVien.Get();
            cboMaNV.DisplayMember = "HoTen";
            cboMaNV.ValueMember = "MaNV";
            if (cboMaNV.Items.Count > 0)
            {
                cboMaNV.SelectedIndex = 0;
            }
        }

        private void LoadComboHocKy(string MaNH)
        {
            cboMaHK.DataSource = _dalHocKy.Get()
                .Where(x => x.MaNH == MaNH).ToList();
            cboMaHK.ValueMember = "MaHK";
            cboMaHK.DisplayMember = "TenHK";
            if (cboMaHK.Items.Count > 0)
            {
                cboMaHK.SelectedIndex = 0;
            }
        }

        private void LoadComboNamHoc()
        {
            cboMaNH.DataSource = _dalNamHoc.Get();
            cboMaNH.ValueMember = "MaNH";
            cboMaNH.DisplayMember = "TenNH";
            if (cboMaNH.Items.Count > 0)
            {
                cboMaNH.SelectedIndex = 0;
            }
        }

        private void LoadComboLop()
        {
            cboMaLop.DataSource = _dalLop.Get();
            cboMaLop.ValueMember = "MaLop";
            cboMaLop.DisplayMember = "TenLop";
            if (cboMaLop.Items.Count > 0)
            {
                cboMaLop.SelectedIndex = 0;
            }
        }

        private void LoadGridData()
        {
            _src.DataSource = new List<CTPhieuMuonDTO>();
            _src.ResetBindings(true);
        }

        private void cboMaNH_DropDownClosed(object sender, EventArgs e)
        {
            var obj = cboMaNH.SelectedItem as NamHocDTO;

            if (obj == null)
                return;

            LoadComboHocKy(obj.MaNH);
        }

        private void HienThi(PhieuMuonDTO obj)
        {
            txtMaPM.Text = obj.MaPM;
            dtNgayLap.Value = obj.NgayLap ?? DateTime.Now;
            txtMaSV.Text = obj.MaSinhVien;
            txtSoCanCuoc.Text = obj.SoCanCuoc;
            txtHoTenNguoiMuon.Text = obj.HoTenNguoiMuon;
            if (!string.IsNullOrEmpty(obj.MaLop))
                cboMaLop.SelectedValue = obj.MaLop;
            else
                cboMaLop.SelectedIndex = 0;
            
            if (!string.IsNullOrEmpty(obj.MaNH))
                cboMaNH.SelectedValue = obj.MaNH;
            else
                cboMaNH.SelectedIndex = 0;

            LoadComboHocKy(cboMaNH.SelectedValue.ToString());

            if (!string.IsNullOrEmpty(obj.MaHK))
            {
                cboMaHK.SelectedValue = obj.MaHK;
            }

            dtHanTra.Value = obj.HanTra ?? DateTime.Now;

            if (!string.IsNullOrEmpty(obj.TinhTrang))
                cboTinhTrang.SelectedItem = obj.TinhTrang;
            else
                cboTinhTrang.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(obj.MaNV))
                cboMaNV.SelectedValue = obj.MaNV;
            else
                cboMaNV.SelectedIndex = 0;

            cboMaVT.SelectedIndex = 0;
            txtSoLuong.Value = 0;

            _src.DataSource = _dalCTPhieuMuon.GetBy(obj.MaPM);
            _src.ResetBindings(true);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            HienThi(new PhieuMuonDTO()
            {
                MaPM = _dal.NewId("PM")
            });

            if (!string.IsNullOrEmpty(FormMain.User.MaNV))
            {
                cboMaNV.SelectedValue = FormMain.User.MaNV;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {

            var dsCT = _src.DataSource as List<CTPhieuMuonDTO>;

            if (dsCT == null)
                dsCT = new List<CTPhieuMuonDTO>();

            if (dsCT.Count <= 0)
            {
                HopThoai.BaoLoi("Không có bản ghi nào trong danh sách chi tiết !");
                return;
            } 

            if (txtMaPM.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã phiếu mượn không được để trống !");
                return;
            }

            var pm = _dal.Get(txtMaPM.Text);

            if (pm != null)
            {
                HopThoai.BaoLoi("Mã phiếu mượn đã tồn tại !");
                return;
            }
            
            if (txtMaSV.Text.IsEmpty() && txtSoCanCuoc.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Bạn phải nhập vào mã số sinh viên hoặc số thẻ căn cước !");
                return;
            } 

            if (txtHoTenNguoiMuon.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Họ tên người mượn không được để trống !");
                return;
            } 

            if (cboMaLop.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn lớp !");
                return;
            } 

            if (cboMaNH.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn năm học !");
                return;
            } 

            if (cboMaHK.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn học kỳ !");
                return;
            }

            if (dtHanTra.Value < dtNgayLap.Value)
            {
                HopThoai.BaoLoi("Hạn trả phải lớn hơn ngày lập !");
                return;
            }

            if (cboMaNV.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn nhân viên lập !");
                return;
            }

            PhieuMuonDTO obj = new PhieuMuonDTO();
            obj.MaPM = txtMaPM.Text;
            obj.NgayLap = dtNgayLap.Value;
            obj.MaSinhVien = txtMaSV.Text;
            obj.SoCanCuoc = txtSoCanCuoc.Text;
            obj.HoTenNguoiMuon = txtHoTenNguoiMuon.Text;
            obj.MaLop = cboMaLop.SelectedValue.ToString();
            obj.MaNH = cboMaNH.SelectedValue.ToString();
            obj.MaHK = cboMaHK.SelectedValue.ToString();
            obj.HanTra = dtHanTra.Value;
            obj.TinhTrang = cboTinhTrang.SelectedItem.ToString();
            obj.MaNV = cboMaNV.SelectedValue.ToString();

            if (_dal.CreateOrUpdate(obj))
            {
                // Cập nhật chi tiết phiếu mượn
                foreach (var ct in dsCT)
                {
                    ct.MaPM = obj.MaPM;
                    
                    if (_dalCTPhieuMuon.CreateOrUpdate(ct))
                    {
                        // Trừ tồn
                        var vt = _dalVatTu.Get(ct.MaVT);

                        if (vt != null)
                        {
                            vt.TonKho -= ct.SoLuong;
                            _dalVatTu.CreateOrUpdate(vt);
                        } 
                    }
                }

                HopThoai.ThongBao("Thêm mới phiếu mượn thành công !");
                return;
            }

            HopThoai.BaoLoi("Thêm mới phiếu mượn không thành công !");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            var dsCT = _src.DataSource as List<CTPhieuMuonDTO>;

            if (dsCT == null)
                dsCT = new List<CTPhieuMuonDTO>();

            if (dsCT.Count <= 0)
            {
                HopThoai.BaoLoi("Không có bản ghi nào trong danh sách chi tiết !");
                return;
            }

            if (txtMaPM.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã phiếu mượn không được để trống !");
                return;
            }

            var pm = _dal.Get(txtMaPM.Text);

            if (pm == null)
            {
                HopThoai.BaoLoi("Không tìm thấy phiếu mượn cần sửa !");
                return;
            }

            if (txtMaSV.Text.IsEmpty() && txtSoCanCuoc.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Bạn phải nhập vào mã số sinh viên hoặc số thẻ căn cước !");
                return;
            }

            if (txtHoTenNguoiMuon.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Họ tên người mượn không được để trống !");
                return;
            }

            if (cboMaLop.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn lớp !");
                return;
            }

            if (cboMaNH.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn năm học !");
                return;
            }

            if (cboMaHK.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn học kỳ !");
                return;
            }

            if (dtHanTra.Value < dtNgayLap.Value)
            {
                HopThoai.BaoLoi("Hạn trả phải lớn hơn ngày lập !");
                return;
            }

            if (cboMaNV.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn nhân viên lập !");
                return;
            }

            PhieuMuonDTO obj = new PhieuMuonDTO();
            obj.MaPM = txtMaPM.Text;
            obj.NgayLap = dtNgayLap.Value;
            obj.MaSinhVien = txtMaSV.Text;
            obj.SoCanCuoc = txtSoCanCuoc.Text;
            obj.HoTenNguoiMuon = txtHoTenNguoiMuon.Text;
            obj.MaLop = cboMaLop.SelectedValue.ToString();
            obj.MaNH = cboMaNH.SelectedValue.ToString();
            obj.MaHK = cboMaHK.SelectedValue.ToString();
            obj.HanTra = dtHanTra.Value;
            obj.TinhTrang = cboTinhTrang.SelectedItem.ToString();
            obj.MaNV = cboMaNV.SelectedValue.ToString();

            // Xoá chi tiết phiếu mượn cũ
            // cộng số tồn tương ứng
            var dsCTPM = _dalCTPhieuMuon.GetBy(obj.MaPM);

            if (_dalCTPhieuMuon.DeleteBy(obj.MaPM))
            {
                for (int i = 0; i < dsCTPM.Count; i++)
                {
                    var vt = _dalVatTu.Get(dsCTPM[i].MaVT);
                    
                    if (vt != null)
                    {
                        vt.TonKho += dsCTPM[i].SoLuong;
                        _dalVatTu.CreateOrUpdate(vt);
                    } 
                } 
            } 

            if (_dal.CreateOrUpdate(obj))
            {
                // Cập nhật chi tiết phiếu mượn
                foreach (var ct in dsCT)
                {
                    ct.MaPM = obj.MaPM;

                    if (_dalCTPhieuMuon.CreateOrUpdate(ct))
                    {
                        // Trừ tồn
                        var vt = _dalVatTu.Get(ct.MaVT);

                        if (vt != null)
                        {
                            vt.TonKho -= ct.SoLuong;
                            _dalVatTu.CreateOrUpdate(vt);
                        }
                    }
                }

                HopThoai.ThongBao("Sửa thông tin phiếu mượn thành công !");
                return;
            }

            HopThoai.BaoLoi("Sửa thông tin phiếu mượn không thành công !");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaPM.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã phiếu mượn không được để trống !");
                return;
            }

            var pm = _dal.Get(txtMaPM.Text);

            if (pm == null)
            {
                HopThoai.BaoLoi("Không tìm thấy phiếu mượn cần xoá !");
                return;
            }

            var answer = HopThoai.XacNhan("Bạn muốn xoá phiếu mượn được chọn ?");

            if (!answer)
                return;

            // Xoá chi tiết phiếu mượn
            var dsCTPM = _dalCTPhieuMuon.GetBy(pm.MaPM);

            if (_dalCTPhieuMuon.DeleteBy(pm.MaPM))
            {
                for (int i = 0; i < dsCTPM.Count; i++)
                {
                    var vt = _dalVatTu.Get(dsCTPM[i].MaVT);

                    if (vt != null)
                    {
                        vt.TonKho += dsCTPM[i].SoLuong;
                        _dalVatTu.CreateOrUpdate(vt);
                    }
                }
            }

            // Xoá phiếu mượn
            if (_dal.Delete(pm.MaPM))
            {
                HopThoai.ThongBao("Xoá phiếu mượn thành công !");
                return;
            }

            HopThoai.BaoLoi("Xoá phiếu mượn không thành công !");
        }

        private void btnThemCT_Click(object sender, EventArgs e)
        {
            var dsCT = _src.DataSource as List<CTPhieuMuonDTO>;

            if (dsCT == null)
                dsCT = new List<CTPhieuMuonDTO>();

            if (cboMaVT.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn vật tư !");
                return;
            }

            var vt = cboMaVT.SelectedItem as VatTuDTO;
            
            if (vt == null)
            {
                HopThoai.BaoLoi("Không tìm thấy vật tư !");
                return;
            }
            
            if (txtSoLuong.Value <= 0)
            {
                HopThoai.BaoLoi("Số lượng phải lớn hơn 0 !");
                return;
            }

            bool isUpdate = false;

            foreach (var item in dsCT)
            {
                if (item.MaVT == vt.MaVT) // Nếu tồn tại vật tư
                {
                    isUpdate = true;
                    if (item.SoLuong + txtSoLuong.Value > vt.TonKho)
                    {
                        HopThoai.BaoLoi("Số lượng vượt quá tồn kho !");
                        return;
                    }
                    item.SoLuong += txtSoLuong.Value;
                    break;
                } 
            } 

            if (!isUpdate)
            {
                if (txtSoLuong.Value > vt.TonKho)
                {
                    HopThoai.BaoLoi("Số lượng vượt quá tồn kho !");
                    return;
                }

                CTPhieuMuonDTO obj = new CTPhieuMuonDTO();
                obj.MaVT = vt.MaVT;
                obj.MaPM = txtMaPM.Text;
                obj.TenVT = vt.TenVT;
                obj.SoLuong = txtSoLuong.Value;
                obj.TinhTrang = "OK";
                dsCT.Add(obj);
            }

            _src.DataSource = dsCT;
            _src.ResetBindings(true);
        }

        private void btnXoaCT_Click(object sender, EventArgs e)
        {
            // Lấy danh sách chi tiết hiện tại
            List<CTPhieuMuonDTO> ds = _src.DataSource as List<CTPhieuMuonDTO>;

            if (ds == null)
                ds = new List<CTPhieuMuonDTO>();

            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
                return;

            // Lấy dữ liệu của dòng được chọn
            var ct = gridData.CurrentRow.DataBoundItem as CTPhieuMuonDTO;

            // Xoá dữ liệu trong danh sách
            ds.Remove(ct);
            // Hiển thị lưới ra màn hình
            _src.DataSource = ds;
            _src.ResetBindings(true);
        }
    }
}
