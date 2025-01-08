using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.Helpers
{
    public static class Utils
    {
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsValidPhoneNumber(this string str)
        {
            var matchSDT = Regex.Match(str, @"(?:\+84|0084|0)[235789][0-9]{1,2}[0-9]{7}(?:[^\d]+|$)", RegexOptions.IgnoreCase);
            return matchSDT.Success;
        }

        public static bool IsValidEmail(this string str)
        {
            var matchEmail = Regex.Match(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.IgnoreCase);
            return matchEmail.Success;
        }
    }
}
