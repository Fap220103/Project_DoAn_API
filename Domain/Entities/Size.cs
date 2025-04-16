using Domain.Bases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Size
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonIgnore]
        public ICollection<ProductVariant> ProductVariants { get; set; } = new Collection<ProductVariant>();


    }
}
