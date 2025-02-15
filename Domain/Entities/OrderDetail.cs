using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Bases;

namespace Domain.Entities
{
    public class OrderDetail 
    {
        public string OrderID { get; set; } = null!;
        public string ProductID { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        //public virtual Order Order { get; set; }
        //public virtual Product Product { get; set; }
    }
}
