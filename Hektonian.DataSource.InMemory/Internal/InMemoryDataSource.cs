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

        public IAsyncReadOnlyDataSet<T> Set<T>(IEnumerable<string> includes)
        where T: class
        {
            if (includes == null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            return new InMemoryReadOnlyDataSet<T>(_store, includes);
        }

        public IAsyncReadOnlyDataSet<T> Set<T>(params string[] includes)
        where T: class
        {
            if (includes == null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            if (includes.Any(include => include == null))
            {
                throw new ArgumentNullException();
            }

            return new InMemoryReadOnlyDataSet<T>(_store, includes);
        }

        public Task<T> MutateAsync<T>(Func<IAsyncMutableDataSource, Task<T>> mutator)
        where T: class
        {
            if (mutator == null)
            {
                throw new ArgumentNullException(nameof(mutator));
            }

            return mutator(this);
        }

        public Task MutateAsync(Func<IAsyncMutableDataSource, Task> mutator)
        {
            if (mutator == null)
            {
                throw new ArgumentNullException(nameof(mutator));
            }

            return mutator(this);
        }

        public IAsyncMutableDataSet<T> Mutate<T>(IEnumerable<string> includes)
        where T: class
        {
            if (includes == null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            return new InMemoryMutableDataSet<T>(_store);
        }

        public IAsyncMutableDataSet<T> Mutate<T>(params string[] includes)
        where T: class
        {
            if (includes == null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            if (includes.Any(include => include == null))
            {
                throw new ArgumentNullException();
            }

            return new InMemoryMutableDataSet<T>(_store);
        }
    }
}