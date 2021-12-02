using MongoDB.Bson;
using TableTennisDomain;
using TableTennisDomain.Infrastructure;

namespace App.Rating
{
    public abstract class RatingSystem<TRecord> where TRecord : IIdentifiable<ObjectId>
    {
        public IRepository<ObjectId, TRecord> RatingByPlayerId { get; }
        
        public void UpdateRating(Match match)
        {
            var player1Record = RatingByPlayerId.GetById(match.FirstPlayerId);
            var player2Record = RatingByPlayerId.GetById(match.SecondPlayerId);
            Calculate(player1Record, player2Record, match.Winner == match.FirstPlayerId);
            RatingByPlayerId.SaveOrUpdate(player1Record);
            RatingByPlayerId.SaveOrUpdate(player2Record);
        }

        protected abstract void Calculate(TRecord player1Record, TRecord player2Record, bool isFirstWinner);
    }
}