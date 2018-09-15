using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hektonian.DataSource
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