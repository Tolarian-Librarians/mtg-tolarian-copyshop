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
        public List<IFullCard> GetCardByPrintId(Guid printId)
        {
            List<IFullCard> response = null;
            try
            {
                SfCard card = _requester.GetCardByPrintId(printId);

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

        public CardSearchResponse GetSearchResults(string query, int maxCountOfItems)
        {
            CardSearchResponse response = new CardSearchResponse();
            try
            {
                (List<SfCard> Cards, string amountFound) businessResponse = _requester.GetCardsBySearchQuery(query, maxCountOfItems);
                response = CardMapper.MapToSearchResultDto(businessResponse.Cards, businessResponse.amountFound);
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
        public List<CardArtworkResponse> GetArtworksOfCard(Guid cardId)
        {
            var response = new List<CardArtworkResponse>();
            try
            {
                response = CardMapper.MapToArtworkDto(_requester.GetPrintsOfCard(cardId));
                response = response.OrderBy(card => card.SetName).ToList();
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

        public List<IFullCard> GetCardsByImportString(string importString)
        {
            List<IFullCard> response = new List<IFullCard>();

            try
            {
                List<string> lines = importString.Split(
                                new[] { "\r\n", "\r", "\n" },
                                StringSplitOptions.None).ToList();

                return CardMapper.MapToCardDto(_requester.GetCardsByImport(lines).Cards);
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
