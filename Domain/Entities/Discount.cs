using Domain.Bases;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Discount : BaseEntity
    {
        public string Code { get; set; } = default!; // Mã khuyến mãi
        public string Title { get; set; } = default!; // Tên chương trình
        public string? Description { get; set; } // Mô tả

        public DiscountType DiscountType { get; set; } // Enum: Percentage hoặc FixedAmount
        public decimal DiscountValue { get; set; } // Giá trị khuyến mãi: 10 (%) hoặc 50000 (đ)

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;
        public int? UsageLimit { get; set; } // Số lần dùng tối đa
        public int UsedCount { get; set; } = 0; // Số lần đã dùng
        public Discount() : base() { } //for EF Core
        public Discount(
            string code,
            string title,
            string? description,
            DiscountType discountType,
            decimal discountValue,
            DateTime endDate,
            int usageLimit
        ) : base()
        {
            Code = code.Trim();
            Title = title.Trim();
            Description = description?.Trim();
            DiscountType = discountType;
            DiscountValue = discountValue;
            StartDate = DateTime.UtcNow;
            EndDate = endDate;
            UsageLimit = usageLimit;
            IsActive = true;
        }
        public bool CanBeUsed()
        {
            var now = DateTime.UtcNow;
            if (!IsActive || now > EndDate) return false;
            if (UsageLimit.HasValue && UsedCount >= UsageLimit.Value) return false;
            return true;
        }


    }
}
