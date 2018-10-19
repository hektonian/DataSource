using System;
using System.Collections.Generic;
using System.Linq;
using Hektonian.DataSource.InMemory.Interfaces;

namespace Hektonian.DataSource.InMemory.Internal
{
    internal class InMemoryDataStore : IInMemoryDataStore
    {
        private Dictionary<Type, List<object>> _data;
        private Dictionary<Type, List<object>> _buffer;

        public InMemoryDataStore(IEnumerable<Type> storeTypes)
        {
            _data = storeTypes?.ToDictionary(type => type, _ => new List<object>()) ?? new Dictionary<Type, List<object>>();
        }

        public InMemoryDataStore(Dictionary<Type, List<object>> data = null)
        {
            _data = data ?? new Dictionary<Type, List<object>>();
            _buffer = null;
        }

        public ICollection<TEntity> Set<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);

            return (_buffer ?? _data).ContainsKey(type)
                       ? _data[type].Cast<TEntity>().ToList()
                       : null;
        }

        public void BeginTransaction()
        {
            _buffer = _data;
        }

        public void Commit()
        {
            _data = _buffer;
            _buffer = null;
        }

        public void Rollback()
        {
            _buffer = null;
        }
    }
}
