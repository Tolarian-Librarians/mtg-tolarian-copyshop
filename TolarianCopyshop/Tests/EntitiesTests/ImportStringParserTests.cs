using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tolarian.Copyshop.Business.Entities;

namespace Tests.EntitiesTests
{
    [TestClass]
    public class ImportStringParserTests
    {
        [TestMethod]
        public void Resolve_Mtga_Import_Test()
        {
            List<string> dummyImportString = new List<string>
            {
                "4 Birds of Paradise (CN2) 176",
                "4 Collision // Colossus (RNA) 223",
                "8 Forest (DAR) 254",
            };

            ImportStringParser unitUnderTest = GetParser();

            List<KeyValuePair<Tolarian.Copyshop.Business.EntitiesModels.PreImportCard, int>> result = unitUnderTest.ResolvePreImportCardsFromImportString(dummyImportString);

            Assert.IsNotNull(result);
            Assert.AreEqual(dummyImportString.Count, result.Count);
            Assert.AreEqual(4, result[0].Value);
            Assert.AreEqual(4, result[1].Value);
            Assert.AreEqual(8, result[2].Value);
        }

        [TestMethod]
        public void Resolve_Mixed_Import_Test()
        {
            List<string> dummyImportString = new List<string>
            {
                "4 Birds of Paradise",
                "Collision // Colossus (RNA)",
                "8 Forest 254",
            };

            ImportStringParser unitUnderTest = GetParser();

            List<KeyValuePair<Tolarian.Copyshop.Business.EntitiesModels.PreImportCard, int>> result = unitUnderTest.ResolvePreImportCardsFromImportString(dummyImportString);

            Assert.IsNotNull(result);
            Assert.AreEqual(dummyImportString.Count, result.Count);
            Assert.AreEqual(4, result[0].Value);
            Assert.AreEqual(1, result[1].Value);
            Assert.AreEqual("RNA", result[1].Key.SetCode);
            Assert.AreEqual(8, result[2].Value);
            Assert.IsNull(result[2].Key.SetCode);
        }

        private ImportStringParser GetParser()
        {
            return new ImportStringParser();
        }
    }
}