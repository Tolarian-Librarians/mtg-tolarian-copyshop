using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refit;
using Tolarian.Copyshop.ScryfallDataAccess;
using System.Linq;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tests.ScryfallDataAccessTests
{
    [TestClass]
    public class CardDataMapperTest
    {
        IScryfallApi service;

        [TestInitialize]
        public void Initialize()
        {
            service = RestService.For<IScryfallApi>("https://api.scryfall.com");
        }

        [TestMethod]
        public void GetCardsBySearchQuery_Test()
        {
            string expectedCardName = "Toothy, Imaginary Friend";
            //Arrange

            //Act
            SfPaginatedCardList result = service.GetCardsBySearchQuery(expectedCardName).Result.Content;

            //Assert
            Assert.IsTrue(result.CardCount == 1);
            Assert.AreEqual(expectedCardName, result.Data[0].Name);
        }
        
        [TestMethod]
        public void GetCardById_DoubleFaced_Test()
        {
            //Arrange
            Guid id = new Guid("b3b87bfc-f97f-4734-94f6-e3e2f335fc4d");


            //Act
            SfCard result = service.GetCardById(id).Result.Content;

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual("Growing Rites of Itlimoc // Itlimoc, Cradle of the Sun", result.Name);
            Assert.AreEqual(2, result.CardFaces.Count);
        }
        
        [TestMethod]
        public void GetCardById_DualCard_Test()
        {
            //Arrange
            Guid id = new Guid("e9d5aee0-5963-41db-a22b-cfea40a967a3");


            //Act
            SfCard result = service.GetCardById(id).Result.Content;

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual("Dusk // Dawn", result.Name);
            Assert.IsTrue(result.CardFaces.All(c => c.ImageUris == null));
        }
        
        [TestMethod]
        public void GetCardsByNameList_Test()
        {
            //Arrange
            List<string> names = new List<string> { "Sol Ring", "Dusk // Dawn"};


            //Act
            CardDataMapper mapper = new CardDataMapper();
            SfPaginatedCardList result = mapper.GetCardsByNameList(names);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(2, result.Data.Length);
        }

    }
}
