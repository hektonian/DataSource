using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hektonian.DataSource.Interfaces;
using Microsoft.EntityFrameworkCore;

#pragma warning disable 1998

namespace Hektonian.DataSource.EntityFrameworkCore.Internal
{
    internal class EfMutableDataSet<T> : IAsyncMutableDataSet<T>
    where T: class
    {
        private readonly DbContext _db;
        private readonly IQueryable<T> _querySet;

        public EfMutableDataSet(DbContext db, IEnumerable<string> navigationPropertyPaths = null)
        {
            _db = db;
            navigationPropertyPaths = navigationPropertyPaths ?? Array.Empty<string>();
            _querySet = navigationPropertyPaths
               .Aggregate(
                    _db.Set<T>(),
                    (IQueryable<T> set, string navigationPropertyPath) => set.Include(navigationPropertyPath)
                );
        }

        public async Task AddAsync(T entity)
        {
            _db.Add(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            _db.AddRange(entities);
        }

        public async Task RemoveAsync(T entity)
        {
            _db.Remove(entity);
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }

        public async Task RemoveAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder)
        {
            _db.RemoveRange(queryBuilder(_querySet));
        }

        public async Task RemoveAsync(Expression<Func<T, bool>> condition)
        {
            _db.RemoveRange(_querySet.Where(condition));
        }

        public async Task UpdateAsync(T entity)
        {
            _db.Update(entity);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _db.UpdateRange(entities);
        }

        public Task SaveAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}
