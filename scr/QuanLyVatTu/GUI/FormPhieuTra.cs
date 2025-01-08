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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyMuonTraVatTu.GUI
{
    public partial class FormPhieuTra : Form
    {
        public FormPhieuTra()
        {
            InitializeComponent();
        }

        public FormPhieuTra(PhieuTraDTO obj)
            : this()
        {
            Obj = obj;
        }

        PhieuTraDAL _dal = new PhieuTraDAL();
        CTPhieuTraDAL _dalCTPhieuTra = new CTPhieuTraDAL();
        CTPhieuMuonDAL _dalCTPhieuMuon = new CTPhieuMuonDAL();
        NhanVienDAL _dalNhanVien = new NhanVienDAL();
        VatTuDAL _vatTuDAL = new VatTuDAL();

        BindingSource _src = new BindingSource();

        public PhieuTraDTO Obj { get; }

        private void FormPhieuTra_Load(object sender, EventArgs e)
        {
            gridData.ReadOnly = true;
            gridData.AutoGenerateColumns = true;
            gridData.AllowUserToAddRows = false;
            gridData.DataSource = _src;
            LoadComboNhanVien();
            if (Obj != null)
                HienThi(Obj);
            else
            {
                HienThi(new PhieuTraDTO()
                {
                    MaPT = _dal.NewId("PT")
                });

                if (!string.IsNullOrEmpty(FormMain.User.MaNV))
                {
                    cboMaNV.SelectedValue = FormMain.User.MaNV;
                }
            } 
                
        }

        private void LoadComboNhanVien()
        {
            cboMaNV.DataSource = _dalNhanVien.Get();
            cboMaNV.ValueMember = "MaNV";
            cboMaNV.DisplayMember = "HoTen";
            if (cboMaNV.Items.Count > 0)
                cboMaNV.SelectedIndex = 0;
        }

        private void HienThi(PhieuTraDTO obj)
        {
            txtMaPT.Text = obj.MaPT;
            txtMaPM.Text = obj.MaPM;
            
            if (!string.IsNullOrEmpty(obj.MaNV))
                cboMaNV.SelectedValue = obj.MaNV;
            else
                cboMaNV.SelectedIndex = 0;
            txtGhiChu.Text = obj.GhiChu;
            _src.DataSource = _dalCTPhieuTra.GetBy(obj.MaPT);
            _src.ResetBindings(true);
        }

        private void btnChonMaPM_Click(object sender, EventArgs e)
        {
            FormChonPhieuMuonChuaTra f = new FormChonPhieuMuonChuaTra();
            f.ChonPhieuMuonHandler += F_ChonPhieuMuonHandler;
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
        }

        private void F_ChonPhieuMuonHandler(DTO.PhieuMuonDTO obj)
        {
            txtMaPM.Text = obj.MaPM;

            var dsCTPT = new List<CTPhieuTraDTO>();

            var dsCTPM = _dalCTPhieuMuon.GetBy(obj.MaPM);

            foreach (var pm in dsCTPM)
            {
                CTPhieuTraDTO pt = new CTPhieuTraDTO();
                pt.MaPT = txtMaPT.Text;
                pt.MaVT = pm.MaVT;
                pt.TenVT = pm.TenVT;
                pt.SoLuong = pm.SoLuong;
                pt.TinhTrang = "Bình Thường";
                dsCTPT.Add(pt);
            }

            _src.DataSource = dsCTPT;
            _src.ResetBindings(true);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            HienThi(new PhieuTraDTO()
            {
                MaPT = _dal.NewId("PT")
            });

            if (!string.IsNullOrEmpty(FormMain.User.MaNV))
            {
                cboMaNV.SelectedValue = FormMain.User.MaNV;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {

            var dsCT = _src.DataSource as List<CTPhieuTraDTO>;

            if (dsCT == null)
                dsCT = new List<CTPhieuTraDTO>();

            if (dsCT.Count <= 0)
            {
                HopThoai.BaoLoi("Không có vật tư nào trong danh sách chi tiết !");
                return;
            } 

            if (txtMaPT.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã phiếu trả không được để trống !");
                return;
            } 

            var pt = _dal.Get(txtMaPT.Text);

            if (pt != null)
            {
                HopThoai.BaoLoi("Mã phiếu trả đã tồn tại !");
                return;
            }

            if (txtMaPM.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Vui lòng chọn phiếu mượn !");
                return;
            }

            if (cboMaNV.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn nhân viên lập !");
                return;
            }

            PhieuTraDTO obj = new PhieuTraDTO();
            obj.MaPT = txtMaPT.Text;
            obj.NgayLap = dtNgayLap.Value;
            obj.MaPM = txtMaPM.Text;
            obj.MaNV = cboMaNV.SelectedValue.ToString();
            obj.GhiChu = txtGhiChu.Text;

            if (_dal.CreateOrUpdate(obj))
            {
                // Thêm chi tiết
                foreach (var ct in dsCT)
                {
                    ct.MaPT = obj.MaPT;
                    if (_dalCTPhieuTra.CreateOrUpdate(ct))
                    {
                        // Cộng vào tồn 
                        var vt = _vatTuDAL.Get(ct.MaVT);

                        if (vt != null)
                        {
                            vt.TonKho += ct.SoLuong;
                            // Cập nhật lại số lượng
                            _vatTuDAL.CreateOrUpdate(vt);
                        }
                    } 
                }

                HopThoai.ThongBao("Thêm mới phiếu trả thành công !");
                return;
            }

            HopThoai.BaoLoi("Thêm mới phiếu trả không thành công !");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            var dsCT = _src.DataSource as List<CTPhieuTraDTO>;

            if (dsCT == null)
                dsCT = new List<CTPhieuTraDTO>();

            if (dsCT.Count <= 0)
            {
                HopThoai.BaoLoi("Không có vật tư nào trong danh sách chi tiết !");
                return;
            }

            if (txtMaPT.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã phiếu trả không được để trống !");
                return;
            }

            var pt = _dal.Get(txtMaPT.Text);

            if (pt == null)
            {
                HopThoai.BaoLoi("Không tìm thấy phiếu trả cần sửa !");
                return;
            }

            if (txtMaPM.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Vui lòng chọn phiếu mượn !");
                return;
            }

            if (cboMaNV.SelectedIndex < 0)
            {
                HopThoai.BaoLoi("Vui lòng chọn nhân viên lập !");
                return;
            }

            PhieuTraDTO obj = new PhieuTraDTO();
            obj.MaPT = txtMaPT.Text;
            obj.NgayLap = dtNgayLap.Value;
            obj.MaPM = txtMaPM.Text;
            obj.MaNV = cboMaNV.SelectedValue.ToString();
            obj.GhiChu = txtGhiChu.Text;

            // Xoá chi tiết có mã phiếu nhập được cập nhật
            var dsCTPhieuTra = _dalCTPhieuTra.GetBy(txtMaPT.Text);
            // Xoá chi tiết và trừ số tồn ở vật tư
            if (_dalCTPhieuTra.DeleteBy(txtMaPT.Text))
            {
                for (int i = 0; i < dsCTPhieuTra.Count; i++)
                {
                    var vt = _vatTuDAL.Get(dsCTPhieuTra[i].MaVT);

                    if (vt != null)
                    {
                        vt.TonKho -= dsCTPhieuTra[i].SoLuong;
                        // Cập nhật lại số lượng
                        _vatTuDAL.CreateOrUpdate(vt);
                    }
                }
            }

            if (_dal.CreateOrUpdate(obj))
            {
                // Thêm chi tiết
                foreach (var ct in dsCT)
                {
                    ct.MaPT = obj.MaPT;
                    if (_dalCTPhieuTra.CreateOrUpdate(ct))
                    {
                        // Cộng vào tồn 
                        var vt = _vatTuDAL.Get(ct.MaVT);

                        if (vt != null)
                        {
                            vt.TonKho += ct.SoLuong;
                            // Cập nhật lại số lượng
                            _vatTuDAL.CreateOrUpdate(vt);
                        }
                    }
                }

                HopThoai.ThongBao("Cập nhật phiếu trả thành công !");
                return;
            }

            HopThoai.BaoLoi("Cập nhật phiếu trả không thành công !");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaPT.Text.IsEmpty())
            {
                HopThoai.BaoLoi("Mã phiếu trả không được để trống !");
                return;
            }

            var pt = _dal.Get(txtMaPT.Text);

            if (pt == null)
            {
                HopThoai.BaoLoi("Không tìm thấy phiếu trả cần xoá !");
                return;
            }

            var answer = HopThoai.XacNhan("Bạn muốn xoá phiếu trả được chọn ?");

            if (!answer)
                return;

            var dsCTPT = _dalCTPhieuTra.GetBy(pt.MaPT);

            if (_dalCTPhieuTra.DeleteBy(pt.MaPT))
            {
                // Cập nhật lại tồn phiếu trả
                // TODO :
                for (int i = 0; i < dsCTPT.Count; i++)
                {
                    var vt = _vatTuDAL.Get(dsCTPT[i].MaVT);

                    if (vt != null)
                    {
                        vt.TonKho -= dsCTPT[i].SoLuong;
                        // Cập nhật lại số lượng
                        _vatTuDAL.CreateOrUpdate(vt);
                    }
                }
            }

            if (_dal.Delete(pt.MaPT))
            {
                HopThoai.ThongBao("Xoá thông tin phiếu trả thành công !");
                return;
            }
            
            HopThoai.BaoLoi("Xoá thông tin phiếu trả không thành công !");
        }
    }
}
