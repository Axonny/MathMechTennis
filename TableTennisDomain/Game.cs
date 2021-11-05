using System.Collections.Generic;
using System;

namespace TableTennisDomain
{
    public enum GameStatus
    {
        Draw,
        FirstTeamWon,
        SecondTeamWon
        
    }
    public class Game
    { 
        public int PointsFirstTeam { get; private set; }
        public int PointsSecondTeam { get; private set; }

        public Game(int pointsFirstTeam, int pointsSecondTeam)
        {
            PointsFirstTeam = pointsFirstTeam;
            PointsSecondTeam = pointsSecondTeam;
        }

        public GameStatus Status
        {
            get
            {
                var difference = PointsFirstTeam - PointsSecondTeam;
                if (Math.Abs(difference) < 2)
                    return GameStatus.Draw;
                if (difference > 0)
                    return GameStatus.FirstTeamWon;
                return GameStatus.SecondTeamWon;
            }
        }
    }
}