using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.PhotoManagers
{
    public class PhotoSettings
    {
        public string CloudName { get; init; }
        public string ApiKey { get; init; }
        public string ApiSecret { get; init; }
    }
}
