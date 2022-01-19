using MongoDB.Bson;
using TableTennisDomain.Infrastructure;

namespace App.Rating
{
    public interface IRatingRecord : IIdentifiable<ObjectId>
    {
        int Rating { get; set; }
    }
}