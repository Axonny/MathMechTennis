using TableTennisDomain.Infrastructure;

namespace TableTennisDomain
{
    public record Player(long Id, int Rating) : IIdentifiable<long>
    {
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}; {nameof(Rating)}: {Rating}";
        }
    }
}