using Application.Services.Externals;
using Application.Services.Repositories;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Application.Features.ShoppingCarts.Commands
{
    public class UpdateCartResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class UpdateCartRequest : IRequest<UpdateCartResult>
    {
        public List<CartItem> items { get; init; } = null!;
        public string UserId { get; init; } = null!;

    }

    public class UpdateCartValidator : AbstractValidator<UpdateCartRequest>
    {
        public UpdateCartValidator()
        {
            RuleFor(x => x.UserId)
             .NotEmpty();
        }
    }


    public class UpdateCartHandler : IRequestHandler<UpdateCartRequest, UpdateCartResult>
    {
        private readonly ICartService _cartService;

        public UpdateCartHandler(
             ICartService cartService
            )
        {
            _cartService = cartService;
        }

        public async Task<UpdateCartResult> Handle(UpdateCartRequest request, CancellationToken cancellationToken)
        {
            Cart cart = await _cartService.GetCartAsync(request.UserId);
            if (cart != null)
            {
                foreach(var item in request.items)
                {
                    cart.UpdateQuantity(item.ProductVariantId, item.Quantity);
                }
              
                await _cartService.SaveCartAsync(cart);
            }
            else
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.UserId}");
            }

            return new UpdateCartResult
            {
                Id = request.UserId,
                Message = "Success"
            };
        }
    }
}
