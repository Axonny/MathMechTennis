namespace TableTennisDomain.Repositories
{
    public interface IRepository<TItem>
    {
        TItem GetById(string id);
        void SaveOrUpdate(string id, TItem obj);
    }
}