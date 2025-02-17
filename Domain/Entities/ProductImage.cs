using Domain.Bases;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public string ProductId { get; set; } = null!;
        public string Image { get; set; } = null!;
        public bool IsDefault { get; set; }
        public Product Product { get; set; }
    }
}
