using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Bases;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class OrderDetail
    {
        public string OrderId { get; set; } = null!;
        public string ProductVariantId { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; }
        public ProductVariant ProductVariant { get; set; }
    }
}
