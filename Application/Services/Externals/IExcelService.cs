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
        Task<byte[]> ExportProductsAsync(List<ProductDto> products);
        Task<List<ProductImportDto>> ImportProductsAsync(Stream fileStream);
    }
    public class ProductImportDto
    {
        public string ProductCode { get; set; }
        public string Title { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
    }
}
