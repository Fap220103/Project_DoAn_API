using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SecurityManagers.Tokens
{
    public class TokenException : Exception
    {
        public TokenException() { }

        public TokenException(string message) : base(message) { }

        public TokenException(string message, Exception innerException) : base(message, innerException) { }
    }
}
