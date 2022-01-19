using MongoDB.Bson;
using TableTennisDomain;
using TableTennisDomain.Infrastructure;

namespace App.Rating
{
    public abstract class RatingSystem<TRecord> where TRecord : IRatingRecord
    {
        public IRepository<ObjectId, TRecord> RatingByPlayerId { get; }
        
        protected RatingSystem(IRepository<ObjectId, TRecord> ratingByPlayerId)
        {
            RatingByPlayerId = ratingByPlayerId;
        }
        
        public void UpdateRating(Match match)
        {
            var player1Record = RatingByPlayerId.GetById(match.FirstPlayerId);
            var player2Record = RatingByPlayerId.GetById(match.SecondPlayerId);
            Calculate(player1Record, player2Record, match.Winner == match.FirstPlayerId);
            RatingByPlayerId.Update(player1Record);
            RatingByPlayerId.Update(player2Record);
        }

        public abstract void RegisterNewPlayer(ObjectId id);
        public abstract void Calculate(TRecord player1Record, TRecord player2Record, bool isFirstWinner);
    }
}