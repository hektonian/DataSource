﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hektonian.DataSource.Interfaces
{
    /// <summary>
    /// Read-only asynchronous data set
    /// </summary>
    /// <typeparam name="TEntity">Data set entity type</typeparam>
    public interface IAsyncReadOnlyDataSet<TEntity> where TEntity : class
    {
        /// <summary>
        /// Selects a list of entities from the data source with a query built by queryBuilder
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>A collection of queried entities</returns>
        Task<IEnumerable<TOutput>> GetAllAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> queryBuilder);

        /// <summary>
        /// Selects the first element from the queried data
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>The first element in a query</returns>
        Task<TOutput> FirstAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> queryBuilder);

        /// <summary>
        /// Selects the first element from the queried data or default if query result is empty
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>The first element in a query or default</returns>
        Task<TOutput> FirstOrDefaultAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> queryBuilder);

        /// <summary>
        /// Selects a single element from the queried data
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>A single element in a query</returns>
        Task<TOutput> SingleAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> queryBuilder);

        /// <summary>
        /// Selects a single element from the queried data or default if query result is empty
        /// </summary>
        /// <typeparam name="TOutput">Output type of the query</typeparam>
        /// <param name="queryBuilder">Query builder function</param>
        /// <returns>A single element in a query or default</returns>
        Task<TOutput> SingleOrDefaultAsync<TOutput>(Func<IQueryable<TEntity>, IQueryable<TOutput>> queryBuilder);

        /// <summary>
        /// Selects a list of entities that fulfill the given condition from the data source
        /// </summary>
        /// <param name="condition">Condition the entities must fulfill</param>
        /// <returns>A collection of matching entities</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> condition);

        /// <summary>
        /// Selects a single element in data set that matches the given condition
        /// </summary>
        /// <param name="condition">Condition to fulfill</param>
        /// <returns>A single element in data set that matches the condition</returns>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> condition);

        /// <summary>
        /// Selects a single element in data set that matches the given condition or default value if none found
        /// </summary>
        /// <param name="condition">Condition to fulfill</param>
        /// <returns>A single element in data set that matches the condition or default</returns>
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> condition);
    }
}