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
    public class Size : BaseEntity
    {
        public string SizeName { get; set; } = null!;
        [JsonIgnore]
        public ICollection<ProductVariant> ProductSize { get; set; } = new Collection<ProductVariant>();
        public Size() : base() { } //for EF Core
        public Size(
            string sizeName
            ) : base()
        {
            SizeName = sizeName.Trim();
        }
        public void Update(
           string sizeName
        )
        {
            SizeName = sizeName.Trim();
        }
    }
}
