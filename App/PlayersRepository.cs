using System;
using System.Net;
using MongoDB.Bson;
using MongoDB.Driver;
using TableTennisDomain.Infrastructure;

namespace App
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PlayersRepository : MongoDbRepository<Player>
    {
        public PlayersRepository() 
            : base("Players")
        { }

        public string GetNicknameByPlayerId(ObjectId id)
        {
            try
            {
                return Collection.Find(p => p.Id == id).First().Nickname;
            }
            catch (Exception exception)
            {
                throw new RepositoryException(exception);
            }
        }

        public ObjectId GetPlayerIdByNickname(string username)
        {
            try
            {
                return Collection.Find(p => p.Nickname == username).First().Id;
            }
            catch (Exception exception)
            {
                throw new RepositoryException(exception);
            }
        }

        public bool TryGetPlayerIdByChatId(long chatId, out Player player)
        {
            player = Collection.Find(x => x.ChatId == chatId).FirstOrDefault();
            return player != null;
        }
    }
}