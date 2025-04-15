using Domain.Bases;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductCategory : BaseEntityAdvance
    {
        public string? Alias { get; set; }
        public string? Icon { get; set; }
        public string? ParentId { get; set; }
        public bool IsActive { get; set; }
        public int Level { get; set; }
        public ICollection<Product> Products { get; set; } = new Collection<Product>();
        public ProductCategory? ParentCategory { get; set; }
        public ICollection<ProductCategory> ChildCategories { get; set; } = new List<ProductCategory>();
        public ProductCategory() : base() { } //for EF Core
        public ProductCategory(
            string? userId,
            string title,
            string? alias,
            string? description,
            string seoTitle,
            string seoDescription,
            string seoKeywords,
            string? icon,
            int level,
            string parentId
            ) : base(seoTitle, seoDescription, seoKeywords,userId, title, description)
        {
            Level = level;
            Icon = icon;
            ParentId = string.IsNullOrWhiteSpace(parentId) ? null : parentId.Trim();
            Alias = alias;
            IsActive = true;
        }

        public void Update(
            string? userId,
            string title,
            string? alias,
            string? description,
            string seoTitle,
            string seoDescription,
            string seoKeywords,
            string icon,
            int level,
            string? parentId,
            bool isActive
            )
        {
            Title = title.Trim();
            Description = description?.Trim();
            Icon = icon;
            Alias = alias;
            ParentId = parentId;
            Level = level;
            IsActive = isActive;
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
