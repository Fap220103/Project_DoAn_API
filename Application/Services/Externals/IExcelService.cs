using Application.Features.Products.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Externals
{
    public interface IExcelService
    {
        Task<byte[]> ExportProductsAsync(List<ProductExportDto> products);
        Task<List<ProductImportDto>> ImportProductsAsync(Stream fileStream);
    }
    public class ProductImportDto
    {
        public string ProductCode { get; set; }
        public string Title { get; set; }
        public decimal OriginalPrice { get; init; }
        public decimal Price { get; init; }
        public int SalePercent { get; init; }
        public string Description { get; set; }
        public string Detail { get; set; }

        // Người dùng nhập tên danh mục
        public string ProductCategoryName { get; set; }
    }
}
