using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.Entities;

namespace Tests.EntitiesTests
{
    [TestClass]
    public class ImportStringParserTests
    {
        [TestMethod]
        public void Resolve_Mtga_Import_Test()
        {
            var dummyImportString = new List<string>
            {
                "4 Birds of Paradise (CN2) 176",
                "4 Collision // Colossus (RNA) 223",
                "8 Forest (DAR) 254",
            };

            var unitUnderTest = GetParser();

            var result = unitUnderTest.ResolvePreImportCardsFromImportString(dummyImportString);

            Assert.IsNotNull(result);
            Assert.AreEqual(dummyImportString.Count, result.Count);
            Assert.AreEqual(4, result[result.Keys.First()]);
            Assert.AreEqual(4, result[result.Keys.Skip(1).First()]);
            Assert.AreEqual(8, result[result.Keys.Skip(2).First()]);
        }
        
        [TestMethod]
        public void Resolve_Mixed_Import_Test()
        {
            var dummyImportString = new List<string>
            {
                "4 Birds of Paradise",
                "Collision // Colossus (RNA)",
                "8 Forest 254",
            };

            var unitUnderTest = GetParser();

            var result = unitUnderTest.ResolvePreImportCardsFromImportString(dummyImportString);

            Assert.IsNotNull(result);
            Assert.AreEqual(dummyImportString.Count, result.Count);
            Assert.AreEqual(4, result[result.Keys.First()]);
            Assert.AreEqual(1, result[result.Keys.Skip(1).First()]);
            Assert.AreEqual("RNA", result.Keys.Skip(1).First().SetCode);
            Assert.AreEqual(8, result[result.Keys.Skip(2).First()]);
            Assert.IsNull(result.Keys.Skip(2).First().SetCode);
        }

        private ImportStringParser GetParser()
        {
            return new ImportStringParser();
        }
    }
}
