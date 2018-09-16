using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hektonian.DataSource.InMemory.Interfaces;
using Hektonian.DataSource.InMemory.Internal;
using Hektonian.DataSource.Interfaces;

namespace Hektonian.DataSource.InMemory
{
    public class InMemoryDataSource : IAsyncDataSource, IAsyncMutableDataSource
    {
        private readonly IInMemoryDataStore _store;

        public InMemoryDataSource(IInMemoryDataStore store = null)
        {
            _store = store;
        }

        public IAsyncReadOnlyDataSet<T> Set<T>(IEnumerable<string> includes)
        where T: class
        {
            return new InMemoryDataSet<T>(_store, includes);
        }

        public IAsyncReadOnlyDataSet<T> Set<T>(params string[] includes)
        where T: class
        {
            return new InMemoryDataSet<T>(_store, includes);
        }

        public Task<T> MutateAsync<T>(Func<IAsyncMutableDataSource, Task<T>> mutator)
        where T: class
        {
            return mutator(this);
        }

        public Task MutateAsync(Func<IAsyncMutableDataSource, Task> mutator)
        {
            return mutator(this);
        }

        public IAsyncMutableDataSet<T> Mutate<T>(IEnumerable<string> includes)
        where T: class
        {
            throw new NotImplementedException();
        }

        public IAsyncMutableDataSet<T> Mutate<T>(params string[] includes)
        where T: class
        {
            throw new NotImplementedException();
        }
    }
}