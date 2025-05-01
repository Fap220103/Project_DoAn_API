using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.VnPayManagers
{
    public class VnPaySetting
    {
        public string vnp_Url { get; set; }
        public string vnp_Api { get; set; }
        public string vnp_TmnCode { get; set; }
        public string vnp_HashSecret { get; set; }
        public string vnp_Returnurl { get; set; }
    }
}
