using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hektonian.DataSource.EntityFrameworkCore.Internal;
using Hektonian.DataSource.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hektonian.DataSource.EntityFrameworkCore
{
    public class EfDataSource<TDbContext> : IAsyncDataSource
    where TDbContext : DbContext
    {
        private readonly TDbContext _db;

        public EfDataSource(TDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
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
            return mutator(new EfMutableDataSource<TDbContext>(_db));
        }

        public Task MutateAsync(Func<IAsyncMutableDataSource, Task> mutator)
        {
            return mutator(new EfMutableDataSource<TDbContext>(_db));
        }

        
    }
}
