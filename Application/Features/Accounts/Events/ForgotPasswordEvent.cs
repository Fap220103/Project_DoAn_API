using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Events
{
    public class ForgotPasswordEvent : INotification
    {
        public string Email { get; }
        public string? EmailConfirmationToken { get; }
        public string Host { get; }

        public ForgotPasswordEvent(
            string email,
            string? emailConfirmationToken,
            string host)
        {
            Email = email;
            EmailConfirmationToken = emailConfirmationToken;
            Host = host;
        }
    }
}
