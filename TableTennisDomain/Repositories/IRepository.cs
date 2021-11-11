using System.Collections.Generic;
using TableTennisDomain.Infrastructure;

namespace TableTennisDomain.Repositories
{
    public interface IRepository<TKey, TItem>
        where TItem : IIdentifiable<TKey>
    {
        List<TItem> GetAll();
        TItem GetById(TKey id);
        void SaveOrUpdate(TItem obj);
    }
}