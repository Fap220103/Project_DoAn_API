using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Bases;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        public string OrderId { get; set; } = null!;
        public string ProductVariantId { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; }
        public ProductVariant ProductVariant { get; set; }
        public OrderDetail() : base() { } //for EF Core

        //public OrderDetail(
        //    string key,
        //    string value
        //    ) : base()
        //{
        //    Key = key;
        //    Value = value;
        //}

        //public void Update(
        //    string key,
        //    string value
        //    )
        //{
        //    Key = key;
        //    Value = value;
        //}
    }
}
