using TableTennisDomain.Infrastructure;

namespace App.Rating
{
    public class TrueSkillRatingRepository : MongoDbRepository<TrueSkillRecord>
    {
        public TrueSkillRatingRepository() 
            : base("TrueSkillRating")
        { }
        
    }
}