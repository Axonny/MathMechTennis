using TableTennisDomain.Infrastructure;

namespace TableTennisDomain.Repositories
{
    public record EloRecord(long Id, long Rating) : IIdentifiable<long>;
}