namespace TableTennisDomain
{
    public interface IRepository<TItem>
    {
        TItem GetById(string id);
        void Save(TItem obj);
    }
}