using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Helpers
{
    public class HopThoai
    {
        public static void BaoLoi(string msg, string title = "Quản lý mượn trả vật tư")
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ThongBao(string msg, string title = "Quản lý mượn trả vật tư")
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void CanhBao(string msg, string title = "Quản lý mượn trả vật tư")
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static bool XacNhan(string msg, string title = "Quản lý mượn trả vật tư")
        {
            return MessageBox.Show(msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

    }
}
