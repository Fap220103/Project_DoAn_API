using Application.Services.Externals;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CommonManagers
{
    public class CommonService : ICommonService
    {
        private static readonly Random _random = new Random();
        private static readonly string[] VietNamChar = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };
        public string ChuyenCoDauThanhKhongDau(string str)
        {
            str = str.Trim();
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                {
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
                }
            }
            // Không thay thế khoảng trắng thành dấu "-"
            str = str.Replace("--", "-")
                     .Replace("?", "")
                     .Replace("&", "")
                     .Replace(",", "")
                     .Replace(":", "")
                     .Replace("!", "")
                     .Replace("'", "")
                     .Replace("\"", "")
                     .Replace("%", "")
                     .Replace("#", "")
                     .Replace("$", "")
                     .Replace("*", "")
                     .Replace("`", "")
                     .Replace("~", "")
                     .Replace("@", "")
                     .Replace("^", "")
                     .Replace(".", "")
                     .Replace("/", "")
                     .Replace(">", "")
                     .Replace("<", "")
                     .Replace("[", "")
                     .Replace("]", "")
                     .Replace(";", "")
                     .Replace("+", "");
            return str.ToLower();
        }

        public string FilterChar(string str)
        {
            str = str.Trim();
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                {
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
                }
            }
            str = str.Replace(" ", "-");
            str = str.Replace("--", "-");
            str = str.Replace("?", "")
                     .Replace("&", "")
                     .Replace(",", "")
                     .Replace(":", "")
                     .Replace("!", "")
                     .Replace("'", "")
                     .Replace("\"", "")
                     .Replace("%", "")
                     .Replace("#", "")
                     .Replace("$", "")
                     .Replace("*", "")
                     .Replace("`", "")
                     .Replace("~", "")
                     .Replace("@", "")
                     .Replace("^", "")
                     .Replace(".", "")
                     .Replace("/", "")
                     .Replace(">", "")
                     .Replace("<", "")
                     .Replace("[", "")
                     .Replace("]", "")
                     .Replace(";", "")
                     .Replace("+", "");
            return str.ToLower();
        }

        public string FormatNumber(object value, int SoSauDauPhay = 2)
        {
            bool isNumber = IsNumeric(value);
            decimal GT = 0;
            if (isNumber)
            {
                GT = Convert.ToDecimal(value);
            }

            string thapPhan = new string('0', SoSauDauPhay); // đổi từ '#' thành '0' nếu bạn luôn muốn đủ số chữ số
            string snumformat = "#,##0" + (SoSauDauPhay > 0 ? "." + thapPhan : string.Empty);

            return GT.ToString(snumformat);
        }


        public string GenerateCode(string prefix)
        {
            lock (_random)
            {
                var now = DateTime.UtcNow;
                var randomPart = _random.Next(1000, 10000);
                return $"{prefix}-{now:yyyyMMddHHmmss}-{randomPart}";
            }
        }

        public string HtmlRate(int rate)
        {
            string str = string.Empty;
            switch (rate)
            {
                case 0:
                    str = @"<li><i class='fa fa-star-o' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star-o' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star-o' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star-o' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star-o' aria-hidden='true'></i></li>";
                    break;
                case 1:
                    str = @"<li><i class='fa fa-star' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star-o' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star-o' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star-o' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star-o' aria-hidden='true'></i></li>";
                    break;
                case 2:
                    str = @"<li><i class='fa fa-star' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star-o' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star-o' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star-o' aria-hidden='true'></i></li>";
                    break;
                case 3:
                    str = @"<li><i class='fa fa-star' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star-o' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star-o' aria-hidden='true'></i></li>";
                    break;
                case 4:
                    str = @"<li><i class='fa fa-star' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star-o' aria-hidden='true'></i></li>";
                    break;
                case 5:
                    str = @"<li><i class='fa fa-star' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star' aria-hidden='true'></i></li>
                            <li><i class='fa fa-star' aria-hidden='true'></i></li>";
                    break;
                default:
                    break;
            }
            return str;
        }

        public bool IsNumeric(object value)
        {
            return value is sbyte
                   || value is byte
                   || value is short
                   || value is ushort
                   || value is int
                   || value is uint
                   || value is long
                   || value is ulong
                   || value is float
                   || value is double
                   || value is decimal;
        }
    }
}
