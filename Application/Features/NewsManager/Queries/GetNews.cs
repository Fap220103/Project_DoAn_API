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

    public class GetNewsResult
    {
        public IEnumerable<News> Data { get; set; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetNewsRequest : IRequest<GetNewsResult>
    {
        public string? Type { get; set; }
    }

    public class GetNewsHandler : IRequestHandler<GetNewsRequest, GetNewsResult>
    {
        private readonly IQueryContext _context;

        public GetNewsHandler(
            IQueryContext context
            )
        {
            _context = context;
        }

        public async Task<GetNewsResult> Handle(GetNewsRequest request, CancellationToken cancellationToken)
        {
            var entities = _context.News.ApplyIsDeletedFilter().AsQueryable();
            if (!string.IsNullOrEmpty(request.Type))
            {
                entities = entities.Where(x => x.Type == request.Type);
            }
            return new GetNewsResult
            {
                Data = entities,
                Message = "Success"
            };
        }
    }
}
