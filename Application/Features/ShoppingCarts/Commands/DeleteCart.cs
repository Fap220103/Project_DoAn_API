using Application.Services.Externals;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCarts.Commands
{
    public class DeleteCartResult
    {
        public string Message { get; init; } = null!;
    }

    public class DeleteCartRequest : IRequest<DeleteCartResult>
    {
    }

    public class DeleteCartValidator : AbstractValidator<DeleteCartRequest>
    {
        public DeleteCartValidator()
        {
        }
    }


    public class DeleteCartHandler : IRequestHandler<DeleteCartRequest, DeleteCartResult>
    {
        private readonly ICartSessionService _cartSessionService;

        public DeleteCartHandler(
             ICartSessionService cartSessionService
            )
        {
            _cartSessionService = cartSessionService;
        }

        public async Task<DeleteCartResult> Handle(DeleteCartRequest request, CancellationToken cancellationToken)
        {

            ShoppingCart cart = _cartSessionService.GetCart();
            if (cart != null)
            {
                cart.ClearCart();
                _cartSessionService.SetCart(cart);
            }

            return new DeleteCartResult
            {
                Message = "Success"
            };
        }
    }
}
