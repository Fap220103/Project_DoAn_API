using Domain.Bases;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Product : BaseEntityAdvance
    {   
        public string? Alias { get; set; }
        public string? ProductCode { get; set; }   
        [AllowHtml]
        public string? Detail { get; set; }
        public string? Image { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Price { get; set; }
        public int SalePercent { get; set; }
        public decimal PriceSale { get; set; }
        public int ViewCount { get; set; }
        public bool IsSale { get; set; }
        public bool IsActive { get; set; }
        public string? ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public ICollection<ProductImage> ProductImage { get; set; } = new Collection<ProductImage>();
        public ICollection<ProductVariant> ProductVariants { get; set; } = new Collection<ProductVariant>();
        public Product() : base() { } //for EF Core
        public Product(
            string? userId,
            string title,
            string? alias,
            string? description,
            string? seoTitle,
            string? seoDescription,
            string? seoKeywords,
            string? image,
            string detail,
            decimal originalPrice,
            decimal price,
            int salePercent,
            string? productCategoryId
            ) : base(seoTitle, seoDescription, seoKeywords, userId, title, description)
        {
            Alias = alias;
            Image = image;
            Detail = detail;
            OriginalPrice = originalPrice;
            Price = price;
            SalePercent = salePercent;
            PriceSale = price - (price * salePercent / 100);
            IsSale = salePercent > 0;
            IsActive = true;
            ProductCategoryId = productCategoryId;
        }
        public void Update(
            string? userId,
            string title,
            string? alias,
            string? description,
            string? seoTitle,
            string? seoDescription,
            string? seoKeywords,
            string detail,
            decimal originalPrice,
            decimal price,
            int salePercent,
            bool isActive,
            string? productCategoryId
            )
        {
            Alias = alias;
            Detail = detail;
            Title = title.Trim();
            Description = description?.Trim();
            OriginalPrice = originalPrice;
            Price = price;
            SalePercent = salePercent;
            PriceSale = price - (price * salePercent / 100);
            IsSale = salePercent > 0;
            IsActive = isActive;
            ProductCategoryId = productCategoryId;
            SeoTitle = seoTitle?.Trim();
            SeoDescription = seoDescription?.Trim();
            SeoKeywords = seoKeywords?.Trim();
            SetAudit(userId);
        }

        public void Delete(
            string? userId
            )
        {
            SetAsDeleted();
            SetAudit(userId);
        }
    }
}
