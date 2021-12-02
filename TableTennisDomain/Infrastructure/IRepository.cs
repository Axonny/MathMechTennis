namespace TableTennisDomain.Infrastructure
{
    public interface IRepository<TKey, TItem>
        where TItem : IIdentifiable<TKey>
    {
        TItem GetById(TKey id);
        void Update(TItem obj);
        void Save(TItem obj);
    }
}