using System.Collections.Generic;

namespace Hektonian.DataSource.Interfaces
{
    /// <summary>
    /// Mutate-only data source
    /// </summary>
    public interface IAsyncMutableDataSource
    {
        IAsyncMutableDataSet<T> Mutate<T>(IEnumerable<string> includes) where T : class;
        IAsyncMutableDataSet<T> Mutate<T>(params string[] includes) where T : class;
    }
}