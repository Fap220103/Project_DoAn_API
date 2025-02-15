using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SecurityManagers.Navigations
{
    public class NavigationException : Exception
    {
        public NavigationException() { }

        public NavigationException(string message) : base(message) { }

        public NavigationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
