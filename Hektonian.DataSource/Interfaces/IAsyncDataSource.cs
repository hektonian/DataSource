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
        IAsyncReadOnlyDataSet<T> Set<T>(IEnumerable<string> includes) where T: class;
        IAsyncReadOnlyDataSet<T> Set<T>(params string[] includes) where T : class;
        /// <summary>
        /// Retrieves a read-only data set
        /// </summary>
        /// <param name="includes">Relation includes</param>
        /// <returns>Read-only data set</returns>
        /// <summary>
        /// Creates a read-only data set
        /// </summary>
        /// <param name="includes">Relation includes</param>
        /// <returns>Read-only data set</returns>

        /// <summary>
        /// Begins the mutation of the data source
        /// </summary>
        /// <typeparam name="T">The result type of the mutator</typeparam>
        /// <param name="mutator">Data source mutator</param>
        /// <returns>The result of the mutation</returns>
        Task<T> MutateAsync<T>(Func<IAsyncMutableDataSource, Task<T>> mutator) where T : class;

        /// <summary>
        /// Begins the mutation of the data source
        /// </summary>
        /// <param name="mutator">Data source mutator</param>
        Task MutateAsync(Func<IAsyncMutableDataSource, Task> mutator);
    }
}