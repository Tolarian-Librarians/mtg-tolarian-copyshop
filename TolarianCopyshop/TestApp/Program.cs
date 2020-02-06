using CardInfoAccess;
using CardInfoAccess.Model;
using Refit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var scryfallClient = RestService.For<IScryfallApi>("https://api.scryfall.com");

            Guid tempGuid = new Guid("c8b4d10d-ecf4-4dad-89d3-12333b522358");

            MtgCard result = scryfallClient.GetCardById(tempGuid).Result;
        }
    }
}
