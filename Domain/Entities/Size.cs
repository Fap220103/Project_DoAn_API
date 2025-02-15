using Domain.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Size : BaseEntity
    {
        public string SizeName { get; set; } = null!;
        public Size() : base() { } //for EF Core
        public Size(
            string sizeName
            ) : base()
        {
            SizeName = sizeName.Trim();
        }


    
    }
}
