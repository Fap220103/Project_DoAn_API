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
    public class Order : BaseEntityAudit
    {
        public string Code { get; set; } = null!;
        public string CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public int Quantity { get; set; }
        public int TypePayment { get; set; }
        public int Status { get; set; }
        public string ShippingAddressId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new Collection<OrderDetail>();
        public Order() : base() { } //for EF Core
        public Order(
            string? userId,
            string code,
            decimal totalAmount,
            decimal totalDiscount,
            int quantity,
            int typePayment,
            int status,
            string shippingAddressId
            ) : base(userId)
        {
            Code = code.Trim();
            TotalAmount = totalAmount;
            TotalDiscount = totalDiscount;
            Quantity = quantity;
            TypePayment = typePayment;
            Status = status;
            CustomerId = userId;
            ShippingAddressId = shippingAddressId;
        }
        public void Update(
            string? userId,
            string code,
            decimal totalAmount,
            decimal totalDiscount,
            int quantity,
            int typePayment,
            string customerId,
            int status,
            string shippingAddressId
          )
        {
            Code = code.Trim();
            TotalAmount = totalAmount;
            TotalDiscount = totalDiscount;
            Quantity = quantity;
            TypePayment = typePayment;
            Status = status;
            ShippingAddressId = shippingAddressId;
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
