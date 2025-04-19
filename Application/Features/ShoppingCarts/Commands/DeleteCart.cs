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
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }
    public class DeleteCartRequest : IRequest<DeleteCartResult>
    {
        public string userId { get; init; } = null!;
    }
    public class DeleteCartHandler : IRequestHandler<DeleteCartRequest, DeleteCartResult>
    {
        private readonly ICartService _cartService;

        public DeleteCartHandler(
             ICartService cartService
            )
        {
            _cartService = cartService;
        }

        public async Task<DeleteCartResult> Handle(DeleteCartRequest request, CancellationToken cancellationToken)
        {

            Cart cart = await _cartService.GetCartAsync(request.userId);
            if (cart != null)
            {
                await _cartService.DeleteCartAsync(request.userId);
            }
            else
            {
                throw new ApplicationException($"{ExceptionConsts.CartNotFound} {request.userId}");
            }

            return new DeleteCartResult
            {
                Id = request.userId,
                Message = "Success"
            };
        }
    }
}
