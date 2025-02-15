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
            ""ParentName"": ""Dashboard"",
            ""ParentCaption"": ""Dashboard"",
            ""ParentUrl"": ""#"",
            ""Children"": [
              {
                ""Name"": ""Dashboard"",
                ""Caption"": ""Dashboard"",
                ""Url"": ""/Dashboards""
              }
            ]
          },
          {
            ""ParentName"": ""Product"",
            ""ParentCaption"": ""Sản phẩm"",
            ""ParentUrl"": ""#"",
            ""Children"": [
              {
                ""Name"": ""ProductList"",
                ""Caption"": ""Danh sách sản phẩm"",
                ""Url"": ""/danh-sach-san-pham""
              },
              {
                ""Name"": ""ProductCategory"",
                ""Caption"": ""Danh mục sản phẩm"",
                ""Url"": ""/danh-muc-san-pham""
              },
              {
                ""Name"": ""Inventory"",
                ""Caption"": ""Tồn kho"",
                ""Url"": ""/ton-kho""
              }
            ]
          },
          {
            ""ParentName"": ""Order"",
            ""ParentCaption"": ""Đơn hàng"",
            ""ParentUrl"": ""#"",
            ""Children"": [
              {
                ""Name"": ""OrderList"",
                ""Caption"": ""Danh sách đơn hàng"",
                ""Url"": ""/danh-sach-don-hang""
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
                ""Caption"": ""Thông tin người dùng"",
                ""Url"": ""/thong-tin-nguoi-dung""
              },
              {
                ""Name"": ""StaffProfile"",
                ""Caption"": ""Thông tin nhân viên"",
                ""Url"": ""/thong-tin-nhan-vien""
              }
            ]
          },
          {
            ""ParentName"": ""Sale"",
            ""ParentCaption"": ""Khuyến mãi"",
            ""ParentUrl"": ""#"",
            ""Children"": [
              {
                ""Name"": ""SaleCode"",
                ""Caption"": ""Mã khuyến mãi"",
                ""Url"": ""/ma-khuyen-mai""
              },
              {
                ""Name"": ""SaleProgram"",
                ""Caption"": ""Chương trình khuyến mãi"",
                ""Url"": ""/chuong-trinh-khuyen-mai""
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
                ""Url"": ""/doanh-thu""
              },
              {
                ""Name"": ""BestSellingProduct"",
                ""Caption"": ""Sản phẩm bán chạy"",
                ""Url"": ""/san-pham-ban-chay""
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
                ""Url"": ""/tin-nhan""
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
                ""Url"": ""/phan-hoi""
              },
              {
                ""Name"": ""Reviews"",
                ""Caption"": ""Đánh giá"",
                ""Url"": ""/danh-gia""
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
                ""Url"": ""/cau-hinh""
              },
              {
                ""Name"": ""Banner"",
                ""Caption"": ""Banner"",
                ""Url"": ""/banner""
              }
            ]
          },
          {
            ""ParentName"": ""RoleMembership"",
            ""ParentCaption"": ""Role & Membership"",
            ""ParentUrl"": ""#"",
            ""Children"": [
              {
                ""Name"": ""Role"",
                ""Caption"": ""Role"",
                ""Url"": ""/Roles""
              },
              {
                ""Name"": ""Claim"",
                ""Caption"": ""Claim"",
                ""Url"": ""/Claims""
              },
              {
                ""Name"": ""Member"",
                ""Caption"": ""Member"",
                ""Url"": ""/Members""
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
