using Application.Features.Products.Queries;
using Application.Services.Externals;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.ExcelManagers.ExcelService;

namespace Infrastructure.ExcelManagers
{
    public class ExcelService : IExcelService
    {
        public async Task<byte[]> ExportProductsAsync(List<ProductExportDto> products)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Products");

            worksheet.Cells[1, 1].Value = "Mã SP";
            worksheet.Cells[1, 2].Value = "Tên SP";
            worksheet.Cells[1, 3].Value = "Giá nhập";
            worksheet.Cells[1, 4].Value = "Giá bán";
            worksheet.Cells[1, 5].Value = "Giá khuyến mãi";
            worksheet.Cells[1, 6].Value = "Mô tả";

            for (int i = 0; i < products.Count; i++)
            {
                var p = products[i];
                worksheet.Cells[i + 2, 1].Value = p.ProductCode;
                worksheet.Cells[i + 2, 2].Value = p.Title;
                worksheet.Cells[i + 2, 3].Value = p.OriginalPrice;
                worksheet.Cells[i + 2, 4].Value = p.Price;
                worksheet.Cells[i + 2, 5].Value = p.PriceSale;
                worksheet.Cells[i + 2, 6].Value = p.Description;
            }

            return await Task.FromResult(package.GetAsByteArray());
        }
        public async Task<List<ProductImportDto>> ImportProductsAsync(Stream fileStream)
        {
            var products = new List<ProductImportDto>();

            using var package = new ExcelPackage(fileStream);
            var worksheet = package.Workbook.Worksheets[0];
            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var dto = new ProductImportDto
                {
                    ProductCode = worksheet.Cells[row, 1].Text,
                    Title = worksheet.Cells[row, 2].Text,
                    OriginalPrice = decimal.TryParse(worksheet.Cells[row, 3].Text, out var oriPrice) ? oriPrice : 0,
                    Price = decimal.TryParse(worksheet.Cells[row, 4].Text, out var price) ? price : 0,
                    SalePercent = int.TryParse(worksheet.Cells[row, 5].Text, out var sale) ? sale : 0,
                    Description = worksheet.Cells[row, 6].Text,
                    Detail = worksheet.Cells[row, 7].Text,
                    ProductCategoryName = worksheet.Cells[row, 8].Text
                };

                products.Add(dto);
            }

            return await Task.FromResult(products);
        }

    }
}
