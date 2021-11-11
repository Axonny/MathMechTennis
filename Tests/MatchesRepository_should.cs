using System;
using System.Linq;
using NUnit.Framework;
using TableTennisDomain.DomainRepositories;

namespace Tests
{
    public class MatchesRepository_should
    {
        [Test]
        public void CreatingStringId()
        {
            var repo = new MatchesRepository();

            Assert.That(repo.GetAll().Select(item => item.Id).Append(repo.GetUniqueId()), Is.Unique);
        }
    }
}