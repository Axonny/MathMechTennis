using System.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TableTennisDomain.Infrastructure
{
    public abstract class MongoDbRepository<TItem> : IRepository<ObjectId, TItem> where TItem : IIdentifiable<ObjectId>
    {
        protected readonly IMongoCollection<TItem> Collection;

        protected MongoDbRepository(string collectionName)
        {
            //var connectionString = ConfigurationManager.ConnectionStrings["MongoDbServer"].ConnectionString;
            //var databaseName = ConfigurationManager.ConnectionStrings["MongoDbDatabase"].ConnectionString;
            //var client = new MongoClient(connectionString);
            //var database = client.GetDatabase(databaseName);
            //Collection = database.GetCollection<TItem>(collectionName);
        }

        public TItem GetById(ObjectId id)
        {
            return Collection.Find(x => x.Id == id).First();
        }
        
        public void Update(TItem obj)
        {
            if (obj.Id != ObjectId.Empty)
                Collection.ReplaceOne(x => x.Id == obj.Id, obj);
            else
                Collection.InsertOne(obj);
        }

        public void Save(TItem obj)
        {
            Collection.InsertOne(obj);
        }
    }
}