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
    public class ProductCategory : BaseEntity
    {
        public string? Alias { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ParentId { get; set; }
        public bool IsActive { get; set; }
        public int Level { get; set; }
        public string Link { get; set; } = null!;
        public bool IsHasChild { get; set; } = false;
        
        public ICollection<Product> Products { get; set; } = new Collection<Product>();
        public ProductCategory? ParentCategory { get; set; }
        public ICollection<ProductCategory> ChildCategories { get; set; } = new List<ProductCategory>();
        public ProductCategory() : base() { } //for EF Core
        public ProductCategory(
            string title,
            string? alias,
            string? description,
            int level,
            string parentId,
            string link
            ) 
        {
            Title = title.Trim();
            Description = description?.Trim();
            Level = level;
            ParentId = string.IsNullOrWhiteSpace(parentId) ? null : parentId.Trim();
            Alias = alias;
            IsActive = true;
            Link = link;
            IsHasChild = ChildCategories != null && ChildCategories.Any(); ;
        }

        public void Update(
            string title,
            string? alias,
            string? description,
            int level,
            string? parentId,
            bool isActive,
            string link
            )
        {
            Title = title.Trim();
            Description = description?.Trim();
            Alias = alias;
            ParentId = parentId;
            Level = level;
            IsActive = isActive;
            Link = link;
            IsHasChild = ChildCategories != null && ChildCategories.Any(); ;
        }

    }
}
