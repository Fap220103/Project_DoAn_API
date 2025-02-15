using Domain.Bases;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Product : BaseEntityAdvance, IAggregateRoot
    {   
        public string? Alias { get; set; }
        public string? ProductCode { get; set; }   
        [AllowHtml]
        public string? Detail { get; set; }
        public string? Image { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceSale { get; set; }
        public int? ViewCount { get; set; }
        public bool IsHome { get; set; }
        public bool IsSale { get; set; }
        public bool IsFeature { get; set; }
        public bool IsHot { get; set; }
        public bool IsActive { get; set; }
        public int ProductCategoryID { get; set; }
        public ICollection<ProductImage> ProductImage { get; set; } = new Collection<ProductImage>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new Collection<OrderDetail>();
        public ICollection<ProductColor> ProductColor { get; set; } = new Collection<ProductColor>();
        public ICollection<ProductSize> ProductSize { get; set; } = new Collection<ProductSize>();
        public ICollection<Inventory> Inventory { get; set; } = new Collection<Inventory>();
        //public virtual ICollection<ReviewProduct> ReviewProducts { get; set; }
        //public virtual ICollection<WishList> WishLists { get; set; }
    }
}
