using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EmailManagers
{
    public class EmailSettings
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
