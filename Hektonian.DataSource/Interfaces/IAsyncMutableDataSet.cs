using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hektonian.DataSource.Interfaces
{
    /// <summary>
    /// Mutation-only asynchronous data set
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IAsyncMutableDataSet<TEntity> where TEntity : class
    {
        /// <summary>
        /// Adds an entity to the data source
        /// </summary>
        /// <param name="entity">Entity to add</param>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Adds a collection of entities to the data source
        /// </summary>
        /// <param name="entities"></param>
        Task AddRangeAsync(params TEntity[] entities);

        /// <summary>
        /// Adds a collection of entities to the data source
        /// </summary>
        /// <param name="entities">Entities to add</param>
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Removes an entity from the data source
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        Task RemoveAsync(TEntity entity);

        /// <summary>
        /// Removes a collection of entities from the data source
        /// </summary>
        /// <param name="entities">Entities to remove</param>
        Task RemoveRangeAsync(params TEntity[] entities);

        /// <summary>
        /// Removes a collection of entities from the data source
        /// </summary>
        /// <param name="entities">Entities to remove</param>
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Removes the elements selected by the query builder
        /// </summary>
        /// <param name="queryBuilder">Query builder function</param>
        Task RemoveAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryBuilder);

        /// <summary>
        /// Removes the elements that match the condition from the data source
        /// </summary>
        /// <param name="condition">Condition to fulfill</param>
        Task RemoveAsync(Expression<Func<TEntity, bool>> condition);

        /// <summary>
        /// Updates an entity in the data source
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Updates a collection of entities to the data source
        /// </summary>
        /// <param name="entities">Entities to update</param>
        Task UpdateRangeAsync(params TEntity[] entities);

        /// <summary>
        /// Updates a collection of entities to the data source
        /// </summary>
        /// <param name="entities">Entities to update</param>
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Saves changes to store
        /// </summary>
        Task SaveAsync();
    }
}