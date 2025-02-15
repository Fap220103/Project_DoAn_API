using Domain.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Color : BaseEntity
    {
        public string ColorName { get; set; } = null!;
        public string ColorCode { get; set; } = null!;
        public Color() : base() { } //for EF Core
        public Color(
            string colorName,
            string colorCode
            ) : base()
        {
            ColorName = colorName.Trim();
            ColorCode = colorCode.Trim();
        }


    }
}
