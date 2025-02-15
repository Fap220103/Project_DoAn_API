using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SecurityManagers.Tokens
{
    public class TokenSettings
    {
        public string Key { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public int ExpireInMinute { get; init; }
        public double ClockSkewInMinute { get; init; }
    }
}
