using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hektonian.DataSource.EntityFrameworkCore
{
    public class EfAsyncMutableDataSet<T> : IAsyncMutableDataSet<T>
    where T: class
    {
        private readonly DbContext _db;
        private readonly IQueryable<T> _querySet;

        public EfAsyncMutableDataSet(DbContext db, IEnumerable<string> navigationPropertyPaths = null)
        {
            _db = db;
            navigationPropertyPaths = navigationPropertyPaths ?? Array.Empty<string>();
            _querySet = navigationPropertyPaths
               .Aggregate(
                    _db.Set<T>(),
                    (IQueryable<T> set, string navigationPropertyPath) => set.Include(navigationPropertyPath)
                );
        }

        public Task<T> AddAsync(T entity)
        {
            _db.Add(entity);

            return SaveAndReloadAsync(entity);
        }

        public Task AddRangeAsync(IEnumerable<T> entities)
        {
            _db.AddRange(entities);

            return _db.SaveChangesAsync();
        }

        public Task RemoveAsync(T entity)
        {
            _db.Remove(entity);

            return _db.SaveChangesAsync();
        }

        public Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);

            return _db.SaveChangesAsync();
        }

        public Task RemoveAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder)
        {
            _db.RemoveRange(queryBuilder(_querySet));

            return _db.SaveChangesAsync();
        }

        public Task<T> UpdateAsync(T entity)
        {
            _db.Update(entity);

            return SaveAndReloadAsync(entity);
        }

        public Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _db.UpdateRange(entities);
            return _db.SaveChangesAsync();
        }

        private async Task<T> SaveAndReloadAsync(T entity)
        {
            await _db.SaveChangesAsync()
                     .ConfigureAwait(false);

            await _db.Entry(entity)
                     .ReloadAsync()
                     .ConfigureAwait(false);

            return entity;
        }
    }
}
