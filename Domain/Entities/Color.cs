using Domain.Bases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string HexCode { get; set; } = null!;
            [JsonIgnore]
        public ICollection<ProductVariant> ProductColor { get; set; } = new Collection<ProductVariant>();
    }
}
