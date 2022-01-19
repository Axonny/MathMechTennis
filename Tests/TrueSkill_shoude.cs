using App.Rating;
using NUnit.Framework;
using MongoDB.Bson;

namespace Tests
{
    public class TrueSkill_shoude
    {
        [Test]
        public void CorrectCalculation()
        {
            var rating = new TrueSkillRating(null);
            var recordPlayer1 = new TrueSkillRecord(ObjectId.GenerateNewId(), 25, 25 / 3);
            var recordPlayer2 = new TrueSkillRecord(ObjectId.GenerateNewId(), 25, 25 / 3); 
            rating.Calculate(recordPlayer1, recordPlayer2, true);
            Assert.AreEqual(11,recordPlayer1.Rating);
            Assert.AreEqual(3,recordPlayer2.Rating);
            
        }
        
    }
}