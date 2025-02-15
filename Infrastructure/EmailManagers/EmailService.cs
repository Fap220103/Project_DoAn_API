using Application.Services.CQS.Queries;
using Application.Services.Externals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace Infrastructure.EmailManagers
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IQueryContext _context;
        private readonly IEncryptionService _encryptionService;

        public EmailService(ILogger<EmailService> logger, IQueryContext context, IEncryptionService encryptionService)
        {
            _logger = logger;
            _context = context;
            _encryptionService = encryptionService;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var defaultConfig = await _context.Config
            .ApplyIsDeletedFilter()
            .Where(x => x.Active == true)
            .SingleOrDefaultAsync();

            if (defaultConfig != null)
            {
                try
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("noreply", defaultConfig.SmtpUserName));
                    message.To.Add(new MailboxAddress(email, email));
                    message.Subject = subject;

                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = htmlMessage
                    };

                    message.Body = bodyBuilder.ToMessageBody();

                    using (var client = new SmtpClient())   
                    {
                        await client.ConnectAsync(defaultConfig.SmtpHost, defaultConfig.SmtpPort, true);
                        await client.AuthenticateAsync(defaultConfig.SmtpUserName, _encryptionService.Decrypt(defaultConfig.SmtpPassword));
                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }

            }

        }
    }
}
