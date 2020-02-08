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
    public class DataMapper : ICardDataGateway
    {
        public SfPaginatedCardList GetCardsByQuery(string query)
        {
            var service = RestService.For<IScryfallApi>("");
            return service.GetCardsBySearchQuery(query).Result;
        }
    }
}
