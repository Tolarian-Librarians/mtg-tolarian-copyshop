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

        /// <summary>
        /// Gets the information for one Card by ID. This returns a List because the target card may be multifaced.
        /// </summary>
        public List<FullCardResponse> GetCardById(Guid id, out string message)
        {
            message = string.Empty;
            List<FullCardResponse> response = null;
            try
            {
                SfCard card = _requester.GetCardById(id);

                if (IsDoubleFacedCard(card))
                    response = _mapper.Map<List<FullCardResponse>>(card);
                else
                    response = new List<FullCardResponse> { _mapper.Map<FullCardResponse>(card) };
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

        private bool IsDoubleFacedCard(SfCard card)
        {
            return card.CardFaces != null && card.ImageUris == null;
        }

        public List<CardNameResponse> GetCardNamesAndIdsBySearchQuery(string query, int maxCountOfItems, out string message)
        {
            message = string.Empty;
            var result = new List<CardNameResponse>();

            try
            {
                result = _mapper.Map<List<CardNameResponse>>(_requester.GetCardsBySearchQuery(query, maxCountOfItems));
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
