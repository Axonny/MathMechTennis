using TableTennisDomain.Infrastructure;

namespace App
{
    //                 ChatId
    public record User(long Id, long PlayerId, string Username) : IIdentifiable<long> {}
}