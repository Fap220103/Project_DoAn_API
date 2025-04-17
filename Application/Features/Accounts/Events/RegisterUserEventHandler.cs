using Application.Services.Externals;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Events
{
    public class RegisterUserEventHandler : INotificationHandler<RegisterUserEvent>
    {
        private readonly IEmailService _emailService;
        public RegisterUserEventHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Handle(RegisterUserEvent notification, CancellationToken cancellationToken)
        {
            if (!notification.SendEmailConfirmation)
            {
                Console.WriteLine($"Handling event for: {notification.Email}. Not sending email confirmation");
            }
            else
            {

                Console.WriteLine($"Handling event for: {notification.Email}. Sending email confirmation");

                var callbackUrl = $"https://localhost:7190/api/Accounts/ConfirmEmail?email={notification.Email}&code={notification.EmailConfirmationToken}";
                var encodeCallbackUrl = $"{HtmlEncoder.Default.Encode(callbackUrl)}";

                var emailSubject = $"Confirm your email";
                var emailMessage = $"Please confirm your account by <a href='{encodeCallbackUrl}'>clicking here</a>.";

                await _emailService.SendEmailAsync(notification.Email, emailSubject, emailMessage);

            }
        }
    }
}
