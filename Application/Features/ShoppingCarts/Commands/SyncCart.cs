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
    public class SyncCartResult
    {
        public string Id { get; init; } = null!;
        public int countCart { get; set; }
        public string Message { get; init; } = null!;
    }

    public class SyncCartRequest : IRequest<SyncCartResult>
    {
        public List<CartItem> Items { get; init; } = null!;
        public string userId { get; init; } = null!;
    }

    public class SyncCartValidator : AbstractValidator<SyncCartRequest>
    {
        public SyncCartValidator()
        {
        }
    }


    public class SyncCartHandler : IRequestHandler<SyncCartRequest, SyncCartResult>
    {
        private readonly IQueryContext _context;
        private readonly ICartService _cartService;
        public SyncCartHandler(
            IQueryContext context,
            ICartService cartService
            )
        {
            _context = context;
            _cartService = cartService;
        }

        public async Task<SyncCartResult> Handle(SyncCartRequest request, CancellationToken cancellationToken = default)
        {
            var cart = await _cartService.GetCartAsync(request.userId) ?? new Cart { UserId = request.userId };

            foreach (var item in request.Items)
            {
                cart.AddToCart(item, item.Quantity);
            }

            await _cartService.SaveCartAsync(cart);
            
            return new SyncCartResult
            {
                Id = request.userId,
                countCart = cart.Items.Count(),
                Message = "Success"
            };
        }

    }
}
