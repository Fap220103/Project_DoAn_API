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
    public class Size
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonIgnore]
        public ICollection<ProductVariant> ProductSize { get; set; } = new Collection<ProductVariant>();


    }
}
