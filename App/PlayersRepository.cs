using MongoDB.Bson;
using MongoDB.Driver;
using TableTennisDomain.Infrastructure;

namespace App
{
    public class PlayersRepository : MongoDbRepository<Player>
    {
        public PlayersRepository() 
            : base("mongodb://127.0.0.1:27017", "MathMechTennis", "Users")
        { }

        public ObjectId GetPlayerIdByUsername(string username)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var bson = Collection.Find(filter).First();
            return bson.GetElement("_id").Value.AsObjectId;
        }
    }
}