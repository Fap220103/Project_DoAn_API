using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.CQS.Queries
{
    public interface IQueryContext : IEntityDbSet
    {
        IQueryable<T> Set<T>() where T : class;
    }
}
