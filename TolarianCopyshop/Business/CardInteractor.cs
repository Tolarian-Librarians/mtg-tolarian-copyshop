using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business.Models;

namespace Tolarian.Copyshop.Business
{
    public class CardInteractor : ICardDataRequester
    {
        private readonly ICardDataGateway _gateway;

        public CardInteractor(ICardDataGateway gateway)
        {
            _gateway = gateway;
        }

        public List<SfCard> GetCardsBySearchQuery(string searchQuery)
        {
            throw new NotImplementedException();
        }
    }
}
