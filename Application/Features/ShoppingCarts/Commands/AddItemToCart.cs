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
        public List<CartItem> Items { get; init; } = null!;
        public string userId { get; init; } = null!;
    }

    public class AddItemToCartValidator : AbstractValidator<AddItemToCartRequest>
    {
        public AddItemToCartValidator()
        {
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
  
            foreach(var item in request.Items)
            {
                cart.AddToCart(item, item.Quantity);
            }
 
            await _cartService.SaveCartAsync(cart);

            return new AddItemToCartResult
            {
                Id = request.userId,
                Message = "Success"
            };
        }

    }
}
