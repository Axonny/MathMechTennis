using System;
using System.Data;
using MongoDB.Bson;
using TableTennisDomain.Infrastructure;

namespace TableTennisDomain
{
    public class Match : IIdentifiable<ObjectId>
    {
        public ObjectId Id { get; set; }

        public DateTime Date { get; set; }
        
        public ObjectId FirstPlayerId { get; set; }
        public ObjectId SecondPlayerId { get; set; }
        
        public int GamesWonByFirstPlayer { get; set; }
        public int GamesWonBySecondPlayer { get; set; }

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

        public Match(ObjectId matchId, ObjectId firstPlayerId, ObjectId secondPlayerId, int gamesWonByFirstPlayer,
            int gamesWonBySecondPlayer, DateTime date = default)
        {
            Id = matchId;
            FirstPlayerId = firstPlayerId;
            SecondPlayerId = secondPlayerId;
            GamesWonByFirstPlayer = gamesWonByFirstPlayer;
            GamesWonBySecondPlayer = gamesWonBySecondPlayer;
            Date = date == default ? DateTime.Now : date;
        }
    }
}