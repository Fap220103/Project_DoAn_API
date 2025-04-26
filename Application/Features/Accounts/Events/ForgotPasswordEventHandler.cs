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
    public class ForgotPasswordEventHandler : INotificationHandler<ForgotPasswordEvent>
    {
        private readonly IEmailService _emailService;
        public ForgotPasswordEventHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        //public async Task Handle(ForgotPasswordEvent notification, CancellationToken cancellationToken)
        //{

        //    Console.WriteLine($"Handling event for: {notification.Email}. Sending email confirmation");

        //    var callbackUrl = $"{notification.Host}/Accounts/ForgotPasswordConfirmation?email={notification.Email}&code={notification.EmailConfirmationToken}&tempPassword={notification.TempPassword}";
        //    var encodeCallbackUrl = $"{HtmlEncoder.Default.Encode(callbackUrl)}";

        //    var emailSubject = $"Forgot password confirmation";
        //    var emailMessage = $"Your temporary password is: <strong>{notification.ClearTempPassword}</strong>. Please confirm reset your password by <a href='{encodeCallbackUrl}'>clicking here</a>.";

        //    await _emailService.SendEmailAsync(notification.Email, emailSubject, emailMessage);
        //}
        public async Task Handle(ForgotPasswordEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Handling event for: {notification.Email}. Sending email confirmation");
            var callbackUrl = $"{notification.Host}/resetpass?email={notification.Email}&code={notification.EmailConfirmationToken}";
            var encodeCallbackUrl = $"{HtmlEncoder.Default.Encode(callbackUrl)}";
            var emailSubject = $"Forgot password confirmation";
            var emailMessage = $"Please confirm reset your password by <a href='{encodeCallbackUrl}'>clicking here</a>.";
            await _emailService.SendEmailAsync(notification.Email, emailSubject, emailMessage);
        }
    }
}
