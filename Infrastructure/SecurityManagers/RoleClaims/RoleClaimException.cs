using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SecurityManagers.RoleClaims
{
    public class RoleClaimException : Exception
    {
        public RoleClaimException() { }

        public RoleClaimException(string message) : base(message) { }

        public RoleClaimException(string message, Exception innerException) : base(message, innerException) { }
    }
}
