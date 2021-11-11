using System.Collections.Generic;

namespace TableTennisDomain.Infrastructure
{
    public interface IRepository<TKey, TItem>
        where TItem : IIdentifiable<TKey>
    {
        TKey GetUniqueId();
        List<TItem> GetAll();
        TItem GetById(TKey id);
        void SaveOrUpdate(TItem obj);
    }
}