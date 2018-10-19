using System.Collections.Generic;

namespace Hektonian.DataSource.InMemory.Interfaces
{
    public interface IInMemoryDataStore
    {
        ICollection<TEntity> Set<TEntity>() where TEntity : class;

        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}