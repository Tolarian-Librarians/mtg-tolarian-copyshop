using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Business.DbRequestModels;
using Tolarian.Copyshop.Business.EntitiesModels;
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
        Mock<ISetCodeTranslator> _translatorMock;
        Mock<IImportStringParser> _parserMock;

        [TestInitialize]
        public void Initialize()
        {
            this._rep = new MockRepository(MockBehavior.Strict);
            this._cardGatewayMock = this._rep.Create<ICardDataGateway>();
            this._translatorMock = this._rep.Create<ISetCodeTranslator>();
            this._parserMock = this._rep.Create<IImportStringParser>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this._rep.VerifyAll();
        }

        [TestMethod]
        public void GetCardsForImport_Test()
        {
            //Arrange
            DeckImportInteractor unitUnderTest = this.GetInteractor();
            this.SetupMocks();

            //Act
            (List<SfCard> cards, string notFound) = unitUnderTest.GetCardsForImport(this.dummyImportString);

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(notFound));
            Assert.IsNotNull(cards);
            Assert.AreEqual(cards.Count, 16);
            this._cardGatewayMock.Verify(m => m.GetCardCollectionByIdentifiers(It.IsAny<List<GetCardCollectionRequest>>()), Times.Once);
            this._translatorMock.Verify(m => m.TranslateArenaCodeToScryfallCode(It.IsAny<string>()), Times.Exactly(3));
        }

        [TestMethod]
        public void GetCardsForImport_NothingFound_Test()
        {
            //Arrange
            DeckImportInteractor unitUnderTest = this.GetInteractor();
            this.SetupMocks();

            SfCardCollection notFoundResult = new SfCardCollection
            {
                Data = Array.Empty<SfCard>(),
                NotFound = new SfIdentifier[]
                {
                    new SfIdentifier(),
                },
            };

            this._cardGatewayMock.Setup(m => m.GetCardCollectionByIdentifiers(It.IsAny<List<GetCardCollectionRequest>>())).Returns(notFoundResult);

            //Act
            (List<SfCard> cards, string notFound) = unitUnderTest.GetCardsForImport(this.dummyImportString);

            //Assert
            this._cardGatewayMock.Verify(m => m.GetCardCollectionByIdentifiers(It.IsAny<List<GetCardCollectionRequest>>()), Times.Exactly(2));
        }

        [TestMethod]
        public void ImportFromTappedOut_Test()
        {
            Uri uri = new Uri("https://tappedout.net/mtg-decks/your-life-can-depend-on-this-dont-blink/");
            (List<SfCard>, string notFound) result = this.GetInteractor().ImportFromTappedOut(uri);
        }

        private DeckImportInteractor GetInteractor()
        {
            return new DeckImportInteractor(this._cardGatewayMock.Object, this._translatorMock.Object, this._parserMock.Object);
        }

        private void SetupMocks()
        {
            this._parserMock.Setup(m => m.ResolvePreImportCardsFromImportString(It.Is<List<string>>(p => p == this.dummyImportString))).Returns(this.preImportCards);
            this._translatorMock.Setup(m => m.TranslateArenaCodeToScryfallCode(It.Is<string>(p => p == "CN2"))).Returns("CN2");
            this._translatorMock.Setup(m => m.TranslateArenaCodeToScryfallCode(It.Is<string>(p => p == "RNA"))).Returns("RNA");
            this._translatorMock.Setup(m => m.TranslateArenaCodeToScryfallCode(It.Is<string>(p => p == "DAR"))).Returns("DOM");
            this._cardGatewayMock.Setup(m => m.GetCardCollectionByIdentifiers(It.IsAny<List<GetCardCollectionRequest>>())).Returns(this.dummyResult);
        }

        private List<string> dummyImportString = new List<string>
        {
            "4 Birds of Paradise (CN2) 176",
            "4 Collision // Colossus (RNA) 223",
            "8 Forest (DAR) 254",
        };

        private List<KeyValuePair<PreImportCard, int>> preImportCards = new List<KeyValuePair<PreImportCard, int>>
        {
            new KeyValuePair<PreImportCard, int>(new PreImportCard{ CardName = "Birds of Paradise", SetCode = "CN2" }, 4),
            new KeyValuePair<PreImportCard, int>(new PreImportCard{ CardName = "Collision // Colossus", SetCode = "RNA" }, 4),
            new KeyValuePair<PreImportCard, int>(new PreImportCard{ CardName = "Forest", SetCode = "DAR" }, 8),
        };

        private SfCardCollection dummyResult = new SfCardCollection
        {
            Data = new SfCard[]
            {
                new SfCard { Name = "Birds of Paradise" },
                new SfCard { Name = "Collision // Colossus" },
                new SfCard { Name = "Forest" },
            },
            NotFound = Array.Empty<SfIdentifier>(),
        };
    }
}
