using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.DeckInfo;
using Tolarian.Copyshop.Business.Models.SfCardInfo;
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
                new FullCardResponse { CardCount = 3, Id = new Guid("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"), Name = "Emrakul"},
                new FullCardResponse { CardCount = 1, Id = new Guid("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb"), Name = "Dusk"},
                new FullCardResponse { CardCount = 1, Id = new Guid("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb"), Name = "Dawn"},                
                new FullCardResponse { CardCount = 2, Id = new Guid("cccccccccccccccccccccccccccccccc"), Name = "face1"},
                new FullCardResponse { CardCount = 2, Id = new Guid("cccccccccccccccccccccccccccccccc"), Name = "face2"},
            };

            DeckController unitUnderTest = new DeckController(_deckInfoMock.Object, ControllerTestUtils.GetMapper());

            unitUnderTest.GetTotalCardCountOfDeck(dummyDeck);

            int expectedListCountAfterMapping = 3;
            int expectedCopiesOfSingleMultifacedCard = 1;
            int expectedCopiesOfDoubleMultifacedCard = 2;
            _deckInfoMock.Verify(m => m.GetTotalCardCountOfDeck
            (It.Is<List<DeckInfoCard>>(
                a => a.Count == expectedListCountAfterMapping && a[1].Copies == expectedCopiesOfSingleMultifacedCard && a[2].Copies == expectedCopiesOfDoubleMultifacedCard)
            ), Times.Once());
        }
    }
}
