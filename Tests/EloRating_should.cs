using App.Rating;
using NUnit.Framework;
using MongoDB.Bson;


namespace Tests
{
    public class EloRating_should
    {
        [Test]
        public void SelectCorrectFactor()
        {
            var rating = new EloRating();
            var recordPlayer1 = new EloRecord(ObjectId.GenerateNewId(), 1000, 0);
            var recordPlayer2 = new EloRecord(ObjectId.GenerateNewId(), 1000, 40);
            rating.Calculate(recordPlayer1, recordPlayer2, true);
            Assert.AreEqual(1020,recordPlayer1.Rating);
            Assert.AreEqual(991,recordPlayer2.Rating);
            var recordPlayer3 = new EloRecord(ObjectId.GenerateNewId(), 2300, 40);
            var recordPlayer4 = new EloRecord(ObjectId.GenerateNewId(), 2500, 40);
            rating.Calculate(recordPlayer3, recordPlayer4, false);
            Assert.AreEqual(2295,recordPlayer3.Rating);
            Assert.AreEqual(2502,recordPlayer4.Rating);
        }

        [Test]
        public void MathCountChange()
        {
            var rating = new EloRating();
            var recordPlayer1 = new EloRecord(ObjectId.GenerateNewId(), 1000, 0);
            var recordPlayer2 = new EloRecord(ObjectId.GenerateNewId(), 1700, 40);
            rating.Calculate(recordPlayer1, recordPlayer2, false);
            Assert.AreEqual(1,recordPlayer1.MatchCount);
            Assert.AreEqual(41,recordPlayer2.MatchCount);
        }
        
        [Test]
        public void StrongAndWeakPlayerPlay()
        {
            var rating = new EloRating();
            var recordPlayer1 = new EloRecord(ObjectId.GenerateNewId(), 1500, 30);
            var recordPlayer2 = new EloRecord(ObjectId.GenerateNewId(), 1800, 40);
            var recordPlayer3 = new EloRecord(ObjectId.GenerateNewId(), 1500, 30);
            var recordPlayer4 = new EloRecord(ObjectId.GenerateNewId(), 1800, 40);
            rating.Calculate(recordPlayer1, recordPlayer2, false);
            rating.Calculate(recordPlayer3, recordPlayer4, true);
            Assert.True(1800 - recordPlayer4.Rating > recordPlayer2.Rating - 1800);
            Assert.True(1500 - recordPlayer1.Rating < recordPlayer3.Rating - 1500);
            Assert.True(recordPlayer3.Rating - 1500 > recordPlayer2.Rating - 1800);
            Assert.True(1500 - recordPlayer1.Rating < 1800 - recordPlayer4.Rating);
        }
        
    }
}