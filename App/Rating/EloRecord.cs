using TableTennisDomain.Infrastructure;

namespace App.Rating
{
    public class EloRecord : IIdentifiable<long>
    {
        public long Id { get; }
        public long Rating { get; set; }

        public EloRecord(long id, long rating)
        {
            Id = id;
            Rating = rating;
        }
    }
}