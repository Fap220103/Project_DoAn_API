using Application.Services.CQS.Queries;
using Application.Services.Externals;
using Application.Services.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCarts.Commands
{
    public class AddItemToCartResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class AddItemToCartRequest : IRequest<AddItemToCartResult>
    {
        public CartItem Item { get; init; } = null!;
        public string userId { get; init; } = null!;
        public int Quantity { get; init; } = 1;
    }

    public class AddItemToCartValidator : AbstractValidator<AddItemToCartRequest>
    {
        public AddItemToCartValidator()
        {
            RuleFor(x => x.userId)
             .NotEmpty();
        }
    }


    public class AddItemToCartHandler : IRequestHandler<AddItemToCartRequest, AddItemToCartResult>
    {
        private readonly IQueryContext _context;
        private readonly ICartService _cartService;
        public AddItemToCartHandler(
            IQueryContext context,
            ICartService cartService
            )
        {
            _context = context;
            _cartService = cartService;
        }

        public async Task<AddItemToCartResult> Handle(AddItemToCartRequest request, CancellationToken cancellationToken = default)
        {
            var cart = await _cartService.GetCartAsync(request.userId) ?? new Cart { UserId = request.userId };

            var checkProduct = await _context.Product
                .Include(p => p.ProductImage)
                .FirstOrDefaultAsync(p => p.Id == request.Item.ProductId, cancellationToken);

            if (checkProduct == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            CartItem item = new CartItem
            {
                ProductId = checkProduct.Id,
                ProductName = checkProduct.Title,
                Quantity = request.Quantity,
                Price = checkProduct.PriceSale > 0 ? (decimal)checkProduct.PriceSale : checkProduct.Price,
                Image = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault)?.Image ?? string.Empty,
            };

            item.TotalPrice = item.Quantity * item.Price;
            cart.AddToCart(item, request.Quantity);
            await _cartService.SaveCartAsync(cart);

            return new AddItemToCartResult
            {
                Id = item.ProductId,
                Message = "Success"
            };
        }

    }
}
