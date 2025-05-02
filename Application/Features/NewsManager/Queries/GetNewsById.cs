using Application.Common.Models;
using Application.Services.CQS.Queries;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.NewsManager.Queries
{

    public class GetNewsByIdResult
    {
        public News Data { get; set; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetNewsByIdRequest : IRequest<GetNewsByIdResult>
    {
        public string Id { get; set; }
    }

    public class GetNewsByIdHandler : IRequestHandler<GetNewsByIdRequest, GetNewsByIdResult>
    {
        private readonly IQueryContext _context;

        public GetNewsByIdHandler(
            IQueryContext context
            )
        {
            _context = context;
        }

        public async Task<GetNewsByIdResult> Handle(GetNewsByIdRequest request, CancellationToken cancellationToken)
        {
            var entity = _context.News.ApplyIsDeletedFilter().FirstOrDefault(x=> x.Id == request.Id);

            return new GetNewsByIdResult
            {
                Data = entity,
                Message = "Success"
            };
        }
    }
}
