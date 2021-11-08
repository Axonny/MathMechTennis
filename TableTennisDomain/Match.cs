using System;
using System.Data;

namespace TableTennisDomain
{
    public class Match
    {
        public string Id { get; }

        public DateTime Date { get; }
        
        public Player FirstPlayer { get; }
        public Player SecondPlayer { get; }
        
        public int GamesWonByFirstPlayer { get; }
        public int GamesWonBySecondPlayer { get; }

        public Player Winner
        {
            get
            {
                if (GamesWonByFirstPlayer > GamesWonBySecondPlayer)
                    return FirstPlayer;
                if (GamesWonByFirstPlayer < GamesWonBySecondPlayer)
                    return SecondPlayer;
                throw new DataException("Match ended in a draw");
            }
        }

        public Match(string id, Player firstPlayer, Player secondPlayer, int gamesWonByFirstPlayer,
            int gamesWonBySecondPlayer, DateTime date = default)
        {
            Id = id;
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;
            GamesWonByFirstPlayer = gamesWonByFirstPlayer;
            GamesWonBySecondPlayer = gamesWonBySecondPlayer;
            Date = date == default ? DateTime.Now : date;
        }
    }
}