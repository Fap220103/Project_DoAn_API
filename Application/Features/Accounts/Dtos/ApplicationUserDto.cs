using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Dtos
{
    public class ApplicationUserDto
    {
        public string? Id { get; set; }
        public string? UserName { get; init; }
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
        public string? ProfilePictureName { get; init; }
        public bool EmailConfirmed { get; init; }
        public int Status { get; init; }
        public bool IsBlocked { get; init; }
        public DateTime? CreatedAt { get; init; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
