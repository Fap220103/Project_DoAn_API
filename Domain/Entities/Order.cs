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
        public string CustomerName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Email { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public int TypePayment { get; set; }
        public int Status { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new Collection<OrderDetail>();
        public Order() : base() { } //for EF Core
        public Order(
            string? userId,
            string code,
            string customerName,
            string phone,
            string address,
            string email,
            decimal totalAmount,
            int quantity,
            int typePayment,
            int status
            ) : base(userId)
        {
            Code = code.Trim();
            CustomerName = customerName.Trim();
            Phone = phone.Trim();
            Address = address.Trim();
            Email = email.Trim();
            TotalAmount = totalAmount;
            Quantity = quantity;
            TypePayment = typePayment;
            Status = status;
            OrderDetails = new Collection<OrderDetail>();
        }
        public void Update(
            string? userId,
            string code,
            string customerName,
            string phone,
            string address,
            string email,
            decimal totalAmount,
            int quantity,
            int typePayment,
            string customerId,
            int status
          )
        {
            Code = code.Trim();
            CustomerName = customerName.Trim();
            Phone = phone.Trim();
            Address = address.Trim();
            Email = email.Trim();
            TotalAmount = totalAmount;
            Quantity = quantity;
            TypePayment = typePayment;
            Status = status;

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
