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
        public FullCardResponse GetCardByPrintId(Guid printId)
        {
            FullCardResponse response = new FullCardResponse();

            try
            {
                SfCard card = _requester.GetCardByPrintId(printId);
                response.Cards = CardMapper.MapToCardDto(card);
            }
            catch (HttpException ex)
            {
                SetErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                SetErrorMessage(ex);
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
                SetErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                SetErrorMessage(ex);
            }

            return response;
        }
        public CardArtworkResponse GetArtworksOfCard(Guid cardId)
        {
            CardArtworkResponse response = new CardArtworkResponse();
                
            try
            {
                var artworks = CardMapper.MapToArtworkDto(_requester.GetPrintsOfCard(cardId));
                artworks = artworks.OrderBy(card => card.SetName).ToList();
                response.Artworks = artworks;
            }
            catch (HttpException ex)
            {
                SetErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                SetErrorMessage(ex);
            }

            return response;
        }

        public FullCardResponse GetCardsByImportString(string importString)
        {
            FullCardResponse response = new FullCardResponse();

            try
            {
                List<string> lines = importString.Split(
                                new[] { "\r\n", "\r", "\n" },
                                StringSplitOptions.None).ToList();

                (List<SfCard> Cards, string NotFound) = _requester.GetCardsByImport(lines);
                response.Cards = CardMapper.MapToCardDto(Cards);
                response.NotFound = NotFound;
            }
            catch (HttpException ex)
            {
                SetErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                SetErrorMessage(ex);
            }

            return response;
        }
    }
}
