using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.Enums;
using Tolarian.Copyshop.Business.Models.SfCardInfo;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.ResponseObjects;

namespace Tests.ControllerTests
{

    [TestClass]
    public class CardControllerTest
    {
        MockRepository _repo;
        Mock<ICardDataRequester> _requesterMock;
        Mock<IDeckImportInteractor> _importerMock;

        [TestInitialize]
        public void Initialize()
        {
            this._repo = new MockRepository(MockBehavior.Strict);
            this._requesterMock = this._repo.Create<ICardDataRequester>();
            this._importerMock = this._repo.Create<IDeckImportInteractor>();
        }

        [TestMethod]
        public void GetSearchResults_Test()
        {
            //Arrange
            SfCard dummy = TestUtils.GetDummyCard();
            this._requesterMock.Setup(m => m.GetCardsBySearchQuery(It.IsAny<string>(), It.IsAny<int>())).Returns((new List<SfCard> { dummy }, "1"));
            CardController unitUnterTest = this.GetController();

            //Act
            var response = unitUnterTest.GetSearchResults("", 1);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual("1", response.ResultsCount);
            SearchCard result = response.Results[0];
            Assert.AreEqual(dummy.Name, result.Name);
            Assert.AreEqual(dummy.PrintId, result.PrintId);
        }

        [TestMethod]
        public void GetCardByPrintId_StandardCard_Test()
        {
            //Arrange
            SfCard expected = TestUtils.GetDummyCard();
            this._requesterMock.Setup(m => m.GetCardByPrintId(It.IsAny<Guid>())).Returns(expected);
            CardController unitUnterTest = this.GetController();

            //Act
            FullCardResponse response = unitUnterTest.GetCardByPrintId(Guid.Empty);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Card);
            Assert.AreEqual(1, response.Card.CardFaces.Count);
            Assert.AreEqual(expected.Name, response.Card.CardFaces.First().Name);

            //Check that the legalities were separated correctly
            List<MtgPlayModes> expectedFirstHalf = new List<MtgPlayModes> { MtgPlayModes.Commander, MtgPlayModes.Brawl, MtgPlayModes.Duel, MtgPlayModes.Future };
            List<MtgPlayModes> expectedSecondHalf = new List<MtgPlayModes> { MtgPlayModes.Historic, MtgPlayModes.Legacy, MtgPlayModes.Modern };

            Assert.IsTrue(expectedFirstHalf.All(legality => response.Card.Legalities.ContainsKey(legality.ToString())));
            Assert.IsTrue(expectedSecondHalf.All(legality => response.Card.Legalities.ContainsKey(legality.ToString())));
        }

        [TestMethod]
        public void GetCardByPrintId_DoubleCard_Test()
        {
            //Arrange
            SfCard expected = TestUtils.GetDummyDoubleCard();

            this._requesterMock.Setup(m => m.GetCardByPrintId(It.IsAny<Guid>())).Returns(expected);
            CardController unitUnterTest = this.GetController();

            //Act
            FullCardResponse response = unitUnterTest.GetCardByPrintId(Guid.Empty);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Card);
            Assert.AreEqual(1, response.Card.CardFaces.Count);
            Assert.AreEqual(expected.Name, response.Card.CardFaces.First().Name);
            Assert.AreEqual("Text 1 // Text 2", response.Card.CardFaces.First().Text);

        }

        [TestMethod]
        public void GetCardByPrintId_DualFaceCard_Test()
        {
            //Arrange
            SfCard expected = TestUtils.GetDummyDualFacedCard();

            this._requesterMock.Setup(m => m.GetCardByPrintId(It.IsAny<Guid>())).Returns(expected);
            CardController unitUnterTest = this.GetController();

            //Act
            FullCardResponse response = unitUnterTest.GetCardByPrintId(Guid.Empty);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Card);
            Assert.AreEqual(2, response.Card.CardFaces.Count);
            Assert.AreEqual(expected.CardFaces[0].Name, response.Card.CardFaces.First().Name);
            Assert.AreEqual(expected.CardFaces[1].Name, response.Card.CardFaces.Skip(1).First().Name);
        }

        [TestMethod]
        public void GetPrintsOfCard_LongSetName_Test()
        {
            SfCard dummy = TestUtils.GetDummyCard();
            dummy.SetName = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            dummy.SetCode = "SSS";
            this._requesterMock.Setup(m => m.GetPrintsOfCard(It.IsAny<Guid>())).Returns(new List<SfCard> { dummy });
            CardController unitUnterTest = this.GetController();

            var result = unitUnterTest.GetArtworksOfCard(Guid.Empty);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Artworks.Count);
            Assert.AreEqual(23, result.Artworks[0].SetName.Length);
            Assert.IsTrue(result.Artworks[0].SetName.EndsWith("..."));
        }

        [TestMethod]
        public void GetPrintsOfCard_Test()
        {
            SfCard dummy = TestUtils.GetDummyCard();
            dummy.SetName = "Kaladesh";
            dummy.SetCode = "KDH";
            this._requesterMock.Setup(m => m.GetPrintsOfCard(It.IsAny<Guid>())).Returns(new List<SfCard> { dummy });
            CardController unitUnterTest = this.GetController();

            var result = unitUnterTest.GetArtworksOfCard(Guid.Empty);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Artworks.Count);
            Assert.AreEqual(result.Artworks[0].SetName.Length, dummy.SetName.Length);
            Assert.IsFalse(result.Artworks[0].SetName.EndsWith("..."));
        }

        private CardController GetController()
        {
            return new CardController(this._requesterMock.Object, this._importerMock.Object);
        }
    }
}
