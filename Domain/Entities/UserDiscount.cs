using Domain.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserDiscount : BaseEntity
    { 
        public string UserId { get; set; } 
        public string DiscountId { get; set; }
        public bool IsUsed { get; set; } = false;
        public Discount Discount { get; set; }
    }
}
