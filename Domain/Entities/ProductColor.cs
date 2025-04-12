using Domain.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductColor 
    {
        public string ProductId { get; set; } = null!;
        public string ColorId { get; set; } = null!;
        public Color Color { get; set; }
        public Product Product { get; set; }
    }
}
