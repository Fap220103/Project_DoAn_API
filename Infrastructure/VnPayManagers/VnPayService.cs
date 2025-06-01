using Application.Services.CQS.Commands;
using Application.Services.Externals;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.VnPayManagers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.VnPayService
{
    public class VnPayService : IVnPayService
    {
        private readonly VnPaySetting _options;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommandContext _context;

        public VnPayService(IOptions<VnPaySetting> options, IHttpContextAccessor httpContextAccessor, ICommandContext context)
        {
            _options = options.Value;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public string CreatePaymentUrl(Order order, int typePayment)
        {
            var vnPay = new VnPayLibrary(_httpContextAccessor);

            string returnUrl = _options.vnp_Returnurl;
            string url = _options.vnp_Url;
            string tmnCode = _options.vnp_TmnCode;
            string hashSecret = _options.vnp_HashSecret;

            vnPay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnPay.AddRequestData("vnp_Command", "pay");
            vnPay.AddRequestData("vnp_TmnCode", tmnCode);
            vnPay.AddRequestData("vnp_Amount", ((long)(order.TotalAmount * 100)).ToString());
            vnPay.AddRequestData("vnp_BankCode", GetBankCode(typePayment));
            vnPay.AddRequestData("vnp_CreateDate", order.CreatedAt?.ToString("yyyyMMddHHmmss"));
            vnPay.AddRequestData("vnp_CurrCode", "VND");
            vnPay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_httpContextAccessor));
            vnPay.AddRequestData("vnp_Locale", "vn");
            vnPay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng: " + order.Code);
            vnPay.AddRequestData("vnp_OrderType", "other");
            vnPay.AddRequestData("vnp_ReturnUrl", returnUrl);
            vnPay.AddRequestData("vnp_TxnRef", order.Code);

            return vnPay.CreateRequestUrl(url, hashSecret);
        }

        public bool ProcessPaymentReturn(IQueryCollection query, out string message, out long amount, out string orderId)
        {
            var vnp_HashSecret = _options.vnp_HashSecret;
            var vnPay = new VnPayLibrary(_httpContextAccessor);

            foreach (var kv in query)
            {
                if (!string.IsNullOrEmpty(kv.Key) && kv.Key.StartsWith("vnp_"))
                {
                    vnPay.AddResponseData(kv.Key, kv.Value);
                }
            }

            bool isValid = vnPay.ValidateSignature(query["vnp_SecureHash"], vnp_HashSecret);
            amount = Convert.ToInt64(vnPay.GetResponseData("vnp_Amount")) / 100;
            var orderCode = vnPay.GetResponseData("vnp_TxnRef");
            orderId = vnPay.GetResponseData("vnp_TxnRef");
            if (isValid && vnPay.GetResponseData("vnp_ResponseCode") == "00")
            {
                var order = _context.Order.FirstOrDefault(o => o.Code == orderCode);
                if (order != null)
                {
                    order.StatusPayment = StatusPayment.Paid; // Đã thanh toán
                    _context.Order.Update(order);
                    _context.SaveChangesAsync();
                }


                message = "Thanh toán thành công.";
                return true;
            }

            message = "Thanh toán thất bại.";
            return false;
        }

        private string GetBankCode(int type)
        {
            return type switch
            {
                1 => "VNPAYQR", //Thanh toán quét mã QR
                2 => "VNBANK", //Thẻ ATM - Tài khoản ngân hàng nội địa
                3 => "INTCARD", //Thẻ thanh toán quốc tế
                _ => ""
            };
        }
    }
}
