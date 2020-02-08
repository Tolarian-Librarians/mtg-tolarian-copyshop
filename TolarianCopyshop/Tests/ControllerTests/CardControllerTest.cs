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
            SfCard expected = new SfCard
            {
                Name = "Sol Ring",
                Id = Guid.Empty,
                ImageUris = new Dictionary<CardImageTypes, Uri>(),
                Legalities = new Dictionary<MtgPlayModes, string>(),
                Text = "Tap for 2 colorless."
            };

            //Act
            _requesterMock.Setup(m => m.GetCardById(It.IsAny<Guid>())).Returns(expected);
            CardController unitUnterTest = new CardController(Mock.Of<ICardDataPresenter>(), _requesterMock.Object, GetMapper());

            GetCardByIdResponse response = unitUnterTest.GetCardById(Guid.Empty);
        }
        
        [TestMethod]
        public void GetCardById_Test()
        {
            //Arrange
            SfCard expected = new SfCard
            {
                Name = "Sol Ring",
                Id = Guid.Empty,
                ImageUris = new Dictionary<CardImageTypes, Uri> { { CardImageTypes.Png, new Uri("https://img.scryfall.com/cards/png/front/e/6/e672d408-997c-4a19-810a-3da8411eecf2.png?1568004958") }, { CardImageTypes.Small, new Uri("https://img.scryfall.com/cards/small/front/e/6/e672d408-997c-4a19-810a-3da8411eecf2.jpg?1568004958") } },
                Legalities = new Dictionary<MtgPlayModes, string> { { MtgPlayModes.Commander, "legal"} },
                Text = "Tap for 2 colorless."
            };

            //Act
            _requesterMock.Setup(m => m.GetCardById(It.IsAny<Guid>())).Returns(expected);
            CardController unitUnterTest = new CardController(Mock.Of<ICardDataPresenter>(), _requesterMock.Object, GetMapper());

            GetCardByIdResponse response = unitUnterTest.GetCardById(Guid.Empty);
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
