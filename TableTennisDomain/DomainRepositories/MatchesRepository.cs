using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using TableTennisDomain.Infrastructure;

namespace TableTennisDomain.DomainRepositories
{
    public class MatchesRepository : MongoDbRepository<Match>
    {
        public MatchesRepository()
            : base("Matches")
        {
        }
        
        public List<Match> GetByPlayerId(ObjectId id, int count)
        {
            return Collection
                .Find(match => match.FirstPlayerId == id || match.SecondPlayerId == id)
                .ToEnumerable()
                .Take(count)
                .ToList();
        }
    }
}