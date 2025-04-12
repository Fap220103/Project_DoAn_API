using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    public class BaseParams
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? Order { get; set; }
    }
}
