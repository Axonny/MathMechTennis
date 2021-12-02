using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace TableTennisDomain.Infrastructure
{
    public abstract class MongoDbRepository<TItem> : IRepository<ObjectId, TItem> where TItem : IIdentifiable<ObjectId>
    {
        protected readonly IMongoCollection<BsonDocument> Collection;

        protected MongoDbRepository(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            Collection = database.GetCollection<BsonDocument>(collectionName);
        }

        public TItem GetById(ObjectId id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var bson = Collection.Find(filter).First();
            return BsonSerializer.Deserialize<TItem>(bson);
        }
        
        public void SaveOrUpdate(TItem obj)
        {
            var bson = obj.ToBsonDocument();
            var id = bson.GetElement("_id").Value;
            if (id.IsObjectId && id.AsObjectId != ObjectId.Empty)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", id.AsObjectId);
                Collection.ReplaceOne(filter, bson);
            }
            else
            {
                Collection.InsertOne(bson);
            }
        }
    }
}