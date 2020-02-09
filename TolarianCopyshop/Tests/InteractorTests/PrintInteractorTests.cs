using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tolarian.Copyshop.Business;

namespace Tests.PrintTests
{
    [TestClass]
    public class PrintInteractorTests
    {
        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public void PrintDeck_Test()
        {
            PrintInteractor unitUnderTest = new PrintInteractor();
            unitUnderTest.PrintDeck(new List<Uri> { new Uri("https://img.scryfall.com/cards/large/front/3/2/32982ed2-96e4-4cc5-8562-744b06bca239.jpg?1572491355") });
        }
    }
}
