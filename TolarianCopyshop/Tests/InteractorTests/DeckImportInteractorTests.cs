using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.DbRequestModels;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.SfCardInfo;
using Tolarian.Copyshop.Business.UseCaseInteractors;

namespace Tests.InteractorTests
{
    [TestClass]
    public class DeckImportInteractorTests
    {
        MockRepository _rep;
        Mock<ICardDataGateway> _cardGatewayMock;
        Mock<ISetDataGateway> _setGatewayMock;

        [TestInitialize]
        public void Initialize()
        {
            _rep = new MockRepository(MockBehavior.Strict);
            _cardGatewayMock = _rep.Create<ICardDataGateway>();
            _setGatewayMock = _rep.Create<ISetDataGateway>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _rep.VerifyAll();
        }


        //[TestMethod]
        public void GetCardsByNameList_Test()
        {
            //Arrange
            List<string> deckImport = new List<string> { "3 Aether Spellbomb (MRD) 196", "1 Ancient Tomb (TMP)", "0 Ashnod's Altar (5ED) 218", "" };

            List<string> expectedResolvedCardNames = new List<string> { "Aether Spellbomb", "Aether Spellbomb", "Aether Spellbomb", "Ancient Tomb" };
            _cardGatewayMock.Setup(m => m.GetCardCollectionByIdentifiers(It.Is<List<GetCardCollectionRequest>>(l => l.Select(r => r.Name).SequenceEqual(expectedResolvedCardNames))))
                .Returns(new SfCardCollection { Data = new SfCard[3], NotFound = Array.Empty<SfIdentifier>() });
            var unitUnderTest = GetInteractor();

            //Act
            (List<SfCard> cards, string notFound) = unitUnderTest.GetCardsForImport(deckImport);

            //Assert
            _cardGatewayMock.VerifyAll();
        }

        private DeckImportInteractor GetInteractor()
        {
            return new DeckImportInteractor(_cardGatewayMock.Object, _setGatewayMock.Object);
        }
    }
}
