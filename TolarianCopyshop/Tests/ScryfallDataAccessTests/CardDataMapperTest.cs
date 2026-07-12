using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Refit;

using Tolarian.Copyshop.Business.DbRequestModels;
using Tolarian.Copyshop.Business.Models.SfCardInfo;
using Tolarian.Copyshop.ScryfallDataAccess;

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
        public void GetCardNamesByAutoCompleteQuery_Test()
        {
            //Arrange
            const string query = "Toothy, Imagina";
            const string expectedCardName = "Toothy, Imaginary Friend";
            CardDataMapper mapper = GetMapper();

            //Act
            SfCatalog result = mapper.GetCardNamesByAutoCompleteQuery(query);

            //Assert
            Assert.AreEqual(1, result.ObjectCount);
            Assert.AreEqual(expectedCardName, result.Data[0]);
        }

        [TestMethod]
        public void GetCardById_DoubleFaced_Test()
        {
            //Arrange
            Guid id = new Guid("b3b87bfc-f97f-4734-94f6-e3e2f335fc4d");
            CardDataMapper mapper = GetMapper();

            //Act
            SfCard result = mapper.GetCardByPrintId(id);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Growing Rites of Itlimoc // Itlimoc, Cradle of the Sun", result.Name);
            Assert.HasCount(2, result.CardFaces);
        }

        [TestMethod]
        public void GetCardById_DualCard_Test()
        {
            //Arrange
            Guid id = new Guid("e9d5aee0-5963-41db-a22b-cfea40a967a3");
            CardDataMapper mapper = GetMapper();

            //Act
            SfCard result = mapper.GetCardByPrintId(id);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Dusk // Dawn", result.Name);
            Assert.IsTrue(result.CardFaces.All(c => c.ImageUris == null));
        }

        [TestMethod]
        public void GetCardByExactName_RelatedCards_Test()
        {
            //Arrange
            Guid id = new Guid("ebdf2f50-f69a-47c4-a75f-ff55781bb0c8");
            CardDataMapper mapper = GetMapper();

            //Act
            SfCard result = mapper.GetCardByPrintId(id);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Toothy, Imaginary Friend", result.Name);
            Assert.IsNotNull(result.RelatedCards);
        }

        [TestMethod]
        public void GetPrintsOfCard_Test()
        {
            Guid dummyOracleGuid = new Guid("900ca697-ad38-4b2b-bc74-2ff7eb6ea951"); //Emrakul, the aeons torn

            CardDataMapper mapper = GetMapper();
            var result = mapper.GetPrintsOfCard(dummyOracleGuid);

            Assert.IsNotNull(result);
            Assert.HasCount(8, result);
            Assert.AreEqual(8, result.Count(x => x.CardId == dummyOracleGuid));
        }

        [TestMethod]
        public void GetCardsByNameList_Test()
        {
            //Arrange
            List<GetCardCollectionRequest> names = new List<GetCardCollectionRequest>
            {
                new GetCardCollectionRequest { Name = "Sol Ring" },
                new GetCardCollectionRequest { Name = "Dusk // Dawn" }
            };
            CardDataMapper mapper = GetMapper();

            //Act
            SfCardCollection result = mapper.GetCardCollectionByIdentifiers(names);

            //Assert
            Assert.IsNotNull(result);
            Assert.HasCount(2, result.Data);
        }

        [TestMethod]
        public void GetTokensByQuery_Test()
        {
            //Arrange
            const string query = "Gobl";
            CardDataMapper mapper = GetMapper();

            //Act
            var result = mapper.GetTokensByQuery(query);

            //Assert
            Assert.HasCount(8, result);
            Assert.DoesNotContain(c => !c.Name.Contains(query) && !c.TypeLine.Contains("Token"), result);
        }

        private static CardDataMapper GetMapper()
        {
            return new CardDataMapper();
        }
    }
}