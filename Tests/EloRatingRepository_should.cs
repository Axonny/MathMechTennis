using System.Linq;
using App.Rating;
using MongoDB.Bson;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        // [Test]
        // public void SaveGetAll()
        // {
        //     var repo = new EloRatingRepository();
        //
        //     var records = CreateRecords();
        //
        //     foreach (var record in records) 
        //         repo.SaveOrUpdate(record);
        //
        //     Assert.That(repo.GetAll(), Is.EquivalentTo(records));
        // }
        
        [Test]
        public void SaveGetById()
        {
            var repo = new EloRatingRepository();

            var records = CreateRecords();
            var ids = records.Select(record => record.Id);

            foreach (var record in records) 
                repo.SaveOrUpdate(record);

            Assert.That(ids.Select(id => repo.GetById(id)).ToList(), Is.EquivalentTo(records));
        }

        private static EloRecord[] CreateRecords()
        {
            return new[]
            {
                new EloRecord(ObjectId.GenerateNewId(10000), 10, 0),
                new EloRecord(ObjectId.GenerateNewId(2100), 20, 0),
                new EloRecord(ObjectId.GenerateNewId(11), 40, 0 ),
                new EloRecord(ObjectId.GenerateNewId(5000), 35, 0),
                new EloRecord(ObjectId.GenerateNewId(1200), 0, 0)
            };
        }
    }
}