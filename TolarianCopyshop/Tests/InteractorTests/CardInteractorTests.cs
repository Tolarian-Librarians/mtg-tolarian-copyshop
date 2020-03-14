using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.UseCaseInteractors;
using Tolarian.Copyshop.Business.Models.SfCardInfo;
using System;

namespace Tests.InteractorTests
{
    [TestClass]
    public class CardInteractorTests
    {
        MockRepository _rep;
        Mock<ICardDataGateway> _gatewayMock;

        [TestInitialize]
        public void Initialize()
        {
            _rep = new MockRepository(MockBehavior.Strict);
            _gatewayMock = _rep.Create<ICardDataGateway>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _rep.VerifyAll();
        }

        [TestMethod]
        public void GetCardByPrintId_Test()
        {
            SfCard dummy = TestUtils.GetDummyCard();
            _gatewayMock.Setup(m => m.GetCardByPrintId(It.Is<Guid>(g => g == dummy.PrintId))).Returns(dummy);
            CardInteractor unitUnderTest = GetInteractor();

            SfCard result = unitUnderTest.GetCardByPrintId(dummy.PrintId);

            Assert.IsNotNull(result);
            Assert.AreEqual(dummy.PrintId, result.PrintId);
            Assert.AreEqual(dummy.CardId, result.CardId);
            Assert.AreEqual(dummy.Name, result.Name);
        }

        [TestMethod]
        public void GetCardsByNameList_Test()
        {
            //Arrange
            List<string> deckImport = new List<string> { "3 Aether Spellbomb (MRD) 196", "1 Ancient Tomb (TMP)",  "0 Ashnod's Altar (5ED) 218", ""};

            List<string> expectedResolvedCardNames = new List<string> { "Aether Spellbomb", "Aether Spellbomb", "Aether Spellbomb", "Ancient Tomb" };
            _gatewayMock.Setup(m => m.GetCardsByNameList(It.Is<List<string>>(l => l.SequenceEqual(expectedResolvedCardNames)))).Returns(new SfPaginatedCardList { Data = new SfCard[3]});
            CardInteractor unitUnderTest = GetInteractor();

            //Act
            var result = unitUnderTest.GetCardsByNameList(deckImport);

            //Assert
            _gatewayMock.VerifyAll();
        }

        [TestMethod]
        public void GetCardsBySearchQuery_Test()
        {
            var dummyList = TestUtils.GetDummyCardList();
            _gatewayMock.Setup(m => m.GetCardsBySearchQuery(It.IsAny<string>())).Returns(dummyList);
            int maxCountOfItems = 3;
            CardInteractor unitUnderTest = GetInteractor();

            var result = unitUnderTest.GetCardsBySearchQuery("aaaa", maxCountOfItems);

            Assert.IsNotNull(result);
            Assert.AreEqual(maxCountOfItems, result.Item1.Count);
            Assert.AreEqual(dummyList.CardCount, result.Item2);
        }

        [TestMethod]
        public void GetPrintsOfCard_Test()
        {
            var dummyList = TestUtils.GetDummyCardList();
            _gatewayMock.Setup(m => m.GetPrintsOfCard(It.IsAny<Guid>())).Returns(dummyList);
            CardInteractor unitUnderTest = GetInteractor();

            var result = unitUnderTest.GetPrintsOfCard(Guid.Empty);

            Assert.IsNotNull(result);
            Assert.AreEqual(dummyList.CardCount, result.Count);
        }

        private CardInteractor GetInteractor()
        {
            return new CardInteractor(_gatewayMock.Object);
        }
    }
}
