using Application.Features.Orders.Commands;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Externals
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task SendEmailOrderAsync(List<CartDto> items, Order order, ShippingAddress addressOrder, string email, decimal totalDiscount);
    }
}
