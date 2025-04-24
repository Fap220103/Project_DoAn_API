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
       
        Failed = 5,              // Thanh toán thất bại
        Success = 6,           // Đã thanh toán
    }
}
