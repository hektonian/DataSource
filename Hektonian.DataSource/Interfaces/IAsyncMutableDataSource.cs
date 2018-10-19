using System.Collections.Generic;

namespace Hektonian.DataSource.Interfaces
{
    /// <summary>
    /// Mutate-only data source
    /// </summary>
    public interface IAsyncMutableDataSource
    {
        /// <summary>
        /// Creates a mutable data set
        /// </summary>
        /// <param name="includes">Relation includes</param>
        /// <returns>Mutable data set</returns>
        IAsyncMutableDataSet<TEntity> Mutate<TEntity>(IEnumerable<string> includes) where TEntity : class;

        /// <summary>
        /// Creates a mutable data set
        /// </summary>
        /// <param name="includes">Relation includes</param>
        /// <returns>Mutable data set</returns>
        IAsyncMutableDataSet<TEntity> Mutate<TEntity>(params string[] includes) where TEntity : class;
    }
}