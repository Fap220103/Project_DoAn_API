using Domain.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductSize
    {
        public string ProductId { get; set; } = null!;
        public string SizeId { get; set; } = null!;
        public Product Product { get; set; }
        public Size Size { get; set; }
    }
}
