using TableTennisDomain;
using TableTennisDomain.Repositories;

namespace App
{
    public class Service<TRatingRecord>
    {
        private readonly MatchesRepository matchesRepository;
        private readonly RatingSystem<TRatingRecord> ratingSystem;
        
        public Service(MatchesRepository matchesRepository, RatingSystem<TRatingRecord> ratingSystem)
        {
            this.matchesRepository = matchesRepository;
            this.ratingSystem = ratingSystem;
        }

        public void RegisterMatch(long player1Id, long player2Id, int winnerIndex)
        {
            
        }
    }
}