using System.Collections.Generic;
using MongoDB.Driver;
using TableTennisDomain.Infrastructure;

namespace App
{
    public class MatchStatusRepository : MongoDbRepository<MatchStatusRecord>
    {
        public MatchStatusRepository() : base("MatchStatuses")
        {
        }

        public IEnumerable<MatchStatusRecord> GetAllUnconfirmed()
        {
            return Collection.Find(record => !record.IsConfirmedByFirst || !record.IsConfirmedBySecond).ToEnumerable();
        }
    }
}