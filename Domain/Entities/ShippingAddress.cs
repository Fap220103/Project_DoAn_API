using Domain.Bases;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShippingAddress : BaseEntity
    {
        public string UserId { get; set; } 
        public string RecipientName { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine { get; set; }
        public string ProvinceCode { get; set; }
        public string Province { get; set; }
        public string DistrictCode { get; set; }
        public string District { get; set; }
        public string WardCode { get; set; }
        public string Ward { get; set; }
        public bool IsDefault { get; set; }
        public ShippingAddress() : base() { } //for EF Core
        public ShippingAddress(
            string userId,
            string recipientName,
            string phoneNumber,
            string addressLine,
            string province,
            string district,
            string ward,
            string provinceCode,
            string districtCode,
            string wardCode,
            bool isDefault
        )
        {
            UserId = userId.Trim();
            RecipientName = recipientName.Trim();
            PhoneNumber = phoneNumber.Trim();
            AddressLine = addressLine.Trim();
            Province = province.Trim();
            District = district.Trim();
            Ward = ward.Trim();
            ProvinceCode = provinceCode.Trim();
            DistrictCode = districtCode.Trim();
            WardCode = wardCode.Trim();
            IsDefault = isDefault;
        }
        public void Update(
            string recipientName,
            string phoneNumber,
            string addressLine,
            string province,
            string district,
            string ward,
            string provinceCode,
            string districtCode,
            string wardCode,
            bool isDefault
        )
        {
            RecipientName = recipientName.Trim();
            PhoneNumber = phoneNumber.Trim();
            AddressLine = addressLine.Trim();
            Province = province.Trim();
            District = district.Trim();
            Ward = ward.Trim();
            ProvinceCode = provinceCode.Trim();
            DistrictCode = districtCode.Trim();
            WardCode = wardCode.Trim();
            IsDefault = isDefault;
        }
    }
}
