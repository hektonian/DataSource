using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hektonian.DataSource.EntityFrameworkCore
{
    public class EfDataSource<TDbContext> : IAsyncDataSource, IAsyncMutableDataSource
    where TDbContext : DbContext
    {
        private readonly TDbContext _db;

        public EfDataSource(TDbContext db)
        {
            _db = db;
        }

        public IAsyncReadOnlyDataSet<T> Set<T>(IEnumerable<string> includes)
        where T: class
        {
            return new EfReadOnlyDataSet<T>(_db, includes);
        }

        public IAsyncReadOnlyDataSet<T> Set<T>(params string[] includes)
        where T: class
        {
            return new EfReadOnlyDataSet<T>(_db, includes);
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
            return new EfAsyncMutableDataSet<T>(_db, includes);
        }

        public IAsyncMutableDataSet<T> Mutate<T>(params string[] includes)
        where T: class
        {
            return new EfAsyncMutableDataSet<T>(_db, includes);
        }
    }
}
