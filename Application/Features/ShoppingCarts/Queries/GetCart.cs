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
        public ShoppingCart Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetCartRequest : IRequest<GetCartResult>
    {
    }
    public class GetCartHandler : IRequestHandler<GetCartRequest, GetCartResult>
    {
        private readonly ICartSessionService _cartSessionService;

        public GetCartHandler(
            ICartSessionService cartSessionService
            )
        {
            _cartSessionService = cartSessionService;
        }

        public async Task<GetCartResult> Handle(GetCartRequest request, CancellationToken cancellationToken)
        {
            ShoppingCart cart = _cartSessionService.GetCart();

            return new GetCartResult
            {
                Data = cart,
                Message = "Success"
            };
        }
    }
}
