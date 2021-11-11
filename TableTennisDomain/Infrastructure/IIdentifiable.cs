namespace TableTennisDomain.Infrastructure
{
    public interface IIdentifiable<TId>
    {
        public TId Id { get; }
    }
}