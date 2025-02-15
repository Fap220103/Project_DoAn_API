using Application.Services.CQS.Queries;
using Application.Services.Repositories;
using Domain.Bases;
using Domain.Interfaces;
using Infrastructure.DataAccessManagers.EFCores.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccessManagers.EFCores.Repositories
{
    public class BaseCommandRepository<T> : IBaseCommandRepository<T> where T : BaseEntity, IAggregateRoot
    {
        protected readonly CommandContext _context;
        public BaseCommandRepository(CommandContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(entity, cancellationToken);
        }

        public void Create(T entity)
        {

            _context.Add(entity);
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Update(entity);
        }

        public void Purge(T entity)
        {
            _context.Remove(entity);
        }

        public virtual async Task<T?> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Set<T>()
                .ApplyIsDeletedFilter()
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            return entity;
        }

        public virtual T? Get(string id)
        {
            var entity = _context.Set<T>()
                .ApplyIsDeletedFilter()
                .SingleOrDefault(x => x.Id == id);

            return entity;
        }

        public virtual IQueryable<T> GetQuery()
        {
            var query = _context.Set<T>().AsQueryable();

            return query;
        }


    }
}
