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
                ""Name"": ""Inventory"",
                ""Caption"": ""Tồn kho"",
                ""Url"": ""/admin/inventory""
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
                ""Name"": ""SaleProgram"",
                ""Caption"": ""Khuyến mãi"",
                ""Url"": ""/admin/sale""
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
            ""ParentName"": ""Support"",
            ""ParentCaption"": ""Hỗ trợ khách hàng"",
            ""ParentUrl"": ""#"",
            ""Children"": [
              {
                ""Name"": ""Messenger"",
                ""Caption"": ""Tin nhắn"",
                ""Url"": ""/admin/messenger""
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
          },
          {
            ""ParentName"": ""RoleClaim"",
            ""ParentCaption"": ""Role & Claim"",
            ""ParentUrl"": ""#"",
            ""Children"": [
              {
                ""Name"": ""Role"",
                ""Caption"": ""Role"",
                ""Url"": ""/admin/roles""
              },
              {
                ""Name"": ""Claim"",
                ""Caption"": ""Claim"",
                ""Url"": ""/admin/claims""
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
