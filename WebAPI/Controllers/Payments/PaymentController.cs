using Application.Services.Externals;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Payments
{
    public class PaymentController : BaseApiController
    {
        private readonly IVnPayService _vnPayService;

        public PaymentController(ISender sender, IVnPayService vnPayService, ICartService cartService) : base(sender)
        {
            _vnPayService = vnPayService;
        }
        [HttpGet("vnpay-return")]
        public IActionResult VnPayReturn()
        {
            var query = HttpContext.Request.Query;
            var success = _vnPayService.ProcessPaymentReturn(query, out string message, out long amount, out string orderId);

            if (success)
            {
                return Redirect($"http://localhost:4200/checkoutsuccess/{orderId}");
            }

            return Redirect($"http://localhost:4200/payment-failure?message={Uri.EscapeDataString(message)}");
        }
    }
}
