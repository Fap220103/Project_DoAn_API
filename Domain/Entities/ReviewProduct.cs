using Domain.Bases;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ReviewProduct : BaseEntity
    {
        public string ProductId { get; set; } = null!;
        public string CustomerId { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int Rate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Product Product { get; set; }
        public ReviewProduct() : base() { } //for EF Core
        public ReviewProduct(
            string productId,
            string customerId,
            string content,
            int rate
        )
        {
            ProductId = productId.Trim();
            CustomerId = customerId.Trim();
            Content = content.Trim();
            Rate = rate;
            CreatedDate = DateTime.Now;
        }

    }
}
