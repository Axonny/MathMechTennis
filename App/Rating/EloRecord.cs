using MongoDB.Bson;

namespace App.Rating
{
    public class EloRecord : IRatingRecord
    {
        public ObjectId Id { get; set; }
        public int Rating { get; set; }
        
        public int MatchCount { get; set; }

        public EloRecord(ObjectId id, int rating, int matchCount)
        {
            Id = id;
            Rating = rating;
            MatchCount = matchCount;
        }
    }
}