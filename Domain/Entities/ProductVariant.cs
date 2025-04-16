using Domain.Bases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductVariant : BaseEntity
    {
        public string ProductId { get; set; } = null!;
        public int ColorId { get; set; } 
        public int SizeId { get; set; } 
        public Color Color { get; set; }
        public Size Size { get; set; }
        public Product Product { get; set; }
        public Inventory Inventory { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new Collection<OrderDetail>();
    }
}
