using System.Collections.Generic;
using System.Linq;
using TableTennisDomain.Infrastructure;

namespace TableTennisDomain.Repositories
{
    public class RuntimeRepository<TKey, TItem> : IRepository<TKey, TItem> 
        where TItem : IIdentifiable<TKey>
    {
        private readonly Dictionary<TKey, TItem> storage = new();

        public List<TItem> GetAll() => storage.Values.ToList();

        public TItem GetById(TKey id) => storage[id];

        public void SaveOrUpdate(TItem obj) => storage[obj.Id] = obj;
    }
}