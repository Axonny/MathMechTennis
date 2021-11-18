using TableTennisDomain.Infrastructure;

namespace App
{
    public record Player(long Id, long ChatId, string Username) : IIdentifiable<long> {}
}