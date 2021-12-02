using System;
using System.Data;
using MongoDB.Bson;
using TableTennisDomain.Infrastructure;

namespace TableTennisDomain
{
    public class Match : IIdentifiable<ObjectId>
    {
        public ObjectId Id { get; }

        public DateTime Date { get; }
        
        public ObjectId FirstPlayerId { get; }
        public ObjectId SecondPlayerId { get; }
        
        public int GamesWonByFirstPlayer { get; }
        public int GamesWonBySecondPlayer { get; }

        public ObjectId Winner
        {
            get
            {
                if (GamesWonByFirstPlayer > GamesWonBySecondPlayer)
                    return FirstPlayerId;
                if (GamesWonByFirstPlayer < GamesWonBySecondPlayer)
                    return SecondPlayerId;
                throw new DataException("Match ended in a draw");
            }
        }

        public Match(ObjectId firstPlayerId, ObjectId secondPlayerId, int gamesWonByFirstPlayer,
            int gamesWonBySecondPlayer, DateTime date = default)
        {
            FirstPlayerId = firstPlayerId;
            SecondPlayerId = secondPlayerId;
            GamesWonByFirstPlayer = gamesWonByFirstPlayer;
            GamesWonBySecondPlayer = gamesWonBySecondPlayer;
            Date = date == default ? DateTime.Now : date;
        }
    }
}