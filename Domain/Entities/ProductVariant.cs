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
        public string ColorId { get; set; } = null!;
        public string SizeId { get; set; } = null!;
        public Color Color { get; set; }
        public Size Size { get; set; }
        public Product Product { get; set; }
        public ICollection<Inventory> Inventory { get; set; } = new Collection<Inventory>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new Collection<OrderDetail>();
    }
}
