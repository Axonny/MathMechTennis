using System;
using MongoDB.Bson;
using TableTennisDomain.Infrastructure;
using System.Web.UI.DataVisualization.Charting;

namespace App.Rating
{
    public class TrueSkillRating : RatingSystem<TrueSkillRecord>
    {
        private const double Beta = 25 / 6;
        public TrueSkillRating(IRepository<ObjectId, TrueSkillRecord> ratingRepository) : base(ratingRepository)
        {
        }

        public override void RegisterNewPlayer(ObjectId id)
        {
            RatingByPlayerId.Save(new TrueSkillRecord(id, 25, 25 / 3));
        }

        public override void Calculate(TrueSkillRecord player1Record, TrueSkillRecord player2Record, bool isFirstWinner)
        {
            var t = Math.Abs(player1Record.Mu - player2Record.Mu);
            var c = Math.Sqrt(2 * Beta * Beta 
                              + player1Record.Sigma * player1Record.Sigma 
                              + player2Record.Sigma * player2Record.Sigma);
            var v = GetV(t / c);
            var w = v * (v + t / c);
            player1Record.Mu = GetNewMu(player1Record, c, v, isFirstWinner);
            player2Record.Mu = GetNewMu(player2Record, c, v, !isFirstWinner);
            player1Record.Sigma = GetNewSigma(player1Record, c, w);
            player2Record.Sigma = GetNewSigma(player2Record, c, w);
        }
        
        private double GetNewSigma(TrueSkillRecord playerRecord, double c, double w)
        {
            return playerRecord.Sigma * (1 - w * playerRecord.Sigma * playerRecord.Sigma / (c * c));
        }
        
        private double GetNewMu(TrueSkillRecord playerRecord, double c, double v, bool isWinner)
        {
            if (isWinner)
                return playerRecord.Mu + v * playerRecord.Sigma * playerRecord.Sigma / c;
            return playerRecord.Mu - v * playerRecord.Sigma * playerRecord.Sigma / c;
        }
        
        private double GetV(double t)
        {
            var chart1 = new Chart();
            var cdf = chart1.DataManipulator.Statistics.NormalDistribution(t);
            var degree = -(t * t / 2);
            var pdf = 1 / Math.Sqrt(2 * Math.PI) * Math.Exp(degree);
            return pdf / cdf;
        }
    }
}