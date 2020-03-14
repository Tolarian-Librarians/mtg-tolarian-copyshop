using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Business.Models.Enums;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tests.ControllerTests
{

    [TestClass]
    public class CardControllerTest
    {
        MockRepository _repo;
        Mock<ICardDataRequester> _requesterMock;

        [TestInitialize]
        public void Initialize()
        {
            _repo = new MockRepository(MockBehavior.Strict);
            _requesterMock = _repo.Create<ICardDataRequester>();
        }

        [TestMethod]
        public void GetSearchResults_Test()
        {
            //Arrange
            SfCard dummy = TestUtils.GetDummyCard();
            _requesterMock.Setup(m => m.GetCardsBySearchQuery(It.IsAny<string>(), It.IsAny<int>())).Returns((new List<SfCard> { dummy }, 1));
            CardController unitUnterTest = GetController();

            //Act
            var response = unitUnterTest.GetSearchResults("", 1);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.ResultsCount);
            CardSearchCard result = response.Results[0];
            Assert.AreEqual(dummy.Name, result.Name);
            Assert.AreEqual(dummy.PrintId, result.PrintId);
        }

        [TestMethod]
        public void GetCardByPrintId_StandardCard_Test()
        {
            //Arrange
            SfCard expected = TestUtils.GetDummyCard();
            _requesterMock.Setup(m => m.GetCardByPrintId(It.IsAny<Guid>())).Returns(expected);
            CardController unitUnterTest = GetController();

            //Act
            List<IFullCard> response = unitUnterTest.GetCardByPrintId(Guid.Empty);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(expected.Name, response[0].Name);

            //Check that the legalities were separated correctly
            List<MtgPlayModes> expectedFirstHalf = new List<MtgPlayModes>{ MtgPlayModes.Commander, MtgPlayModes.Brawl, MtgPlayModes.Duel, MtgPlayModes.Future};
            List<MtgPlayModes> expectedSecondHalf = new List<MtgPlayModes>{ MtgPlayModes.Historic, MtgPlayModes.Legacy, MtgPlayModes.Modern};

            Assert.IsTrue(expectedFirstHalf.All(legality => response[0].Legalities1.ContainsKey(legality.ToString())));
            Assert.IsTrue(expectedSecondHalf.All(legality => response[0].Legalities2.ContainsKey(legality.ToString())));
        }

        [TestMethod]
        public void GetCardByPrintId_DoubleCard_Test()
        {
            //Arrange
            SfCard expected = TestUtils.GetDummyDoubleCard();

            _requesterMock.Setup(m => m.GetCardByPrintId(It.IsAny<Guid>())).Returns(expected);
            CardController unitUnterTest = GetController();

            //Act
            List<IFullCard> response = unitUnterTest.GetCardByPrintId(Guid.Empty);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(expected.Name, response[0].Name);
            Assert.AreEqual("Text 1 // Text 2", response[0].Text);

        }

        [TestMethod]
        public void GetCardByPrintId_DualFaceCard_Test()
        {
            //Arrange
            SfCard expected = TestUtils.GetDummyDualFacedCard();

            _requesterMock.Setup(m => m.GetCardByPrintId(It.IsAny<Guid>())).Returns(expected);
            CardController unitUnterTest = GetController();

            //Act
            List<IFullCard> response = unitUnterTest.GetCardByPrintId(Guid.Empty);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Count);
            Assert.AreEqual(expected.CardFaces[0].Name, response[0].Name);
            Assert.AreEqual(expected.CardFaces[1].Name, response[1].Name);
        }

        private CardController GetController()
        {
            return new CardController(_requesterMock.Object);
        }
    }
}
