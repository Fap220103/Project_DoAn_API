using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum OrderStatus
    {
        Pending = 1,            // Chờ xác nhận
        Shipping = 2,           // Đang giao hàng
        Delivered = 3,          // Đã giao hàng
        Cancelled = 4,          // Đã hủy
        Success = 5,            // Đã thanh toán
    }
}
