using System;
using MongoDB.Bson;

namespace App.Rating
{
    public class EloRating : RatingSystem<EloRecord>
    {
        private const int StartElo = 1000;
        
        public EloRating() : base(new EloRatingRepository()) { }

        public override void RegisterNewPlayer(ObjectId id)
        {
            RatingByPlayerId.Save(new EloRecord(id, StartElo, 0));
        }

        public override void Calculate(EloRecord player1Record, EloRecord player2Record, bool isFirstWinner)
        {
            var player1Score = 0;
            var player2Score = 0;
            if (isFirstWinner)
                player1Score = 1;
            else
                player2Score = 1;
            
            player1Record.Rating = CalculatePlayerRating(player1Record, player2Record, player1Score);
            player2Record.Rating = CalculatePlayerRating(player2Record, player1Record, player2Score);
            player1Record.MatchCount++;
            player2Record.MatchCount++;
        }

        private static double GetExpectedScore(long player1Rating, long player2Rating)
        {
            return 1 / (1 + Math.Pow(10, (player2Rating - player1Rating) / 400.0));
        }

        private static int CalculatePlayerRating(EloRecord player1Record, EloRecord player2Record, int player1Score)
        {
            return player1Record.Rating
                   + (int) Math.Round(GetFactor(player1Record)
                                       * (player1Score - GetExpectedScore(player1Record.Rating, player2Record.Rating)));
        }

        private static int GetFactor(EloRecord record)
        {
            if (record.MatchCount <= 30)
                return 40;
            if (record.Rating >= 2400)
                return 10;
            return 20;
        }
    }
}