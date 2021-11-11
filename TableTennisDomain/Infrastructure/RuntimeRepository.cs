using System.Collections.Generic;
using System.Linq;

namespace TableTennisDomain.Infrastructure
{
    public abstract class RuntimeRepository<TKey, TItem> : IRepository<TKey, TItem> 
        where TItem : IIdentifiable<TKey>
    {
        private readonly Dictionary<TKey, TItem> storage = new();

        public List<TItem> GetAll() => storage.Values.ToList();

        public TItem GetById(TKey id) => storage[id];

        public void SaveOrUpdate(TItem obj) => storage[obj.Id] = obj;
        
        public abstract TKey GetUniqueId();
    }
}