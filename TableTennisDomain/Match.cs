using System;
using System.Collections.Generic;
using System.Linq;
using TableTennisDomain.Infrastructure;

namespace TableTennisDomain
{
    public class Match : IIdentifiable<string>
    {
        public string Id { get; private set; }

        public DateTime Date;
        public IReadOnlyList<Game> Games { get; private set; }
        
        public IReadOnlyList<Player> FirstTeam { get; private set; }
        public IReadOnlyList<Player> SecondTeam { get; private set; }

        public IReadOnlyList<Player> Winners
        {
            get
            {
                return Status switch
                {
                    GameStatus.Draw => new List<Player>(),
                    GameStatus.FirstTeamWon => FirstTeam,
                    _ => SecondTeam
                };
            }
        }

        public GameStatus Status
        {
            get
            {
                return Games.Select(game => game.Status)
                    .GroupBy(status => status)
                    .OrderByDescending(g => g.Count()).First().Key;
            }
        }

        public Match(string id, List<Player> firstTeam, List<Player> secondTeam, List<Game> games, DateTime date = default)
        {
            Id = id;
            FirstTeam = firstTeam;
            SecondTeam = secondTeam;
            Games = games;
            Date = date == default ? DateTime.Now : date;
        }
    }
}