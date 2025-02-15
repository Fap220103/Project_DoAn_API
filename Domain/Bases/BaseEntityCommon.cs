using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Bases
{

    /// <summary>
    /// Base class for common entities that extends <see cref="BaseEntityAudit"/> and adds properties for name and description.
    /// Used for entities that require basic audit information along with a title and an optional description.
    /// </summary>
    public abstract class BaseEntityCommon : BaseEntityAudit
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        protected BaseEntityCommon() { } // for EF Core
        protected BaseEntityCommon(
            string? userId,
            string title,
            string? description
            ) : base(userId)
        {
            Title = title.Trim();
            Description = description?.Trim();
        }
    }
}
