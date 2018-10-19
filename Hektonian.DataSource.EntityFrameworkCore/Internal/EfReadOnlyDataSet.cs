using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hektonian.DataSource.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hektonian.DataSource.EntityFrameworkCore.Internal
{
    internal class EfReadOnlyDataSet<T> : IAsyncReadOnlyDataSet<T>
    where T : class
    {
        private readonly IQueryable<T> _querySet;

        internal EfReadOnlyDataSet(DbContext db, IEnumerable<string> navigationPropertyPaths = null)
        {
            navigationPropertyPaths = navigationPropertyPaths ?? Array.Empty<string>();
            _querySet = navigationPropertyPaths
               .Aggregate(
                    db.Set<T>(),
                    (IQueryable<T> set, string navigationPropertyPath) => set.Include(navigationPropertyPath)
                );
        }

        public async Task<IEnumerable<TOutput>> GetAllAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return await queryBuilder(_querySet)
                        .ToListAsync()
                        .ConfigureAwait(false);
        }

        public Task<TOutput> FirstAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(_querySet)
               .FirstAsync();
        }

        public Task<TOutput> FirstOrDefaultAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(_querySet)
               .FirstOrDefaultAsync();
        }

        public Task<TOutput> SingleAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(_querySet)
               .SingleAsync();
        }

        public Task<TOutput> SingleOrDefaultAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(_querySet)
               .SingleOrDefaultAsync();
        }

        public Task<TOutput> LastAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            throw new NotImplementedException();
        }

        public Task<TOutput> LastOrDefaultAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> condition)
        {
            return await _querySet.Where(condition)
                                  .ToListAsync()
                                  .ConfigureAwait(false);
        }

        public Task<T> FirstAsync(Expression<Func<T, bool>> condition)
        {
            return _querySet.FirstAsync(condition);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> condition)
        {
            return _querySet.FirstOrDefaultAsync(condition);
        }

        public Task<T> SingleAsync(Expression<Func<T, bool>> condition)
        {
            return _querySet.SingleAsync(condition);
        }

        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> condition)
        {
            return _querySet.SingleOrDefaultAsync(condition);
        }

        public Task<T> LastAsync(Expression<Func<T, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> condition)
        {
            throw new NotImplementedException();
        }
    }
}
