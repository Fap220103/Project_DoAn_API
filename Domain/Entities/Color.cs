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
    public class Color : BaseEntity
    {
        public string ColorName { get; set; } = null!;
        public string ColorCode { get; set; } = null!;
        [JsonIgnore]
        public ICollection<ProductVariant> ProductColor { get; set; } = new Collection<ProductVariant>();
        public Color() : base() { } //for EF Core
        public Color(
            string colorName,
            string colorCode
            ) : base()
        {
            ColorName = colorName.Trim();
            ColorCode = colorCode.Trim();
        }
        public void Update(
         string colorName,
         string colorCode
         )
        {
            ColorName = colorName.Trim();
            ColorCode = colorCode.Trim();
        }
    }
}
