using Application.Services.CQS.Queries;
using Application.Services.Externals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using MailKit.Security;
using Domain.Entities;
using Org.BouncyCastle.Ocsp;
using Microsoft.AspNetCore.Hosting;
using Application.Features.Orders.Commands;

namespace Infrastructure.EmailManagers
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly ICommonService _commonService;
        private readonly IWebHostEnvironment _env;
        private readonly EmailSettings _emailSettings;

        public EmailService(ILogger<EmailService> logger,
                            IOptions<EmailSettings> emailSettings,
                            ICommonService commonService, 
                            IWebHostEnvironment env)
        {

            _logger = logger;
            _commonService = commonService;
            _env = env;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("noreply", _emailSettings.SmtpUserName));
                message.To.Add(new MailboxAddress(email, email));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlMessage
                };

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_emailSettings.SmtpUserName, _emailSettings.SmtpPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new EmailException("Lỗi trong quá trình gửi mail");
            }
        }

        public async Task SendEmailOrderAsync(List<CartDto> items, Order order, ShippingAddress addressOrder, string email)
        {
            var strSanPham = "";
            var thanhtien = decimal.Zero;
            var TongTien = decimal.Zero;
            var ngaydat = order.CreatedAt;
            foreach (var sp in items)
            {
                strSanPham += "<tr>";
                strSanPham += "<td>" + sp.ProductName + "</td>";
                strSanPham += "<td>" + sp.SizeName + "</td>";
                strSanPham += "<td>" + sp.ColorName + "</td>";
                strSanPham += "<td>" + sp.Quantity + "</td>";
                strSanPham += "<td>" + _commonService.FormatNumber(sp.TotalPrice, 0) + "</td>";
                strSanPham += "</tr>";
                thanhtien += sp.Price * sp.Quantity;
            }
            TongTien = thanhtien;
            string orderAddress = addressOrder.AddressLine + " - " + addressOrder.Province + " - " + addressOrder.District + " - " + addressOrder.Ward;
            string templatePath = Path.Combine(_env.ContentRootPath, "EmailTemplates", "send2.html");
            string contentCustomer = await File.ReadAllTextAsync(templatePath);
            contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
            contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", addressOrder.RecipientName);
            contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
            contentCustomer = contentCustomer.Replace("{{Phone}}", addressOrder.PhoneNumber);
            contentCustomer = contentCustomer.Replace("{{Email}}", email);
            contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", orderAddress);
            contentCustomer = contentCustomer.Replace("{{ThanhTien}}", _commonService.FormatNumber(thanhtien, 0));
            contentCustomer = contentCustomer.Replace("{{TongTien}}", _commonService.FormatNumber(TongTien, 0));
            await SendEmailAsync(email, "Đơn hàng #" + order.Code, contentCustomer.ToString());
        }
    }
}
