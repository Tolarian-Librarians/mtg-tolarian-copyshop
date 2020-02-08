using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business;
using Tolarian.Copyshop.Business.Models;

namespace Tolarian.Copyshop.ScryfallDataAccess
{
    public class CardDataMapper : ICardDataGateway
    {
        IScryfallApi _service;

        public CardDataMapper()
        {
            _service = RestService.For<IScryfallApi>("https://api.scryfall.com");
        }

        public SfCard GetCardById(Guid id)
        {
            return _service.GetCardById(id).Result;
        }

        public SfPaginatedCardList GetCardsByQuery(string query)
        {
            return _service.GetCardsBySearchQuery(query).Result;
        }
    }
}
