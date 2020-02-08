using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refit;
using Tolarian.Copyshop.Business.Models;
using Tolarian.Copyshop.ScryfallDataAccess;
using System.Linq;

namespace Tests.ScryfallDataAccessTests
{
    [TestClass]
    public class DataMapperTest
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
            SfPaginatedCardList result = service.GetCardsBySearchQuery(expectedCardName).Result;

            //Assert
            Assert.IsTrue(result.CardCount == 1);
            Assert.AreEqual(expectedCardName, result.Data[0].Name);

        }

        [TestMethod]
        public void GetCardsBySearchQueryUniqueArt_Test()
        {
            string expectedCardName = "Sol Ring";

            //Arrange

            //Act
            SfPaginatedCardList result = service.GetCardsBySearchQueryUniqueArt(expectedCardName).Result;

            //Assert
            Assert.IsTrue(result.CardCount == 4);
            Assert.IsTrue(result.Data.All(c => c.Name.Equals(expectedCardName)));

        }
    }
}
