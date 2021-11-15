using System;

namespace App.Rating
{
    public class EloRating : RatingSystem<EloRecord>
    {
        protected override void Calculate(EloRecord player1Record, EloRecord player2Record, bool isFirstWinner)
        {
            var player1Score = 0;
            var player2Score = 0;
            if (isFirstWinner)
                player1Score = 1;
            else
                player2Score = 1;
            
            player1Record.Rating = CalculatePlayerRating(player1Record, player2Record, player1Score);
            player2Record.Rating = CalculatePlayerRating(player2Record, player1Record, player2Score);
        }

        private static double GetExpectedScore(long player1Rating, long player2Rating)
        {
            return 1 / (1 + Math.Pow(10, (player2Rating - player1Rating) / 400));
        }

        private static long CalculatePlayerRating(EloRecord player1Record, EloRecord player2Record, int player1Score)
        {
            return player1Record.Rating
                   + (long) Math.Round(GetFactor(player1Record)
                                       * (player1Score - GetExpectedScore(player1Record.Rating, player2Record.Rating)));
        }

        private static int GetFactor(EloRecord record)
        {
            if (record.Rating >= 2400)
                return 10;
            return 20;
        }
    }
}