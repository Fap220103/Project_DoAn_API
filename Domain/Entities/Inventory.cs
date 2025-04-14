using Domain.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Inventory : BaseEntity
    {
        public string ProductVariantId { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
        [JsonIgnore]
        public ProductVariant ProductVariant { get; set; }
        public Inventory() : base() { }
        public Inventory(string productVariantId, int quantity, DateTime lastUpdated) : base() 
        {
            this.ProductVariantId = productVariantId;
            Quantity = quantity;
            LastUpdated = lastUpdated;
        } 
    }
}
