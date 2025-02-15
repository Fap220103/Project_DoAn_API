using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Bases
{

    /// <summary>
    /// Base class for advanced entities that extends <see cref="BaseEntityAudit"/>.
    /// Adds properties for Seo.
    /// </summary>
    public abstract class BaseEntityAdvance : BaseEntityCommon
    {
        public string? SeoTitle { get; set; } = null!;
        public string? SeoDescription { get; set; } = null!;
        public string? SeoKeywords { get; set; } = null!;

        protected BaseEntityAdvance() { } // for EF Core
        protected BaseEntityAdvance(
            string? seoTitle,
            string? seoDescription,
            string? seoKeywords,
            string? userId,
            string title,
            string? description
            ) : base(userId,title, description)
        {
            SeoTitle = seoTitle?.Trim();
            SeoDescription = seoDescription?.Trim();
            SeoKeywords = seoKeywords?.Trim();
        }
    }
}
