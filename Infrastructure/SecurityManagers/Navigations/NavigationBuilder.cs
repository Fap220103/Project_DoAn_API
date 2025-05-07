using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.SecurityManagers.Navigations
{
    public static class NavigationBuilder
    {
        private static readonly string jsonNavigation = @"
        [   
          {
            ""ParentName"": ""Product"",
            ""ParentCaption"": ""Sản phẩm"",
            ""ParentUrl"": ""#"",
            ""Children"": [
              {
                ""Name"": ""Product"",
                ""Caption"": ""Sản phẩm"",
                ""Url"": ""/admin/product""
              },
              {
                ""Name"": ""ProductCategory"",
                ""Caption"": ""Danh mục"",
                ""Url"": ""/admin/productcategory"" 
              },
              {
                ""Name"": ""ProductVariant"",
                ""Caption"": ""Biến thể"",
                ""Url"": ""/admin/productvariant""
              },
              {
                ""Name"": ""Color"",
                ""Caption"": ""Màu sắc"",
                ""Url"": ""/admin/color""
              },
              {
                ""Name"": ""Size"",
                ""Caption"": ""Kích cỡ"",
                ""Url"": ""/admin/size""
              }
            ]
          },
          {
            ""ParentName"": ""Order"",
            ""ParentCaption"": ""Đơn hàng"",
            ""ParentUrl"": ""#"",
            ""Children"": [
              {
                ""Name"": ""Order"",
                ""Caption"": ""Đơn hàng"",
                ""Url"": ""/admin/order""
              }
            ]
          },
          
          {
            ""ParentName"": ""Profile"",
            ""ParentCaption"": ""Profile"",
            ""ParentUrl"": ""#"",
            ""Children"": [
              {
                ""Name"": ""UserProfile"",
                ""Caption"": ""Người dùng"",
                ""Url"": ""/admin/userprofile""
              }
            ]
          },
          {
            ""ParentName"": ""Sale"",
            ""ParentCaption"": ""Khuyến mãi"",
            ""ParentUrl"": ""#"",
            ""Children"": [           
              {
                ""Name"": ""Discount"",
                ""Caption"": ""Khuyến mãi"",
                ""Url"": ""/admin/discount""
              }
            ]
          },
          {
            ""ParentName"": ""Report"",
            ""ParentCaption"": ""Báo cáo thống kê"",
            ""ParentUrl"": ""#"",
            ""Children"": [
              {
                ""Name"": ""Report"",
                ""Caption"": ""Báo cáo - thống kê"",
                ""Url"": ""/admin/report""
              }
            ]
          },
         
          {
            ""ParentName"": ""Settings"",
            ""ParentCaption"": ""Cài đặt hệ thống"",
            ""ParentUrl"": ""#"",
            ""Children"": [
              {
                ""Name"": ""Setting"",
                ""Caption"": ""Cài đặt"",
                ""Url"": ""/admin/setting""
              }
            ]
          }
        ]
        ";

        public static List<NavigationContributor> BuildFinalNavigations()
        {
            var contributors = JsonSerializer.Deserialize<List<NavigationContributor>>(jsonNavigation, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return contributors ?? new List<NavigationContributor>();
        }
    }
}
