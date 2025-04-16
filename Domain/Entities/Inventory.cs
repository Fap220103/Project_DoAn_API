using Domain.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Inventory 
    {
        [Key]
        public string ProductVariantId { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
        [JsonIgnore]
        public ProductVariant ProductVariant { get; set; }
      
    }
}
