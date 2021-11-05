using System.Collections.Generic;

namespace TableTennisDomain.Repositories
{
    public class RuntimeRepository<TItem> : IRepository<TItem>
    {
        private readonly Dictionary<string, TItem> storage = new();
        
        public TItem GetById(string id) => storage[id];

        public void SaveOrUpdate(string id, TItem obj) => storage[id] = obj;
    }
}