using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hektonian.DataSource.EntityFrameworkCore.Internal;
using Hektonian.DataSource.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hektonian.DataSource.EntityFrameworkCore.Internal
{
    internal class EfDataSource<TDbContext> : IAsyncDataSource
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
            if (includes == null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            return new EfReadOnlyDataSet<T>(_db, includes);
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

            return new EfReadOnlyDataSet<T>(_db, includes);
        }

        public Task<T> MutateAsync<T>(Func<IAsyncMutableDataSource, Task<T>> mutator)
        where T: class
        {
            if (mutator == null)
            {
                throw new ArgumentNullException(nameof(mutator));
            }

            return mutator(new EfMutableDataSource<TDbContext>(_db));
        }

        public Task MutateAsync(Func<IAsyncMutableDataSource, Task> mutator)
        {
            if (mutator == null)
            {
                throw new ArgumentNullException(nameof(mutator));
            }

            return mutator(new EfMutableDataSource<TDbContext>(_db));
        }

        
    }
}
