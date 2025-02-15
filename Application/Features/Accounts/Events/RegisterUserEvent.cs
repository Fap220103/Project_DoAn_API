using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Events
{
    public class RegisterUserEvent : INotification
    {
        public string Email { get; }
        public string? EmailConfirmationToken { get; }
        public bool SendEmailConfirmation { get; }
        public string Host { get; }

        public RegisterUserEvent(
            string email,
            string? emailConfirmationToken,
            bool sendEmailConfirmation,
            string host)
        {
            Email = email;
            EmailConfirmationToken = emailConfirmationToken;
            SendEmailConfirmation = sendEmailConfirmation;
            Host = host;
        }
    }
}
