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
        public string SmtpUserName { get; init; }
        public string SmtpPassword { get; init; }
        public int SmtpPort { get; init; }
        public string SmtpHost { get; init; }
    }
}
