using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business.Models;

namespace Tolarian.Copyshop.Business
{
    public class CardService : ICardDataRequester
    {
        ICardDataGateway dataGateway;

        public CardService(ICardDataGateway gateway)
        {
            dataGateway = gateway;
        }

        public List<SfCard> GetCardsBySearchQuery(string searchQuery)
        {
            return dataGateway.GetCardsByQuery(searchQuery).Data.ToList();
        }
    }
}
