using Application.Features.Accounts.Events;
using Application.Services.Externals;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Application.Features.Orders.Events
{
    public class SendMailOrderEventHandler : INotificationHandler<SendMailOrderEvent>
    {
        private readonly IEmailService _emailService;
        public SendMailOrderEventHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Handle(SendMailOrderEvent notification, CancellationToken cancellationToken)
        {
           await _emailService.SendEmailOrderAsync(notification.cart, notification.order, notification.shippingAddress, notification.email, notification.totalDiscount);
        }
    }
}
