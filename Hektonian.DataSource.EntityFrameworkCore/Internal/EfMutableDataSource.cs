using System;
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

        public IAsyncMutableDataSet<TEntity> Mutate<TEntity>(IEnumerable<string> includes)
        where TEntity : class
        {
            if (includes == null)
            {
                throw new ArgumentNullException(nameof(includes));
            }

            return new EfMutableDataSet<TEntity>(_db, includes);
        }

        public IAsyncMutableDataSet<TEntity> Mutate<TEntity>(params string[] includes)
        where TEntity : class
        {
            return new EfMutableDataSet<TEntity>(_db, includes);
        }
    }
}
