using Application.Features.Orders.Commands;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Events
{
    public class SendMailOrderEvent : INotification
    {
        public List<CartDto> cart { get; }
        public Order order { get; }
        public ShippingAddress shippingAddress { get; }
        public string email { get; }


        public SendMailOrderEvent(
            List<CartDto> cart,
            Order order,
            ShippingAddress shippingAddress,
            string email)
        {
            this.cart = cart;
            this.order = order;
            this.shippingAddress = shippingAddress;
            this.email = email;
        }
    }
}
