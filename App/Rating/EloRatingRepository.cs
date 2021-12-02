using TableTennisDomain.Infrastructure;

namespace App.Rating
{
    public class EloRatingRepository : MongoDbRepository<EloRecord>
    {
        public EloRatingRepository() 
            : base("mongodb://127.0.0.1:27017", "MathMechTennis", "EloRating")
        { }
    }
}