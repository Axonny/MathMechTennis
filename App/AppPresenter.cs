using App.Rating;
using TableTennisDomain;
using TableTennisDomain.DomainRepositories;
using TableTennisDomain.Infrastructure;

namespace App
{
    public class AppPresenter<TRatingRecord> 
        where TRatingRecord : IIdentifiable<long>
    {
        // ReSharper disable MemberCanBePrivate.Global
        public RatingSystem<TRatingRecord> RatingSystem { get; }
        // ReSharper restore MemberCanBePrivate.Global
        
        private readonly MatchesRepository matchesRepository;
        
        public AppPresenter(
            MatchesRepository matchesRepository, 
            RatingSystem<TRatingRecord> ratingSystem)
        {
            this.matchesRepository = matchesRepository;
            RatingSystem = ratingSystem;
        }

        public void RegisterMatch(long player1Id, long player2Id, int gamesWon1, int gamesWon2)
        {
            var match = new Match(matchesRepository.GetUniqueId(), player1Id, player2Id, gamesWon1, gamesWon2);
            
            matchesRepository.SaveOrUpdate(match);
            RatingSystem.UpdateRating(match);
        }
    }
}