using TableTennisDomain.Infrastructure;

namespace TableTennisDomain.DomainRepositories
{
    public class MatchesRepository : MongoDbRepository<Match>
    {
        public MatchesRepository() 
            : base("mongodb://127.0.0.1:27017", "MathMechTennis", "Matches")
        { }
    }
}