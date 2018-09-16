using System;
using System.Collections.Generic;
using System.Linq;
using Hektonian.DataSource.InMemory.Interfaces;

namespace Hektonian.DataSource.InMemory.Internal
{
    internal class InMemoryDataStore : IInMemoryDataStore
    {
        private readonly Dictionary<Type, List<object>> _data;

        public InMemoryDataStore(IEnumerable<Type> storeTypes)
        {
            _data = storeTypes.ToDictionary(type => type, _ => new List<object>());
        }

        public InMemoryDataStore(Dictionary<Type, List<object>> data)
        {
            _data = data;
        }

        public ICollection<T> Set<T>() where T : class
        {
            var type = typeof(T);

            return _data.ContainsKey(type)
                       ? _data[type].Cast<T>().ToList()
                       : null;
        }
    }
}
