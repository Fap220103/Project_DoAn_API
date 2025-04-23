using Domain.Bases;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Repositories
{
    public interface IBaseCommandRepository<T> where T : BaseEntity
    {
        Task CreateAsync(T entity, CancellationToken cancellationToken = default);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Purge(T entity);

        Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        T? Get(string id);

        IQueryable<T> GetQuery();
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    }
}
