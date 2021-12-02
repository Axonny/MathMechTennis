using MongoDB.Bson;
using TableTennisDomain.Infrastructure;

namespace App
{
    public record Player(ObjectId Id, long ChatId, string Username) : IIdentifiable<ObjectId>
    {
        public Player(long chatId, string username) : this(ObjectId.Empty, chatId, username) { }
    }
}