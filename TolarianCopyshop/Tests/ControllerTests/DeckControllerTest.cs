using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.DeckInfo;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;

namespace Tests.ControllerTests
{
    [TestClass]
    public class DeckControllerTest
    {
        MockRepository _rep = new MockRepository(MockBehavior.Strict);
        Mock<IDeckInfoInteractor> _deckInfoMock;

        [TestInitialize]
        public void Initialize()
        {
            _deckInfoMock = _rep.Create<IDeckInfoInteractor>();
        }

        [TestMethod]
        public void GetTotalCardCountOfDeck_Test()
        {
            _deckInfoMock.Setup(m => m.GetTotalCardCountOfDeck(It.IsAny<List<DeckInfoCard>>())).Returns(null);
            List<IFullCard> dummyDeck = new List<IFullCard>
            {
                new FullCard { CardCount = 3, PrintId = new Guid("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"), CardFaces = new List<CardFace> { new CardFace { Name = "Emrakul" } } },
                new FullCard { CardCount = 1, PrintId = new Guid("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb"), CardFaces = new List<CardFace> { new CardFace { Name = "Dusk" } } },
                new FullCard { CardCount = 1, PrintId = new Guid("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb"), CardFaces = new List<CardFace> { new CardFace { Name = "Dawn" } } },
                new FullCard { CardCount = 2, PrintId = new Guid("cccccccccccccccccccccccccccccccc"), CardFaces = new List<CardFace> { new CardFace { Name = "face1" } } },
                new FullCard { CardCount = 2, PrintId = new Guid("cccccccccccccccccccccccccccccccc"), CardFaces = new List<CardFace> { new CardFace { Name = "face2" } } },
            };
            DeckController unitUnderTest = GetController();

            unitUnderTest.GetTotalCardCountOfDeck(dummyDeck);

            int expectedListCountAfterMapping = 3;
            int expectedCopiesOfSingleMultifacedCard = 1;
            int expectedCopiesOfDoubleMultifacedCard = 2;
            _deckInfoMock.Verify(m => m.GetTotalCardCountOfDeck
            (It.Is<List<DeckInfoCard>>(
                a => a.Count == expectedListCountAfterMapping && a[1].Copies == expectedCopiesOfSingleMultifacedCard && a[2].Copies == expectedCopiesOfDoubleMultifacedCard)
            ), Times.Once());
        }

        private DeckController GetController()
        {
            return new DeckController(_deckInfoMock.Object);
        }
    }
}
