using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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

        public FullCardResponse GetCardById(Guid id, out string message)
        {
            message = string.Empty;
            FullCardResponse response = null;
            try
            {
                SfCard card = _requester.GetCardById(id);
                response = _mapper.Map<FullCardResponse>(card);
            }
            catch (HttpException ex)
            {
                message = ex.Message + Environment.NewLine + ex.InnerException != null ? ex.InnerException.Message : "";
            }
            catch (AggregateException ex)
            {
                message = ex.Message + Environment.NewLine + ex.InnerException != null ? ex.InnerException.Message : "";
            }

            return response;
        }

        public List<CardNameResponse> GetCardNamesAndIdsBySearchQuery(string query, int maxCountOfItems, out string message)
        {
            message = string.Empty;
            var result = new List<CardNameResponse>();

            try
            {
                result = _requester.GetCardsBySearchQuery(query, maxCountOfItems).Select(c => new CardNameResponse { Name = c.Name, Id = c.Id }).ToList();
            }
            catch (HttpException ex)
            {
                message = ex.Message + Environment.NewLine + ex.InnerException != null ? ex.InnerException.Message : "";
            }
            catch (AggregateException ex)
            {
                message = ex.Message + Environment.NewLine + ex.InnerException != null ? ex.InnerException.Message : "";
            }

            return result;
        }
    }
}
