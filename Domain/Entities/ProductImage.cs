using Domain.Bases;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public string ProductId { get; set; } = null!;
        public string? Image { get; set; }
        public bool IsDefault { get; set; } 
        [JsonIgnore]
        public Product Product { get; set; }
        public ProductImage() : base() { } //for EF Core
        public ProductImage(
            string productId,
            string image    
            ) : base()
        {
            ProductId = productId;
            Image = image;
            IsDefault = false;
        }
        //public void Update(
        // string colorName,
        // string colorCode
        // )
        //{
        //    ColorName = colorName.Trim();
        //    ColorCode = colorCode.Trim();
        //}
    }
}
