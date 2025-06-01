using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum StatusPayment
    {
        NotPaid = 1,        // Chưa thanh toán
        Paid = 2,           // Đã thanh toán
        Refund = 3,         // Hoàn tiền
    }
}
