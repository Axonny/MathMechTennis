using System;
using TableTennisDomain;
using TableTennisDomain.Infrastructure;

namespace App.Rating
{
    public abstract class RatingSystem<TRecord> where TRecord : IIdentifiable<long>
    {
        public LongKeyRepository<TRecord> RatingByPlayerId { get; }
        
        public void UpdateRating(Match match)
        {
            throw new NotImplementedException();
        }

        protected abstract void Calculate(TRecord player1Record, TRecord player2Record, bool isFirstWinner);
    }
}