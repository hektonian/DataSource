using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hektonian.DataSource.Interfaces
{
    public interface IAsyncMutableDataSet<T> where T : class
    {
        /// <summary>
        /// Adds an entity to the data source and returns an entity with database values
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <returns>The entity as it is in the database</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Adds a collection of entities to the data source
        /// </summary>
        /// <param name="entities">Entities to add</param>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Removes an entity from the data source
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        Task RemoveAsync(T entity);

        /// <summary>
        /// Removes a collection of entities from the data source
        /// </summary>
        /// <param name="entities">Entities to remove</param>
        Task RemoveRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Removes the elements selected by the query builder
        /// </summary>
        /// <param name="queryBuilder">Query builder function</param>
        Task RemoveAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder);

        /// <summary>
        /// Updates an entity in the data source
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns></returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Updates a collection of entities to the data source
        /// </summary>
        /// <param name="entities">Entities to update</param>
        Task UpdateRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Saves changes to store
        /// </summary>
        Task SaveAsync();
    }
}