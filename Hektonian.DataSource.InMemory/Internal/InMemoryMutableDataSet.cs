using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hektonian.DataSource.InMemory.Interfaces;
using Hektonian.DataSource.Interfaces;

#pragma warning disable 1998

namespace Hektonian.DataSource.InMemory.Internal
{
    internal class InMemoryMutableDataSet<T> : IAsyncMutableDataSet<T>
    where T: class
    {
        private readonly IInMemoryDataStore _store;

        public InMemoryMutableDataSet(IInMemoryDataStore store)
        {
            _store = store;
        }

        public async Task AddAsync(T entity)
        {
            _store.Set<T>()
                  .Add(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _store.Set<T>()
                      .Add(entity);
            }
        }

        public async Task RemoveAsync(T entity)
        {
            _store.Set<T>()
                  .Remove(entity);
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _store.Set<T>()
                      .Remove(entity);
            }
        }

        public async Task RemoveAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder)
        {
            var set = _store.Set<T>();

            foreach (var entity in queryBuilder(set.AsQueryable()))
            {
                set.Remove(entity);
            }
        }
            }
        }

        public async Task UpdateAsync(T entity)
        {
            var set = _store.Set<T>();

            if (set.Remove(entity))
            {
                set.Add(entity);
            }
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            var set = _store.Set<T>();

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