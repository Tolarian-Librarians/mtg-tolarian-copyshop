using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.SfCardInfo;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Controller.Mappers;
using Tolarian.Copyshop.Controller.Interfaces;

namespace Tolarian.Copyshop.Controller
{
    public class CardController : TolarianControllerBase
    {
        private readonly ICardDataRequester _requester;
        private readonly IDeckImportInteractor _importInteractor;

        public CardController(ICardDataRequester requester, IDeckImportInteractor importInteractor)
        {
            _requester = requester;
            _importInteractor = importInteractor;
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
                response.Card = CardMapper.MapToCardDto(card);
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
                artworks = artworks.OrderByDescending(card => card.ReleaseDate).ToList();
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

        public CardImportResponse GetCardsByImportString(string importString)
        {
            CardImportResponse response = new CardImportResponse();

            try
            {
                List<string> lines = CardMapper.PrepareImportStringForBusiness(importString);
                (List<SfCard> Cards, string NotFound) = _importInteractor.GetCardsForImport(lines);
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
        
        public CardSearchResponse GetTokenSearchResults(string query)
        {
            CardSearchResponse response = new CardSearchResponse();

            try
            {
                (List<SfCard> Cards, string amountFound) businessResponse = _requester.GetTokensByQuery(query);
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

        public AddTokensToDeckResponse AddTokensToDeck(List<IFullCard> deckCards, bool overwriteTokens)
        {
            AddTokensToDeckResponse response = new AddTokensToDeckResponse();

            try
            {
                if(overwriteTokens)
                {
                    deckCards.RemoveAll(c => c.CardFaces.Any(cf => cf.CardType == ResponseObjects.Enums.CardType.Token));
                }

                var businessResponse = _requester.GetTokensForDeck(CardMapper.GetTokenGuidsOfDeck(deckCards));
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
