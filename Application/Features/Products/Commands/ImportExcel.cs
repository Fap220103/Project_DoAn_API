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
        private readonly ICommonService _commonService;

        public ImportProductsHandler(
            ICommandContext context,
            IExcelService excelImportService,
            ICommonService commonService)
        {
            _context = context;
            _excelImportService = excelImportService;
            _commonService = commonService;
        }

        public async Task<Unit> Handle(ImportProductsRequest request, CancellationToken cancellationToken)
        {
            var productDtos = await _excelImportService.ImportProductsAsync(request.ExcelFileStream);

            foreach (var dto in productDtos)
            {
                if (string.IsNullOrWhiteSpace(dto.ProductCode)) continue;

                var exists = await _context.Product
                    .AnyAsync(x => x.ProductCode == dto.ProductCode, cancellationToken);

                if (exists) continue;

                // Tìm danh mục theo tên
                var category = await _context.ProductCategory
                    .FirstOrDefaultAsync(c => c.Title == dto.ProductCategoryName, cancellationToken);

                if (category == null)
                {
                    // Có thể bỏ qua, hoặc báo lỗi nếu danh mục không tồn tại
                    continue;
                }

                var product = new Product(
                                userId: null,                             
                                title: dto.Title ?? "",                   
                                alias: _commonService.FilterChar(dto.Title),                              
                                description: dto.Description,             
                                seoTitle: dto.Title,                           
                                seoDescription: null,                   
                                seoKeywords: null,                      
                                image: "",                              
                                detail: dto.Detail ?? "",                 
                                originalPrice: dto.OriginalPrice,        
                                price: dto.Price,                       
                                salePercent: dto.SalePercent,           
                                productCategoryId: category?.Id ?? null   
                            );
                product.ProductCode = dto.ProductCode;

                _context.Product.Add(product);
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                throw new Exception($"Lỗi khi lưu dữ liệu vào database: {innerMessage}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không xác định khi lưu dữ liệu: {ex.Message}", ex);
            }
            return Unit.Value;
        }

    }

}
