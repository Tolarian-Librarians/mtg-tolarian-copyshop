using CardInfoAccess;
using CardInfoAccess.Model;
using Refit;
using System;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        public static Task<MtgCard> MtgCard { get; private set; }

        static void Main(string[] args)
        {
            var scryfallClient = RestService.For<IScryfallApi>("https://api.scryfall.com");

            MtgCard result = scryfallClient.GetCardById(new Guid("c8b4d10d-ecf4-4dad-89d3-12333b522358")).Result;
        }
    }
}
