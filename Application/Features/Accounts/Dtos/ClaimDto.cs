using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Dtos
{
    public class ClaimDto
    {
        public string? Id { get; set; }
        public string? Type { get; init; }
        public string? Value { get; init; }

    }
}
