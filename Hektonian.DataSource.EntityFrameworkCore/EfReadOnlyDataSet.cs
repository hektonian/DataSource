using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Hektonian.DataSource.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hektonian.DataSource.EntityFrameworkCore
{
    public class EfReadOnlyDataSet<T> : IAsyncReadOnlyDataSet<T>
    where T : class
    {
        private readonly IQueryable<T> _querySet;

        internal EfReadOnlyDataSet(DbContext db, IEnumerable<string> navigationPropertyPaths = null)
        {
            navigationPropertyPaths = navigationPropertyPaths ?? Array.Empty<string>();
            _querySet = navigationPropertyPaths
               .Aggregate(
                    db.Set<T>(),
                    (IQueryable<T> set, string navigationPropertyPath) => set.Include(navigationPropertyPath)
                );
        }

        /// <inheritdoc />
        /// <summary>
        /// Selects a list of entities from the data source with a query built by queryBuilder
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>A collection of queried entitites</returns>
        public async Task<IEnumerable<TOutput>> GetAllAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return await queryBuilder(_querySet)
                        .ToListAsync()
                        .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <summary>
        /// Selects the first element from the queried data
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>The first element in a query</returns>
        public Task<TOutput> FirstAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(_querySet)
               .FirstAsync();
        }

        /// <inheritdoc />
        /// <summary>
        /// Selects the first element from the queried data or default if query result is empty
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>The first element in a query or default</returns>
        public Task<TOutput> FirstOrDefaultAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(_querySet)
               .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Selects a single element from the queried data
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>A single element in a query</returns>
        public Task<TOutput> SingleAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(_querySet)
               .SingleAsync();
        }

        /// <inheritdoc />
        /// <summary>
        /// Selects a single element from the queried data or default if query result is empty
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>A single element in a query or default</returns>
        public Task<TOutput> SingleOrDefaultAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(_querySet)
               .SingleOrDefaultAsync();
        }

        /// <inheritdoc />
        /// <summary>
        /// Selects the last element from the queried data
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>The last element in a query</returns>
        public Task<TOutput> LastAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(_querySet)
               .LastAsync();
        }

        /// <inheritdoc />
        /// <summary>
        /// Selects the last element from the queried data or default if query result is empty
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>The last element in a query or default</returns>
        public Task<TOutput> LastOrDefaultAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(_querySet)
               .LastOrDefaultAsync();
        }

        /// <inheritdoc />
        /// <summary>
        /// Selects a list of entities that fulfill the given condition from the data source
        /// </summary>
        /// <param name="condition">Condition the entities must fulfill</param>
        /// <returns>A collection of matching entities</returns>
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> condition)
        {
            return await _querySet.Where(condition)
                                  .ToListAsync()
                                  .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <summary>
        /// Selects the first element in data set that matches the given condition
        /// </summary>
        /// <param name="condition">Condition to fulfill</param>
        /// <returns>The first element in data set that matches the condition</returns>
        public Task<T> FirstAsync(Expression<Func<T, bool>> condition)
        {
            return _querySet.FirstAsync(condition);
        }

        /// <inheritdoc />
        /// <summary>
        /// Selects the first element in data set that matches the given condition or default value if none found
        /// </summary>
        /// <param name="condition">Condition to fulfill</param>
        /// <returns>The first element in data set that matches the condition or default</returns>
        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> condition)
        {
            return _querySet.FirstOrDefaultAsync(condition);
        }

        /// <inheritdoc />
        /// <summary>
        /// Selects a single element in data set that matches the given condition
        /// </summary>
        /// <param name="condition">Condition to fulfill</param>
        /// <returns>A single element in data set that matches the condition</returns>
        public Task<T> SingleAsync(Expression<Func<T, bool>> condition)
        {
            return _querySet.SingleAsync(condition);
        }

        /// <inheritdoc />
        /// <summary>
        /// Selects a single element in data set that matches the given condition or default value if none found
        /// </summary>
        /// <param name="condition">Condition to fulfill</param>
        /// <returns>A single element in data set that matches the condition or default</returns>
        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> condition)
        {
            return _querySet.SingleOrDefaultAsync(condition);
        }

        /// <inheritdoc />
        /// <summary>
        /// Selects the last element in data set that matches the given condition
        /// </summary>
        /// <param name="condition">Condition to fulfill</param>
        /// <returns>The last element in data set that matches the condition</returns>
        public Task<T> LastAsync(Expression<Func<T, bool>> condition)
        {
            return _querySet.LastAsync(condition);
        }

        /// <inheritdoc />
        /// <summary>
        /// Selects the last element in data set that matches the given condition or default value if none found
        /// </summary>
        /// <param name="condition">Condition to fulfill</param>
        /// <returns>The last element in data set that matches the condition or default</returns>
        public Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> condition)
        {
            return _querySet.LastOrDefaultAsync(condition);
        }
    }
}
