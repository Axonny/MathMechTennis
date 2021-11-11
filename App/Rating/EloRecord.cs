using TableTennisDomain.Infrastructure;

namespace App.Rating
{
    public record EloRecord(long Id, long Rating) : IIdentifiable<long>;
}