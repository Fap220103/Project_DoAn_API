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
    public class DeleteCartByIdResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class DeleteCartByIdRequest : IRequest<DeleteCartByIdResult>
    {
        public string ProductId { get; init; } = null!;
        public string UserId { get; init; } = null!;
    }

    public class DeleteCartByIdValidator : AbstractValidator<DeleteCartByIdRequest>
    {
        public DeleteCartByIdValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();
        }
    }


    public class DeleteCartByIdHandler : IRequestHandler<DeleteCartByIdRequest, DeleteCartByIdResult>
    {
        private readonly ICartService _cartService;

        public DeleteCartByIdHandler(
             ICartService cartService
            )
        {
            _cartService = cartService;
        }

        public async Task<DeleteCartByIdResult> Handle(DeleteCartByIdRequest request, CancellationToken cancellationToken)
        {

            Cart cart = await _cartService.GetCartAsync(request.UserId);
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
                if (checkProduct != null)
                {
                    cart.Remove(request.ProductId);
                }
            }
            else
            {
                throw new ApplicationException($"{ExceptionConsts.CartNotFound} {request.ProductId}");
            }

            return new DeleteCartByIdResult
            {
                Id = request.ProductId,
                Message = "Success"
            };
        }
    }
}
