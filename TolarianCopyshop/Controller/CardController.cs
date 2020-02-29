using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.SfCardInfo;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Controller.Mappers;

namespace Tolarian.Copyshop.Controller
{
    public class CardController : TolarianControllerBase
    {
        private readonly ICardDataRequester _requester;

        public CardController(ICardDataRequester requester)
        {
            _requester = requester;
        }

        /// <summary>
        /// Gets the information for one Card by ID. This returns a List because the target card may be multifaced.
        /// </summary>
        public List<IFullCard> GetCardById(Guid id)
        {
            List<IFullCard> response = null;
            try
            {
                SfCard card = _requester.GetCardById(id);

                response = CardMapper.MapToCardDto(card);
            }
            catch (HttpException ex)
            {
                this.ErrorMessage = BuildErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                this.ErrorMessage = BuildErrorMessage(ex);
            }

            return response;
        }

        public List<CardSearchResult> GetSearchResults(string query, int maxCountOfItems, out int maxResults)
        {
            maxResults = 0;
            var response = new List<CardSearchResult>();

            try
            {
                response = CardMapper.MapToSearchResultDto(_requester.GetCardsBySearchQuery(query, maxCountOfItems, out maxResults));
            }
            catch (HttpException ex)
            {
                this.ErrorMessage = BuildErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                this.ErrorMessage = BuildErrorMessage(ex);
            }

            return response;
        }

        public List<IFullCard> GetCardsByNameList(string importString)
        {
            List<IFullCard> response = new List<IFullCard>();

            try
            {
                List<string> lines = importString.Split(
                                new[] { "\r\n", "\r", "\n" },
                                StringSplitOptions.None).ToList();

                return CardMapper.MapToCardDto(_requester.GetCardsByNameList(lines));
            }
            catch (HttpException ex)
            {
                this.ErrorMessage = BuildErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                this.ErrorMessage = BuildErrorMessage(ex);
            }

            return response;
        }
    }
}
