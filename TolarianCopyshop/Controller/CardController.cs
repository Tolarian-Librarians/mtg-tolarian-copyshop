using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business;
using Tolarian.Copyshop.Business.Models;
using Tolarian.Copyshop.Controller.ResponseObjects;

namespace Tolarian.Copyshop.Controller
{
    public class CardController
    {
        private readonly ICardDataRequester _requester;
        private readonly IMapper _mapper;

        public CardController(ICardDataRequester requester, IMapper mapper)
        {
            _requester = requester;
            _mapper = mapper;
        }

        public FullCardResponse GetCardById(Guid id)
        {
            SfCard card = _requester.GetCardById(id);

            FullCardResponse response = _mapper.Map<FullCardResponse>(card);

            return response;
        }

        public List<CardNameResponse> GetCardNamesAndIdsBySearchQuery(string query, int maxCountOfItems)
        {
            var result = new List<CardNameResponse>();

            result = _requester.GetCardsBySearchQuery(query, maxCountOfItems).Select(c => new CardNameResponse { Name = c.Name, Id = c.Id }).ToList();

            return result;
        }
    }
}
