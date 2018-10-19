using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hektonian.DataSource.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hektonian.DataSource.EntityFrameworkCore.Internal
{
    internal class EfReadOnlyDataSet<TEntity> : IAsyncReadOnlyDataSet<TEntity>
    where TEntity : class
    {
        private readonly IQueryable<TEntity> _querySet;

        internal EfReadOnlyDataSet(DbContext db, IEnumerable<string> navigationPropertyPaths = null)
        {
            navigationPropertyPaths = navigationPropertyPaths ?? Array.Empty<string>();
            _querySet = navigationPropertyPaths
               .Aggregate(
                    db.Set<TEntity>().AsNoTracking(),
                    (set, navigationPropertyPath) => set.Include(navigationPropertyPath)
                );
        }

        public async Task<IEnumerable<TOutput>> GetAllAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> queryBuilder)
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return await queryBuilder(_querySet)
                        .ToListAsync()
                        .ConfigureAwait(false);
        }
        
        public Task<TOutput> FirstAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> queryBuilder)
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return queryBuilder(_querySet)
               .FirstAsync();
        }

        public Task<TOutput> FirstOrDefaultAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> queryBuilder)
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return queryBuilder(_querySet)
               .FirstOrDefaultAsync();
        }

        public Task<TOutput> SingleAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> queryBuilder)
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return queryBuilder(_querySet)
               .SingleAsync();
        }

        public Task<TOutput> SingleOrDefaultAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> queryBuilder)
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            return queryBuilder(_querySet)
               .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return await _querySet.Where(condition)
                                  .ToListAsync()
                                  .ConfigureAwait(false);
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return _querySet.SingleAsync(condition);
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return _querySet.SingleOrDefaultAsync(condition);
        }
    }
}
