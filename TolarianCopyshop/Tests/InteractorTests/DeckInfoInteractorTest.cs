using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tolarian.Copyshop.Business.Models.DeckInfo;
using Tolarian.Copyshop.Business.UseCaseInteractors;

namespace Tests.InteractorTests
{
    [TestClass]
    public class DeckInfoInteractorTest
    {
        [TestMethod]
        public void GetTotalCardCountOfDeck_Test()
        {
            DeckInfoInteractor unitUnderTest = new DeckInfoInteractor();
            var dummyDeck = GetDummyDeck();
            int expectedCount = 6;

            int actualCount = unitUnderTest.GetTotalCardCountOfDeck(dummyDeck);

            Assert.AreEqual(expectedCount, actualCount);
        }

        private List<DeckInfoCard> GetDummyDeck()
        {
            return new List<DeckInfoCard>
            {
                new DeckInfoCard { Copies = 3, Id = new Guid("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa") },
                new DeckInfoCard { Copies = 1, Id = new Guid("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb") },
                new DeckInfoCard { Copies = 2, Id = new Guid("cccccccccccccccccccccccccccccccc") }, 
            };
        }
    }
}
