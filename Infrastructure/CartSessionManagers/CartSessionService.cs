using Application.Services.Externals;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CartSessionManagers
{
    public class CartSessionService : ICartSessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartSessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ShoppingCart GetCart()
        {
            // Sử dụng extension method đã tạo trong Presentation (ví dụ: trong thư mục Extensions)
            return _httpContextAccessor.HttpContext.Session.GetObject<ShoppingCart>("Cart");
        }

        public void SetCart(ShoppingCart cart)
        {
            _httpContextAccessor.HttpContext.Session.SetObject("Cart", cart);
        }
    }
}
