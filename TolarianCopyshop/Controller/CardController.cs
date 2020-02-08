using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business;
using Tolarian.Copyshop.Controller.ResponseObjects;

namespace Tolarian.Copyshop.Controller
{
    public class CardController
    {
        private readonly ICardDataPresenter _presenter;
        private readonly ICardDataRequester _requester;
        //private readonly IMapper _requester;

        public CardController(ICardDataPresenter presenter, ICardDataRequester requester)
        {
            _presenter = presenter;
            _requester = requester;
        }

        public GetCardByIdResponse GetCardById(Guid id)
        {
            GetCardByIdResponse response = new GetCardByIdResponse
            {

            };

            return response;
        }

        public List<CardNamesAndIdsBySearchQuery> GetCardNamesAndIdsBySearchQuery(string query, int maxCountOfItems)
        {
            var result = new List<CardNamesAndIdsBySearchQuery>();

            result = _requester.GetCardsBySearchQuery(query, maxCountOfItems).Select(c => new CardNamesAndIdsBySearchQuery { Name = c.Name, Id = c.Id }).ToList();

            return result;
        }
    }
}
