using TableTennisDomain.Infrastructure;

namespace TableTennisDomain.DomainRepositories
{
    public class MatchesRepository : MongoDbRepository<Match>
    {
        public MatchesRepository() 
            : base("Matches")
        { }
    }
}