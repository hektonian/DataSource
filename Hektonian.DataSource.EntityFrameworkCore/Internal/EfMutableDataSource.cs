using System.Collections.Generic;
using Hektonian.DataSource.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hektonian.DataSource.EntityFrameworkCore.Internal
{
    internal class EfMutableDataSource<TDbContext> : IAsyncMutableDataSource
    where TDbContext : DbContext

    {
        private readonly TDbContext _db;

        public EfMutableDataSource(TDbContext db)
        {
            _db = db;
        }

        public IAsyncMutableDataSet<T> Mutate<T>(IEnumerable<string> includes)
        where T : class
        {
            return new EfMutableDataSet<T>(_db, includes);
        }

        public IAsyncMutableDataSet<T> Mutate<T>(params string[] includes)
        where T : class
        {
            return new EfMutableDataSet<T>(_db, includes);
        }
    }
}
