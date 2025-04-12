using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Externals
{
    public interface ICommonService
    {
        string FilterChar(string str);
        string ChuyenCoDauThanhKhongDau(string str);
        string FormatNumber(object value, int SoSauDauPhay = 2);
        bool IsNumeric(object value);
        string HtmlRate(int rate);
        string GenerateCode(string prefix);
    }
}
