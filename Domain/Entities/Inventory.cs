using Domain.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Inventory : BaseEntity
    {
        public string ProductId { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
        public Product Product { get; set; }
        public Inventory() : base() { }
        public Inventory(string productId, int quantity, DateTime lastUpdated) : base() 
        {
            this.ProductId = productId;
            Quantity = quantity;
            LastUpdated = lastUpdated;
        } 
    }
}
