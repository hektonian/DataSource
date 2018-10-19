using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hektonian.DataSource.InMemory.Interfaces;
using Hektonian.DataSource.Interfaces;

#pragma warning disable 1998

namespace Hektonian.DataSource.InMemory.Internal
{
    internal class InMemoryReadOnlyDataSet<TEntity> : IAsyncReadOnlyDataSet<TEntity> where TEntity : class
    {
        private readonly IInMemoryDataStore _store;
        private readonly IEnumerable<string> _includes;

        internal InMemoryReadOnlyDataSet(IInMemoryDataStore store, IEnumerable<string> includes)
        {
            _store = store;
            _includes = includes;
        }

        public async Task<IEnumerable<TOutput>> GetAllAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> queryBuilder)
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return queryBuilder(
                    _store.Set<TEntity>()
                          .AsQueryable()
                )
               .ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return _store.Set<TEntity>()
                         .AsQueryable()
                         .Where(condition)
                         .ToList();
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return _store.Set<TEntity>()
                         .AsQueryable()
                         .Single(condition);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return _store.Set<TEntity>()
                         .AsQueryable()
                         .SingleOrDefault(condition);
        }

        public async Task<TOutput> FirstAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> queryBuilder)
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return queryBuilder(
                    _store.Set<TEntity>()
                          .AsQueryable()
                )
               .First();
        }

        public async Task<TOutput> FirstOrDefaultAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> queryBuilder)
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return queryBuilder(
                    _store.Set<TEntity>()
                          .AsQueryable()
                )
               .FirstOrDefault();
        }

        public async Task<TOutput> SingleAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> queryBuilder)
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return queryBuilder(
                    _store.Set<TEntity>()
                          .AsQueryable()
                )
               .Single();
        }

        public async Task<TOutput> SingleOrDefaultAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> queryBuilder)
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return queryBuilder(
                    _store.Set<TEntity>()
                          .AsQueryable()
                )
               .SingleOrDefault();
        }
    }
}