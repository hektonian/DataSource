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
    internal class EfMutableDataSet<TEntity> : IAsyncMutableDataSet<TEntity>
    where TEntity: class
    {
        private readonly DbContext _db;
        private readonly IQueryable<TEntity> _querySet;

        public EfMutableDataSet(DbContext db, IEnumerable<string> navigationPropertyPaths = null)
        {
            _db = db;
            navigationPropertyPaths = navigationPropertyPaths ?? Array.Empty<string>();
            _querySet = navigationPropertyPaths
               .Aggregate(
                    _db.Set<TEntity>(),
                    (IQueryable<TEntity> set, string navigationPropertyPath) => set.Include(navigationPropertyPath)
                );
        }

        public async Task AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _db.Set<TEntity>().Add(entity);
        }

        public async Task AddRangeAsync(params TEntity[] entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _db.Set<TEntity>().AddRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _db.Set<TEntity>().AddRange(entities);
        }

        public async Task RemoveAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _db.Set<TEntity>().Remove(entity);
        }

        public async Task RemoveRangeAsync(params TEntity[] entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _db.Set<TEntity>().RemoveRange(entities);
        }

        public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _db.Set<TEntity>().RemoveRange(entities);
        }

        public async Task RemoveAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryBuilder)
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            _db.Set<TEntity>().RemoveRange(queryBuilder(_querySet));
        }

        public async Task RemoveAsync(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            _db.Set<TEntity>().RemoveRange(_querySet.Where(condition));
        }

        public async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _db.Set<TEntity>().Update(entity);
        }

        public async Task UpdateRangeAsync(params TEntity[] entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _db.Set<TEntity>().UpdateRange(entities);
            ;
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _db.Set<TEntity>().UpdateRange(entities);
        }

        public Task SaveAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}
