using TableTennisDomain.Infrastructure;

namespace App.Rating
{
    public class EloRatingRepository : MongoDbRepository<EloRecord>
    {
        public EloRatingRepository() 
            : base("EloRating")
        { }
    }
}