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
    internal class InMemoryMutableDataSet<TEntity> : IAsyncMutableDataSet<TEntity>
    where TEntity: class
    {
        private readonly IInMemoryDataStore _store;

        public InMemoryMutableDataSet(IInMemoryDataStore store)
        {
            _store = store;
        }

        public async Task AddAsync(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _store.Set<TEntity>()
                  .Add(entity);
        }

        public async Task AddRangeAsync(params TEntity[] entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await AddRangeAsync(entities.AsEnumerable());
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            foreach (var entity in entities)
            {
                _store.Set<TEntity>()
                      .Add(entity);
            }
        }

        public async Task RemoveAsync(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _store.Set<TEntity>()
                  .Remove(entity);
        }

        public async Task RemoveRangeAsync(params TEntity[] entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await RemoveRangeAsync(entities.AsEnumerable());
        }

        public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            foreach (var entity in entities)
            {
                _store.Set<TEntity>()
                      .Remove(entity);
            }
        }

        public async Task RemoveAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryBuilder)
        {
            if (queryBuilder is null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            var set = _store.Set<TEntity>();

            foreach (var entity in queryBuilder(set.AsQueryable()))
            {
                set.Remove(entity);
            }
        }

        public async Task RemoveAsync(Expression<Func<TEntity, bool>> condition)
        {
            if (condition is null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var set = _store.Set<TEntity>();

            foreach (var entity in set.AsQueryable()
                                      .Where(condition))
            {
                set.Remove(entity);
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var set = _store.Set<TEntity>();

            if (set.Remove(entity))
            {
                set.Add(entity);
            }
        }

        public async Task UpdateRangeAsync(params TEntity[] entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await UpdateRangeAsync(entities.AsEnumerable());
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            var set = _store.Set<TEntity>();

            foreach (var entity in entities.Intersect(set))
            {
                set.Remove(entity);
                set.Add(entity);
            }
        }

        public async Task SaveAsync()
        {
            // Not really much we can do here
        }
    }
}