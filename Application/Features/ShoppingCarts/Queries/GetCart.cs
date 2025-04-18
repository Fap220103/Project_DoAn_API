using Application.Services.CQS.Queries;
using Application.Services.Externals;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCarts.Queries
{


    public class GetCartResult
    {
        public Cart Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetCartRequest : IRequest<GetCartResult>
    {
        public string userId { get; init; } = null!;
    }
    public class GetCartHandler : IRequestHandler<GetCartRequest, GetCartResult>
    {
        private readonly ICartService _cartService;

        public GetCartHandler(
            ICartService cartService
            )
        {
            _cartService = cartService;
        }

        public async Task<GetCartResult> Handle(GetCartRequest request, CancellationToken cancellationToken)
        {
            Cart cart = await _cartService.GetCartAsync(request.userId);

            return new GetCartResult
            {
                Data = cart,
                Message = "Success"
            };
        }
    }
}
