using Application.Services.Externals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.VnPayService
{
    public class VnPayService : IVnPayService
    {
        public Task<string> UrlPayment(int TypePaymentVN, string orderCode)
        {
            throw new NotImplementedException();
        }
    }
}
