using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hektonian.DataSource.InMemory.Interfaces;
using Hektonian.DataSource.InMemory.Internal;
using Hektonian.DataSource.Interfaces;

namespace Hektonian.DataSource.InMemory.Internal
{
    internal class InMemoryDataSource : IAsyncDataSource, IAsyncMutableDataSource
    {
        private readonly IInMemoryDataStore _store;

        public InMemoryDataSource(IInMemoryDataStore store = null)
        {
            _store = store ?? new InMemoryDataStore();
        }

        public IAsyncReadOnlyDataSet<TEntity> Set<TEntity>(IEnumerable<string> includes)
        where TEntity: class
        {
            if (includes == null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            return new InMemoryReadOnlyDataSet<TEntity>(_store, includes);
        }

        public IAsyncReadOnlyDataSet<TEntity> Set<TEntity>(params string[] includes)
        where TEntity: class
        {
            if (includes == null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            if (includes.Any(include => include == null))
            {
                throw new ArgumentNullException();
            }

            return new InMemoryReadOnlyDataSet<TEntity>(_store, includes);
        }

        public Task<TEntity> MutateAsync<TEntity>(Func<IAsyncMutableDataSource, Task<TEntity>> mutator)
        where TEntity: class
        {
            if (mutator == null)
            {
                throw new ArgumentNullException(nameof(mutator));
            }

            return mutator(this);
        }

        public async Task MutateAsync(Func<IAsyncMutableDataSource, Task> mutator)
        {
            if (mutator == null)
            {
                throw new ArgumentNullException(nameof(mutator));
            }

            _store.BeginTransaction();

            try
            {
                await mutator(this);
            }
            catch (Exception)
            {
                _store.Rollback();
            }

            _store.Commit();
        }

        public IAsyncMutableDataSet<TEntity> Mutate<TEntity>(IEnumerable<string> includes)
        where TEntity: class
        {
            if (includes == null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            return new InMemoryMutableDataSet<TEntity>(_store);
        }

        public IAsyncMutableDataSet<TEntity> Mutate<TEntity>(params string[] includes)
        where TEntity: class
        {
            if (includes == null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            if (includes.Any(include => include == null))
            {
                throw new ArgumentNullException();
            }

            return new InMemoryMutableDataSet<TEntity>(_store);
        }
    }
}