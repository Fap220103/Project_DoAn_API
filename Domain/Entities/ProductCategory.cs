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
        public ICollection<Product> Products { get; set; } = new Collection<Product>();


    }
}
