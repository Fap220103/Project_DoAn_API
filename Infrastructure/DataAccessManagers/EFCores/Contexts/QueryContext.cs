using Application.Services.CQS.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccessManagers.EFCores.Contexts
{
    public class QueryContext : DataContext, IQueryContext
    {
        public QueryContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public new IQueryable<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }
    }
}
