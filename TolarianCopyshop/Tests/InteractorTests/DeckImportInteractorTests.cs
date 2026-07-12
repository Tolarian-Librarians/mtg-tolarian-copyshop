using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

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
            _rep = new MockRepository(MockBehavior.Strict);
            _cardGatewayMock = _rep.Create<ICardDataGateway>();
            _translatorMock = _rep.Create<ISetCodeTranslator>();
            _parserMock = _rep.Create<IImportStringParser>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _rep.VerifyAll();
        }

        [TestMethod]
        public void GetCardsForImport_Test()
        {
            //Arrange
            DeckImportInteractor unitUnderTest = GetInteractor();
            SetupMocks();

            //Act
            (List<SfCard> cards, string notFound) = unitUnderTest.GetCardsForImport(dummyImportString);

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(notFound));
            Assert.IsNotNull(cards);
            Assert.HasCount(16, cards);
            _cardGatewayMock.Verify(m => m.GetCardCollectionByIdentifiers(It.IsAny<List<GetCardCollectionRequest>>()), Times.Once);
            _translatorMock.Verify(m => m.TranslateArenaCodeToScryfallCode(It.IsAny<string>()), Times.Exactly(3));
        }

        [TestMethod]
        public void GetCardsForImport_NothingFound_Test()
        {
            //Arrange
            DeckImportInteractor unitUnderTest = GetInteractor();
            SetupMocks();

            SfCardCollection notFoundResult = new SfCardCollection
            {
                Data = Array.Empty<SfCard>(),
                NotFound = new SfIdentifier[]
                {
                    new SfIdentifier(),
                },
            };

            _cardGatewayMock.Setup(m => m.GetCardCollectionByIdentifiers(It.IsAny<List<GetCardCollectionRequest>>())).Returns(notFoundResult);

            //Act
            (List<SfCard> cards, string notFound) = unitUnderTest.GetCardsForImport(dummyImportString);

            //Assert
            _cardGatewayMock.Verify(m => m.GetCardCollectionByIdentifiers(It.IsAny<List<GetCardCollectionRequest>>()), Times.Exactly(2));
        }

        [TestMethod]
        public void ImportFromTappedOut_Test()
        {
            Uri uri = new Uri("https://tappedout.net/mtg-decks/your-life-can-depend-on-this-dont-blink/");
            (List<SfCard>, string notFound) result = GetInteractor().ImportFromTappedOut(uri);
        }

        private DeckImportInteractor GetInteractor()
        {
            return new DeckImportInteractor(_cardGatewayMock.Object, _translatorMock.Object, _parserMock.Object);
        }

        private void SetupMocks()
        {
            _parserMock.Setup(m => m.ResolvePreImportCardsFromImportString(It.Is<List<string>>(p => p == dummyImportString))).Returns(preImportCards);
            _translatorMock.Setup(m => m.TranslateArenaCodeToScryfallCode(It.Is<string>(p => p == "CN2"))).Returns("CN2");
            _translatorMock.Setup(m => m.TranslateArenaCodeToScryfallCode(It.Is<string>(p => p == "RNA"))).Returns("RNA");
            _translatorMock.Setup(m => m.TranslateArenaCodeToScryfallCode(It.Is<string>(p => p == "DAR"))).Returns("DOM");
            _cardGatewayMock.Setup(m => m.GetCardCollectionByIdentifiers(It.IsAny<List<GetCardCollectionRequest>>())).Returns(dummyResult);
        }

        private readonly List<string> dummyImportString = new List<string>
        {
            "4 Birds of Paradise (CN2) 176",
            "4 Collision // Colossus (RNA) 223",
            "8 Forest (DAR) 254",
        };

        private readonly List<KeyValuePair<PreImportCard, int>> preImportCards = new List<KeyValuePair<PreImportCard, int>>
        {
            new KeyValuePair<PreImportCard, int>(new PreImportCard{ CardName = "Birds of Paradise", SetCode = "CN2" }, 4),
            new KeyValuePair<PreImportCard, int>(new PreImportCard{ CardName = "Collision // Colossus", SetCode = "RNA" }, 4),
            new KeyValuePair<PreImportCard, int>(new PreImportCard{ CardName = "Forest", SetCode = "DAR" }, 8),
        };

        private readonly SfCardCollection dummyResult = new SfCardCollection
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