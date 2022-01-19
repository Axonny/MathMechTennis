using System;
using MongoDB.Bson;
using TableTennisDomain.Infrastructure;

namespace App.Rating
{
    public class TrueSkillRecord : IRatingRecord
    {
        public ObjectId Id { get; set; }
        public double Mu { get; set; }
        public double Sigma { get; set; }
        
        public int Rating
        {
            get => (int)Math.Round(Mu - 3 * Sigma);
            set => throw new NotImplementedException();
        }

        public TrueSkillRecord(ObjectId id, double mu, double sigma)
        {
            Id = id;
            Mu = mu;
            Sigma = sigma;
        }
        
    }
}