using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business;

namespace Tolarian.Copyshop.Controller
{
    public class CardsController
    {
        ICardDataRequester _requester;
        ICardDataPresenter _presenter;

        public CardsController(ICardDataRequester requester, ICardDataPresenter presenter)
        {
            _requester = requester;
            _presenter = presenter;
        }

        public List<CardSearchResponse> GetCardsForSearch(string query)
        {
            return _requester.GetCardsBySearchQuery(query).Select(c => c.Name).ToArray();
        }
    }
}
