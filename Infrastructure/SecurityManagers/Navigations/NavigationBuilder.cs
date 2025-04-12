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
                ""Name"": ""ProductList"",
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
                ""Caption"": ""Khách hàng"",
                ""Url"": ""/admin/userprofile""
              },
              {
                ""Name"": ""StaffProfile"",
                ""Caption"": ""Nhân viên"",
                ""Url"": ""/admin/staffprofile""
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
                ""Url"": ""/admin/saleprogram""
              }
            ]
          },
          {
            ""ParentName"": ""Report"",
            ""ParentCaption"": ""Báo cáo"",
            ""ParentUrl"": ""#"",
            ""Children"": [
              {
                ""Name"": ""Revenue"",
                ""Caption"": ""Doanh thu"",
                ""Url"": ""/admin/revenue""
              },
              {
                ""Name"": ""BestSellingProduct"",
                ""Caption"": ""Sản phẩm bán chạy"",
                ""Url"": ""/admin/bestsellingproduct""
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
            ""ParentName"": ""FeedbackReviews"",
            ""ParentCaption"": ""Phản hồi và đánh giá"",
            ""ParentUrl"": ""#"",
            ""Children"": [
              {
                ""Name"": ""Feedback"",
                ""Caption"": ""Phản hồi"",
                ""Url"": ""/admin/feedback""
              },
              {
                ""Name"": ""Reviews"",
                ""Caption"": ""Đánh giá"",
                ""Url"": ""/admin/reviews""
              }
            ]
          },
          {
            ""ParentName"": ""Settings"",
            ""ParentCaption"": ""Cài đặt hệ thống"",
            ""ParentUrl"": ""#"",
            ""Children"": [
              {
                ""Name"": ""Config"",
                ""Caption"": ""Cấu hình"",
                ""Url"": ""/admin/config""
              },
              {
                ""Name"": ""Banner"",
                ""Caption"": ""Banner"",
                ""Url"": ""/admin/banner""
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
