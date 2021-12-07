using System;
using System.Net;
using MongoDB.Bson;
using MongoDB.Driver;
using TableTennisDomain.Infrastructure;

namespace App
{
    public class PlayersRepository : MongoDbRepository<Player>
    {
        public PlayersRepository() 
            : base("Players")
        { }

        public string GetUsernameByPlayerId(ObjectId id)
        {
            return Collection.Find(p => p.Id == id).First().Nickname;
        }

        public ObjectId GetPlayerIdByUsername(string nickname)
        {
            var player = Collection.Find(p => p.Nickname == nickname).First();
            return player.Id;
        }

        public bool TryGetPlayerIdByChatId(long chatId, out Player player)
        {
            player = Collection.Find(x => x.ChatId == chatId).FirstOrDefault();
            return player != null;
        }
    }
}