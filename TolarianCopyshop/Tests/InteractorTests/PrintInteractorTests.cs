using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tolarian.Copyshop.Business.UseCaseInteractors;

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
            unitUnderTest.PrintDeck(new PrintDialog(), new Stack<Uri>(new Uri[]
            {
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
                new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218"),
            }));
        }
    }
}
