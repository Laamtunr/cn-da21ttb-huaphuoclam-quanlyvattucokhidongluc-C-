using QuanLyMuonTraVatTu.DAL;
using QuanLyMuonTraVatTu.DTO;
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
    public partial class FormPhieuNhap : Form
    {
        public FormPhieuNhap()
        {
            InitializeComponent();
        }

        public FormPhieuNhap(PhieuNhapDTO obj)
            : this()
        {
            Obj = obj;
        }

        // Thiết lập các thông số cho lưới hiển thị
        BindingSource _src = new BindingSource();
        public void KhoiTaoLuoi()
        {
            // Nhận nguồn dữ liệu từ BindingSource
            gridData.DataSource = _src;
            // Tự động tạo cột tương ứng với nguồn dữ liệu
            gridData.AutoGenerateColumns = true;
            // Không cho chỉnh sửa, chi cho đọc
            gridData.ReadOnly = true;
            gridData.AllowUserToAddRows = false;
            gridData.AllowUserToDeleteRows = false;
            gridData.AllowUserToResizeColumns = false;
            gridData.AllowUserToResizeRows = false;
            gridData.AllowUserToOrderColumns = false;
        }

        VatTuDAL _VatTuDAL = new VatTuDAL();
        PhieuNhapDAL _dal = new PhieuNhapDAL();
        NhanVienDAL _dalNhanVien = new NhanVienDAL();
        CTPhieuNhapDAL _ctPhieuNhapDAL = new CTPhieuNhapDAL();

        public PhieuNhapDTO Obj { get; }

        private void LoadGridData()
        {
            // Lấy dữ liệu vào datasource của bindingsource
            _src.DataSource = new List<CTPhieuNhapDTO>();
            // Làm mới hiển thị trên lưới
            _src.ResetBindings(true);
        }

        private void FormPhieuNhap_Load(object sender, EventArgs e)
        {
            KhoiTaoLuoi();
            LoadComboNhanVien();
            LoadComboVatTu();
            LoadGridData();
            // Nếu đối tượng nhập hàng khác rỗng 
            // thì hiển thị thông tin phiếu nhập ra màn hình
            if (Obj != null)
            {
                txtMaPN.Text = Obj.MaPN;
                dtNgayLap.Value = Obj.NgayLap ?? DateTime.Now;
                if (!string.IsNullOrEmpty(Obj.MaNV))
                {
                    cboMaNV.SelectedValue = Obj.MaNV;
                }
                else
                    cboMaNV.SelectedIndex = 0;
                txtGhiChu.Text = Obj.GhiChu;
                // Hiển thị chi tiết phiếu nhập
                _src.DataSource = _ctPhieuNhapDAL.GetBy(Obj.MaPN);
                _src.ResetBindings(true);
            }
            else
            {
                txtMaPN.Text = _dal.NewId("PN");

                if (!string.IsNullOrEmpty(FormMain.User.MaNV))
                {
                    cboMaNV.SelectedValue = FormMain.User.MaNV;
                }
            }
        }

        NhanVienDAL _nhanVienDAL = new NhanVienDAL();
        private void LoadComboNhanVien()
        {
            cboMaNV.DataSource = _nhanVienDAL.Get();
            cboMaNV.ValueMember = "MaNV";
            cboMaNV.DisplayMember = "HoTen";
            if (cboMaNV.Items.Count > 0)
                cboMaNV.SelectedIndex = 0;
        }

        private void LoadComboVatTu()
        {
            cboMaVT.DataSource = _VatTuDAL.Get();
            cboMaVT.ValueMember = "MaVT";
            cboMaVT.DisplayMember = "TenVT";
            if (cboMaVT.Items.Count > 0)
                cboMaVT.SelectedIndex = 0;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaPN.Text = _dal.NewId("PN");
            dtNgayLap.Value = DateTime.Now;
            txtGhiChu.Text = "";
            cboMaVT.SelectedIndex = 0;
            cboMaNV.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(FormMain.User.MaNV))
            {
                cboMaNV.SelectedValue = FormMain.User.MaNV;
            }
            txtSoLuong.Value = 0;
            _src.DataSource = new List<CTPhieuNhapDTO>();
            _src.ResetBindings(true);
        }

        private void btnThemCT_Click(object sender, EventArgs e)
        {
            // Nếu người dùng không chọn vật tư nào thì không thực hiện
            if (cboMaVT.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn vật tư !", "Thông báo");
                return;
            }

            // Nếu số lượng nhỏ hơn hoặc bằng 0 thì hông thực hiện
            if (txtSoLuong.Value <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0 !", "Thông báo");
                return;
            }

            // Lấy ra danh sách chi tiết hiện tại
            List<CTPhieuNhapDTO> ds = _src.DataSource as List<CTPhieuNhapDTO>;

            if (ds == null)
                ds = new List<CTPhieuNhapDTO>();
            // Lấy ra vật tư được chọn
            VatTuDTO sp = cboMaVT.SelectedItem as VatTuDTO;

            if (sp == null)
                return;

            // Cập nhật cộng dồn số lượng nếu đã tồn tại vật tư trong danh sách
            // chi tiết
            bool isUpdate = false;

            for (int i = 0; i < ds.Count; i++)
            {
                if (ds[i].MaVT == sp.MaVT)
                {
                    ds[i].SoLuong += (int)txtSoLuong.Value;
                    isUpdate = true;
                    break;
                }
            }

            // Ngược lại thêm mới chi tiết 
            if (!isUpdate)
            {
                CTPhieuNhapDTO ct = new CTPhieuNhapDTO();
                ct.MaPN = txtMaPN.Text;
                ct.MaVT = sp.MaVT;
                ct.TenVT = sp.TenVT;
                ct.SoLuong = (int)txtSoLuong.Value;
                // Thêm vào chi tiết
                ds.Add(ct);
            }
            // Hiển thị danh sách chi tiết mới ra lưới
            _src.DataSource = ds;
            _src.ResetBindings(true);
        }

        private void btnXoaCT_Click(object sender, EventArgs e)
        {
            // Lấy danh sách chi tiết hiện tại
            List<CTPhieuNhapDTO> ds = _src.DataSource as List<CTPhieuNhapDTO>;

            if (ds == null)
                ds = new List<CTPhieuNhapDTO>();

            if (gridData.CurrentRow == null || gridData.CurrentRow.IsNewRow)
                return;
            // Lấy dữ liệu của dòng được chọn
            var ct = gridData.CurrentRow.DataBoundItem as CTPhieuNhapDTO;

            // Xoá dữ liệu trong danh sách
            ds.Remove(ct);
            // Hiển thị lưới ra màn hình
            _src.DataSource = ds;
            _src.ResetBindings(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            List<CTPhieuNhapDTO> ds = _src.DataSource as List<CTPhieuNhapDTO>;

            if (ds == null)
                ds = new List<CTPhieuNhapDTO>();

            if (ds.Count <= 0)
            {
                MessageBox.Show("Không có vật tư nào trong phiếu nhập !");
                return;
            }

            if (string.IsNullOrEmpty(txtMaPN.Text))
            {
                MessageBox.Show("Mã phiếu nhập không được để trống !");
                return;
            }

            var pn = _dal.Get(txtMaPN.Text);

            if (pn == null)
            {
                MessageBox.Show("Phiếu nhập cần sửa không tìm thấy !", "Thông báo");
                return;
            }

          
            if (cboMaNV.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên lập !");
                return;
            }

            // Tạo đối tượng và gán các giá trị được nhập
            PhieuNhapDTO obj = new PhieuNhapDTO();
            obj.MaPN = txtMaPN.Text;
            obj.NgayLap = dtNgayLap.Value;
            obj.GhiChu = txtGhiChu.Text;
            obj.MaNV = cboMaNV.SelectedValue.ToString();
            // Thực hiện cập nhật
            if (_dal.CreateOrUpdate(obj))
            {
                // Xoá chi tiết có mã phiếu nhập được cập nhật
                var dsCTPhieuNhap = _ctPhieuNhapDAL.GetBy(txtMaPN.Text);

                // Xoá chi tiết và trừ số tồn ở vật tư
                if (_ctPhieuNhapDAL.DeleteBy(txtMaPN.Text))
                {
                    for (int i = 0; i < dsCTPhieuNhap.Count; i++)
                    {
                        var vt = _VatTuDAL.Get(dsCTPhieuNhap[i].MaVT);

                        if (vt != null)
                        {
                            vt.TonKho -= dsCTPhieuNhap[i].SoLuong;
                            // Cập nhật lại số lượng
                            _VatTuDAL.CreateOrUpdate(vt);
                        }
                    }
                }

                // Thêm mới chi tiết từ danh sách chi tiết
                foreach (var ct in ds)
                {
                    ct.MaPN = obj.MaPN;
                    if (_ctPhieuNhapDAL.CreateOrUpdate(ct))
                    {
                        var vt = _VatTuDAL.Get(ct.MaVT);

                        if (vt != null)
                        {
                            vt.TonKho += ct.SoLuong;
                            // Cập nhật lại số lượng
                            _VatTuDAL.CreateOrUpdate(vt);
                        }
                    }
                }

                MessageBox.Show("Cập nhật phiếu nhập thành công !");
                LoadGridData();
               // this.Close();
                return;
            }

            MessageBox.Show("Cập nhật phiếu nhập không thành công !");

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPN.Text))
            {
                MessageBox.Show("Mã phiếu nhập không được để trống !");
                return;
            }
            // Hỏi người dùng có muốn xoá
            var q = MessageBox.Show("Bạn muốn xoá phiếu nhập được chọn ?", "Thông báo"
                , MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

            // Nếu người dùng đồng ý xoá phiếu nhập
            if (q)
            {
                var dsCTPhieuNhap = _ctPhieuNhapDAL.GetBy(txtMaPN.Text);
                
                // Xoá chi tiết và trừ số tồn ở vật tư
                if (_ctPhieuNhapDAL.DeleteBy(txtMaPN.Text))
                {
                    for (int i = 0; i < dsCTPhieuNhap.Count; i++)
                    {
                        var vt = _VatTuDAL.Get(dsCTPhieuNhap[i].MaVT);
                        
                        if (vt != null)
                        {
                            vt.TonKho -= dsCTPhieuNhap[i].SoLuong;
                            // Cập nhật lại số lượng
                            _VatTuDAL.CreateOrUpdate(vt);
                        } 
                    }
                } 

                if (_dal.Delete(txtMaPN.Text))
                {
                    MessageBox.Show("Xoá thông tin phiếu nhập thành công !");
                    btnLamMoi_Click(sender, e);
                    //this.Close();
                    return;
                }

                MessageBox.Show("Xoá thông tin phiếu nhập không thành công !");
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            List<CTPhieuNhapDTO> ds = _src.DataSource as List<CTPhieuNhapDTO>;

            if (ds == null)
                ds = new List<CTPhieuNhapDTO>();

            if (ds.Count <= 0)
            {
                MessageBox.Show("Không có vật tư nào trong phiếu nhập !");
                return;
            }

            if (string.IsNullOrEmpty(txtMaPN.Text))
            {
                MessageBox.Show("Mã phiếu nhập không được để trống !");
                return;
            }

            var pn = _dal.Get(txtMaPN.Text);

            if (pn != null)
            {
                MessageBox.Show("Mã phiếu nhập đã tồn tại !", "Thông báo");
                return;
            }

            if (cboMaNV.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên lập !");
                return;
            }

            // Tạo đối tượng và gán các giá trị được nhập
            PhieuNhapDTO obj = new PhieuNhapDTO();
            obj.MaPN = txtMaPN.Text;
            obj.NgayLap = dtNgayLap.Value;
            obj.GhiChu = txtGhiChu.Text;
            obj.MaNV = cboMaNV.SelectedValue.ToString();
            // Thực hiện cập nhật
            if (_dal.CreateOrUpdate(obj))
            {
                // Xoá chi tiết có mã phiếu nhập được cập nhật
                var dsCTPhieuNhap = _ctPhieuNhapDAL.GetBy(txtMaPN.Text);

                // Xoá chi tiết và trừ số tồn ở vật tư
                if (_ctPhieuNhapDAL.DeleteBy(txtMaPN.Text))
                {
                    for (int i = 0; i < dsCTPhieuNhap.Count; i++)
                    {
                        var vt = _VatTuDAL.Get(dsCTPhieuNhap[i].MaVT);

                        if (vt != null)
                        {
                            vt.TonKho -= dsCTPhieuNhap[i].SoLuong;
                            // Cập nhật lại số lượng
                            _VatTuDAL.CreateOrUpdate(vt);
                        }
                    }
                }

                // Thêm mới chi tiết từ danh sách chi tiết
                foreach (var ct in ds)
                {
                    ct.MaPN = obj.MaPN;
                    if (_ctPhieuNhapDAL.CreateOrUpdate(ct))
                    {
                        var vt = _VatTuDAL.Get(ct.MaVT);

                        if (vt != null)
                        {
                            vt.TonKho += ct.SoLuong;
                            // Cập nhật lại số lượng
                            _VatTuDAL.CreateOrUpdate(vt);
                        }
                    } 
                }

                MessageBox.Show("Thêm mới phiếu nhập thành công !");
                LoadGridData();
                //this.Close();
                return;
            }

            MessageBox.Show("Thêm mới phiếu nhập không thành công !");
        }
    }
}
