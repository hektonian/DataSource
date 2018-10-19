using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hektonian.DataSource.Interfaces
{
    /// <summary>
    /// Read-only asynchronous data source
    /// </summary>
    public interface IAsyncDataSource
    {
        /// <summary>
        /// Retrieves a read-only data set
        /// </summary>
        /// <param name="includes">Relation includes</param>
        /// <returns>Read-only data set</returns>
        IAsyncReadOnlyDataSet<TEntity> Set<TEntity>(IEnumerable<string> includes) where TEntity: class;

        /// <summary>
        /// Creates a read-only data set
        /// </summary>
        /// <param name="includes">Relation includes</param>
        /// <returns>Read-only data set</returns>
        IAsyncReadOnlyDataSet<TEntity> Set<TEntity>(params string[] includes) where TEntity : class;

        /// <summary>
        /// Begins the mutation of the data source
        /// </summary>
        /// <typeparam name="TEntity">The result type of the mutator</typeparam>
        /// <param name="mutator">Data source mutator</param>
        /// <returns>The result of the mutation</returns>
        Task<TEntity> MutateAsync<TEntity>(Func<IAsyncMutableDataSource, Task<TEntity>> mutator) where TEntity : class;

        /// <summary>
        /// Begins the mutation of the data source
        /// </summary>
        /// <param name="mutator">Data source mutator</param>
        Task MutateAsync(Func<IAsyncMutableDataSource, Task> mutator);
    }
}