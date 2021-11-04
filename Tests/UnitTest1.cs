using System;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(
                new TableTennisDomain.Player("P1", 100), 
                new TableTennisDomain.Player("P1", 100));
        }
    }
}