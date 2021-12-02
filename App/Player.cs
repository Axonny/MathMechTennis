using MongoDB.Bson;
using TableTennisDomain.Infrastructure;

namespace App
{
    public class Player : IIdentifiable<ObjectId>
    {
        public ObjectId Id { get; set; }
        public long ChatId { get; set; }
        public string Nickname { get; set; }

        public Player(ObjectId id, string nickname, long chatId)
        {
            Id = id;
            ChatId = chatId;
            Nickname = nickname;
        }

        public Player(string nickname, long chatId) : this(ObjectId.Empty, nickname, chatId) { }
    }
}