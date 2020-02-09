using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Business;
using AutoMapper;
using Tolarian.Copyshop.Business.Models;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.ScreenPresenter.AutoMapper;
using Tolarian.Copyshop.Business.Models.Enums;

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
        public void GetCardById_ImageUrisAndLegalitiesNull_Test()
        {
            //Arrange
            SfCard expected = GetDummyCard();
            expected.Legalities = null;
            expected.ImageUris = null;
            _requesterMock.Setup(m => m.GetCardById(It.IsAny<Guid>())).Returns(expected);
            CardController unitUnterTest = new CardController(_requesterMock.Object, GetMapper());

            //Act
            List<FullCardResponse> response = unitUnterTest.GetCardById(Guid.Empty, out string err);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(expected.Name, response[0].Name);
        }

        [TestMethod]
        public void GetCardById_StandardCard_Test()
        {
            //Arrange
            SfCard expected = GetDummyCard();
            _requesterMock.Setup(m => m.GetCardById(It.IsAny<Guid>())).Returns(expected);
            CardController unitUnterTest = new CardController(_requesterMock.Object, GetMapper());

            //Act
            List<FullCardResponse> response = unitUnterTest.GetCardById(Guid.Empty, out string err);

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
        public void GetCardById_DoubleCard_Test()
        {
            //Arrange
            SfCard expected = GetDummyDoubleCard();

            _requesterMock.Setup(m => m.GetCardById(It.IsAny<Guid>())).Returns(expected);
            CardController unitUnterTest = new CardController(_requesterMock.Object, GetMapper());

            //Act
            List<FullCardResponse> response = unitUnterTest.GetCardById(Guid.Empty, out string err);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(expected.Name, response[0].Name);
            Assert.AreEqual("Text 1 // Text 2", response[0].Text);

        }

        [TestMethod]
        public void GetCardById_DualFaceCard_Test()
        {
            //Arrange
            SfCard expected = GetDummyDualFacedCard();

            _requesterMock.Setup(m => m.GetCardById(It.IsAny<Guid>())).Returns(expected);
            CardController unitUnterTest = new CardController(_requesterMock.Object, GetMapper());

            //Act
            List<FullCardResponse> response = unitUnterTest.GetCardById(Guid.Empty, out string err);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Count);
            Assert.AreEqual(expected.CardFaces[0].Name, response[0].Name);
            Assert.AreEqual(expected.CardFaces[1].Name, response[1].Name);
        }

        private SfCard GetDummyDualFacedCard()
        {
            SfCard card = GetDummyDoubleCard();
            card.ImageUris = null;

            return card;
        }

        private SfCard GetDummyDoubleCard()
        {
            SfCard card = GetDummyCard();
            card.Text = null;
            card.CardFaces = new List<SfCardFace> { new SfCardFace {Name = "Face 1", Text = "Text 1", ImageUris = new Dictionary<CardImageTypes, Uri>()}, new SfCardFace { Name = "Face 2", Text = "Text 2", ImageUris = new Dictionary<CardImageTypes, Uri>() } };

            return card;
        }

        private SfCard GetDummyCard()
        {
            return new SfCard
            {
                Name = "Dummy Mtg Card",
                Id = Guid.Empty,
                ImageUris = new Dictionary<CardImageTypes, Uri> { { CardImageTypes.Png, new Uri("https://img.scryfall.com/cards/png/front/e/6/e672d408-997c-4a19-810a-3da8411eecf2.png?1568004958") }, { CardImageTypes.Small, new Uri("https://img.scryfall.com/cards/small/front/e/6/e672d408-997c-4a19-810a-3da8411eecf2.jpg?1568004958") } },
                Legalities = new Dictionary<MtgPlayModes, string> { { MtgPlayModes.Commander, "legal" }, { MtgPlayModes.Brawl, "legal" }, { MtgPlayModes.Duel, "legal" }, { MtgPlayModes.Future, "legal" }, { MtgPlayModes.Historic, "legal" }, { MtgPlayModes.Legacy, "legal" }, { MtgPlayModes.Modern, "legal" } },
                Text = "Does dummy stuff."
            };
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Add all profiles in current assembly
                cfg.AddProfile(new AutoMapperProfile());
            });

            return new Mapper(config);
        }
    }
}
