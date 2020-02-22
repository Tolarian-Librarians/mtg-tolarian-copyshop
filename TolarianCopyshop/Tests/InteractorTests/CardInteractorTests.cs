using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Tolarian.Copyshop.Business;
using Tolarian.Copyshop.Business.Models;
using Tolarian.Copyshop.Business.Interfaces;

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
        public void GetCardsByNameList_Test()
        {
            //Arrange
            List<string> deckImport = new List<string> { "3 Aether Spellbomb (MRD) 196", "1 Ancient Tomb (TMP)",  "0 Ashnod's Altar (5ED) 218", ""};

            List<string> expectedResolvedCardNames = new List<string> { "Aether Spellbomb", "Aether Spellbomb", "Aether Spellbomb", "Ancient Tomb" };
            _gatewayMock.Setup(m => m.GetCardsByNameList(It.Is<List<string>>(l => l.SequenceEqual(expectedResolvedCardNames)))).Returns(new SfPaginatedCardList { Data = new SfCard[3]});
            CardInteractor unitUderTest = new CardInteractor(_gatewayMock.Object);

            //Act
            var result = unitUderTest.GetCardsByNameList(deckImport);

            //Assert
            _gatewayMock.VerifyAll();
        }
    }
}
