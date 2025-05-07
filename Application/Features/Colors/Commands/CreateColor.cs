using Application.Services.CQS.Commands;
using Application.Services.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Colors.Commands
{
    public class CreateColorResult
    {
        public int Id { get; init; }
        public string Message { get; init; } = null!;
    }

    public class CreateColorRequest : IRequest<CreateColorResult>
    {
        public string Name { get; init; } = null!;
        public string HexCode { get; init; } = null!;
    }

    public class CreateColorValidator : AbstractValidator<CreateColorRequest>
    {
        public CreateColorValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }


    public class CreateColorHandler : IRequestHandler<CreateColorRequest, CreateColorResult>
    {
        private readonly ICommandContext _context;

        public CreateColorHandler(
            ICommandContext context
            )
        {
            _context = context;
        }

        public async Task<CreateColorResult> Handle(CreateColorRequest request, CancellationToken cancellationToken = default)
        {
            var isExist = await _context.Color.AnyAsync(s => s.Name == request.Name, cancellationToken);
            if (isExist)
            {
                return new CreateColorResult
                {
                    Id = 0,
                    Message = "Color already exists"
                };
            }

            var entity = new Color { Name = request.Name, HexCode = request.HexCode };

            _context.Color.Add(entity);
            await _context.SaveChangesAsync();

            return new CreateColorResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
