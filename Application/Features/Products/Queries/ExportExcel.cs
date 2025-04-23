using Application.Services.CQS.Queries;
using Application.Services.Externals;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries
{
    public class ExportProductsToExcelRequest : IRequest<byte[]>
    {
    }
    public class ExportProductsToExcelHandler : IRequestHandler<ExportProductsToExcelRequest, byte[]>
    {
        private readonly IQueryContext _context;
        private readonly IExcelService _excelExportService;

        public ExportProductsToExcelHandler(IQueryContext context, IExcelService excelExportService)
        {
            _context = context;
            _excelExportService = excelExportService;
        }

        public async Task<byte[]> Handle(ExportProductsToExcelRequest request, CancellationToken cancellationToken)
        {
            var products = await _context.Product
            .Select(p => new ProductDto
            {
                ProductCode = p.ProductCode,
                Title = p.Title,
                Price = p.Price,
                Description = p.Description
            })
            .ToListAsync(cancellationToken);

            return await _excelExportService.ExportProductsAsync(products);
        }
    }
}
