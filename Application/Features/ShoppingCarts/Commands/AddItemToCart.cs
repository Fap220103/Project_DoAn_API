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
        public string ProductId { get; init; } = null!;
        public int Quantity { get; init; }
    }

    public class AddItemToCartValidator : AbstractValidator<AddItemToCartRequest>
    {
        public AddItemToCartValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();
            RuleFor(x => x.Quantity)
                .NotEmpty();
        }
    }


    public class AddItemToCartHandler : IRequestHandler<AddItemToCartRequest, AddItemToCartResult>
    {
        private readonly IQueryContext _context;
        private readonly ICartSessionService _cartSessionService;
        public AddItemToCartHandler(
            IQueryContext context,
            ICartSessionService cartSessionService
            )
        {
            _context = context;
            _cartSessionService = cartSessionService;
        }

        public async Task<AddItemToCartResult> Handle(AddItemToCartRequest request, CancellationToken cancellationToken = default)
        {
            var checkProduct = await _context.Product.Include(p => p.ProductCategory)
                                                .Include(p => p.ProductImage)
                                                .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);
            if (checkProduct != null)
            {
                ShoppingCart cart = _cartSessionService.GetCart();
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                CartItem item = new CartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Quantity = request.Quantity,
                    Alias = checkProduct.Alias,

                };
                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, request.Quantity);
                _cartSessionService.SetCart(cart);

            }

            return new AddItemToCartResult
            {
                Id = request.ProductId,
                Message = "Success"
            };
        }
    }
}
