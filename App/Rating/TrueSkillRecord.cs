using System;
using MongoDB.Bson;
using TableTennisDomain.Infrastructure;

namespace App.Rating
{
    public class TrueSkillRecord : IIdentifiable<ObjectId>
    {
        public ObjectId Id { get; set; }
        public double Mu { get; set; }
        public double Sigma { get; set; }
        
        public int Rating => (int) Math.Round(Mu - 3 * Sigma);

        public TrueSkillRecord(ObjectId id, double mu, double sigma)
        {
            Id = id;
            Mu = mu;
            Sigma = sigma;
        }
        
    }
}