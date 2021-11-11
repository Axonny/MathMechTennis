using System.Linq;
using NUnit.Framework;
using TableTennisDomain.Repositories;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void SaveGetAll()
        {
            var repo = new EloRatingRepository();

            var records = CreateRecords();

            foreach (var record in records) 
                repo.SaveOrUpdate(record);

            Assert.That(repo.GetAll(), Is.EquivalentTo(records));
        }
        
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
                new EloRecord(10000, 10),
                new EloRecord(2100, 20),
                new EloRecord(11, 40),
                new EloRecord(5000, 35),
                new EloRecord(1200, 0)
            };
        }
    }
}