using TableTennisDomain.Infrastructure;

namespace App
{
    //                 ChatId
    public record User(long Id, long playerId) : IIdentifiable<long> {}
}