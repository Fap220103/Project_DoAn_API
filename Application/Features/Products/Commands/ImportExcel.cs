using Application.Services.CQS.Commands;
using Application.Services.Externals;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands
{
    public class ImportProductsRequest : IRequest<Unit>
    {
        public Stream ExcelFileStream { get; set; }
    }
    public class ImportProductsHandler : IRequestHandler<ImportProductsRequest, Unit>
    {
        private readonly ICommandContext _context;
        private readonly IExcelService _excelImportService;

        public ImportProductsHandler(
            ICommandContext context,
            IExcelService excelImportService)
        {
            _context = context;
            _excelImportService = excelImportService;
        }

        public async Task<Unit> Handle(ImportProductsRequest request, CancellationToken cancellationToken)
        {
            var productDtos = await _excelImportService.ImportProductsAsync(request.ExcelFileStream);

            foreach (var dto in productDtos)
            {
                if (string.IsNullOrWhiteSpace(dto.ProductCode)) continue;

                var exists = await _context.Product
                    .AnyAsync(x => x.ProductCode == dto.ProductCode, cancellationToken);

                if (exists) continue; // Bỏ qua nếu đã tồn tại

                var product = new Product
                {
                    ProductCode = dto.ProductCode,
                    Title = dto.Title,
                    Price = dto.Price ?? 0,
                    Description = dto.Description
                };

                _context.Product.Add(product);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

}
