using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.CQS.Queries
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyIsDeletedFilter<T>(this IQueryable<T> query, bool isDeleted = false) where T : class
        {
            if (typeof(IHasIsDeleted).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(x => (x as IHasIsDeleted)!.IsDeleted == isDeleted);
            }

            return query;
        }


        public static IQueryable<T> ApplyFilterWithPaging<T>(
            this IQueryable<T> query,
            int totalRecords,
            int skip,
            int top
        ) where T : class
        {

          

            //if (top == 0)
            //{
            //    throw new ApplicationException("Top must not be zero.");
            //}

            return query.Skip(skip).Take(top);
        }

        public static IQueryable<T> ApplySorting<T>(
            this IQueryable<T> query
        ) where T : class
        {

            //query = queryOptions.ApplySorting(query);

            return query;
        }

        public static IQueryable<T> ApplyFilter<T>(
            this IQueryable<T> query
        ) where T : class
        {

            //query = queryOptions.ApplyFilter(query);

            return query;
        }

        public static IQueryable<T> ApplyPaging<T>(
            this IQueryable<T> query,
            int skip,
            int top
        ) where T : class
        {

            

            if (top == 0)
            {
                throw new ApplicationException("Top must not be zero.");
            }

            return query;
        }

        public static IQueryable<T> ApplyRelationFilter<T, TNavigation>(this IQueryable<T> query, string queryValue, string navigationPropertyName, IQueryContext context)
             where T : class
             where TNavigation : class
        {
            var navigationSet = context.Set<TNavigation>().AsNoTracking();

            // Retrieve IDs based on the navigation entity type
            var ids = navigationSet
                .Where(e => EF.Property<string>(e, "Name").Contains(queryValue))
                .Select(e => EF.Property<string>(e, "Id"))
                .ToList();

            // Build the predicate dynamically
            var parameter = Expression.Parameter(typeof(T), "x");
            var navigationProperty = Expression.Property(parameter, navigationPropertyName);
            var containsMethod = typeof(List<string>).GetMethod("Contains", new[] { typeof(string) });
            if (containsMethod == null)
            {
                throw new ApplicationException($"Method 'Contains' not found on List<string>");
            }
            var containsExpression = Expression.Call(Expression.Constant(ids), containsMethod, navigationProperty);
            var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);

            return query.Where(lambda);
        }


    }
}
