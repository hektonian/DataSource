using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hektonian.DataSource.Interfaces
{
    public interface IAsyncReadOnlyDataSet<TSource> where TSource : class
    {
        /// <summary>
        /// Selects a list of entities from the data source with a query built by queryBuilder
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>A collection of queried entities</returns>
        Task<IEnumerable<TOutput>> GetAllAsync<TOutput>(Func<IQueryable<TSource>, IQueryable<TOutput>> queryBuilder);

        /// <summary>
        /// Selects the first element from the queried data
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>The first element in a query</returns>
        Task<TOutput> FirstAsync<TOutput>(Func<IQueryable<TSource>, IQueryable<TOutput>> queryBuilder);

        /// <summary>
        /// Selects the first element from the queried data or default if query result is empty
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>The first element in a query or default</returns>
        Task<TOutput> FirstOrDefaultAsync<TOutput>(Func<IQueryable<TSource>, IQueryable<TOutput>> queryBuilder);

        /// <summary>
        /// Selects a single element from the queried data
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>A single element in a query</returns>
        Task<TOutput> SingleAsync<TOutput>(Func<IQueryable<TSource>, IQueryable<TOutput>> queryBuilder);

        /// <summary>
        /// Selects a single element from the queried data or default if query result is empty
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>A single element in a query or default</returns>
        Task<TOutput> SingleOrDefaultAsync<TOutput>(Func<IQueryable<TSource>, IQueryable<TOutput>> queryBuilder);

        /// <summary>
        /// Selects the last element from the queried data
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>The last element in a query</returns>
        Task<TOutput> LastAsync<TOutput>(Func<IQueryable<TSource>, IQueryable<TOutput>> queryBuilder);

        /// <summary>
        /// Selects the last element from the queried data or default if query result is empty
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>The last element in a query or default</returns>
        Task<TOutput> LastOrDefaultAsync<TOutput>(Func<IQueryable<TSource>, IQueryable<TOutput>> queryBuilder);

        /// <summary>
        /// Selects a list of entities that fulfill the given condition from the data source
        /// </summary>
        /// <param name="condition">Condition the entities must fulfill</param>
        /// <returns>A collection of matching entities</returns>
        Task<IEnumerable<TSource>> GetAllAsync(Expression<Func<TSource, bool>> condition);

        /// <summary>
        /// Selects the first element in data set that matches the given condition
        /// </summary>
        /// <param name="condition">Condition to fulfill</param>
        /// <returns>The first element in data set that matches the condition</returns>
        Task<TSource> FirstAsync(Expression<Func<TSource, bool>> condition);

        /// <summary>
        /// Selects the first element in data set that matches the given condition or default value if none found
        /// </summary>
        /// <param name="condition">Condition to fulfill</param>
        /// <returns>The first element in data set that matches the condition or default</returns>
        Task<TSource> FirstOrDefaultAsync(Expression<Func<TSource, bool>> condition);

        /// <summary>
        /// Selects a single element in data set that matches the given condition
        /// </summary>
        /// <param name="condition">Condition to fulfill</param>
        /// <returns>A single element in data set that matches the condition</returns>
        Task<TSource> SingleAsync(Expression<Func<TSource, bool>> condition);

        /// <summary>
        /// Selects a single element in data set that matches the given condition or default value if none found
        /// </summary>
        /// <param name="condition">Condition to fulfill</param>
        /// <returns>A single element in data set that matches the condition or default</returns>
        Task<TSource> SingleOrDefaultAsync(Expression<Func<TSource, bool>> condition);

        /// <summary>
        /// Selects the last element in data set that matches the given condition
        /// </summary>
        /// <param name="condition">Condition to fulfill</param>
        /// <returns>The last element in data set that matches the condition</returns>
        Task<TSource> LastAsync(Expression<Func<TSource, bool>> condition);

        /// <summary>
        /// Selects the last element in data set that matches the given condition or default value if none found
        /// </summary>
        /// <param name="condition">Condition to fulfill</param>
        /// <returns>The last element in data set that matches the condition or default</returns>
        Task<TSource> LastOrDefaultAsync(Expression<Func<TSource, bool>> condition);
    }
}