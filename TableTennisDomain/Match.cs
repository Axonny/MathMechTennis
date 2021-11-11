using System;
using System.Data;
using TableTennisDomain.Infrastructure;

namespace TableTennisDomain
{
    public class Match : IIdentifiable<string>
    {
        public string Id { get; }

        public DateTime Date { get; }
        
        public long FirstPlayerId { get; }
        public long SecondPlayerId { get; }
        
        public int GamesWonByFirstPlayer { get; }
        public int GamesWonBySecondPlayer { get; }

        public long Winner
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

        public Match(string id, long firstPlayerId, long secondPlayerId, int gamesWonByFirstPlayer,
            int gamesWonBySecondPlayer, DateTime date = default)
        {
            Id = id;
            FirstPlayerId = firstPlayerId;
            SecondPlayerId = secondPlayerId;
            GamesWonByFirstPlayer = gamesWonByFirstPlayer;
            GamesWonBySecondPlayer = gamesWonBySecondPlayer;
            Date = date == default ? DateTime.Now : date;
        }
    }
}