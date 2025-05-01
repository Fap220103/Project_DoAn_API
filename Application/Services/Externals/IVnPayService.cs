using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Externals
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(Order order, int typePayment);
        bool ProcessPaymentReturn(IQueryCollection queryCollection, out string message, out long amount, out string orderId);
    }
}
