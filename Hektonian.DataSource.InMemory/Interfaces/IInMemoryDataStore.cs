using System.Collections.Generic;

namespace Hektonian.DataSource.InMemory.Interfaces
{
    public interface IInMemoryDataStore
    {
        ICollection<T> Set<T>() where T : class;
    }
}