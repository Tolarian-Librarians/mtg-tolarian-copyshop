using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.SfCardInfo;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.Mappers;
using Tolarian.Copyshop.Controller.ResponseObjects;

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
            FullCardResponse response = new();

            try
            {
                SfCard card = _requester.GetCardByPrintId(printId);
                response.Card = CardMapper.MapToCardDto(card);
            }
            catch (WebException ex)
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
            CardSearchResponse response = new();

            try
            {
                (List<SfCard> Cards, string amountFound) = _requester.GetCardsBySearchQuery(query, maxCountOfItems);
                response = CardMapper.MapToSearchResultDto(Cards, amountFound);
            }
            catch (WebException ex)
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
            CardArtworkResponse response = new();

            try
            {
                List<ArtworkCard> artworks = CardMapper.MapToArtworkDto(_requester.GetPrintsOfCard(cardId));
                artworks = artworks.OrderByDescending(card => card.ReleaseDate).ToList();
                response.Artworks = artworks;
            }
            catch (WebException ex)
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
            CardImportResponse response = new();

            try
            {
                List<string> lines = CardMapper.PrepareImportStringForBusiness(importString);
                (List<SfCard> Cards, string NotFound) = _importInteractor.GetCardsForImport(lines);
                response.Cards = CardMapper.MapToCardDto(Cards);
                response.NotFound = NotFound;
            }
            catch (WebException ex)
            {
                SetErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                SetErrorMessage(ex);
            }

            return response;
        }

        public CardImportResponse GetCardsFromUri(string deckUrl)
        {
            CardImportResponse response = new();

            try
            {
                Uri uri = new(deckUrl);

                if (!uri.Host.Contains("tappedout", StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new NotSupportedException("Currently, we only support importing decks from tappedout.net.");
                }

                (List<SfCard> Cards, string NotFound) = _importInteractor.ImportFromTappedOut(uri);

                response.Cards = CardMapper.MapToCardDto(Cards);
                response.NotFound = NotFound;
            }
            catch (WebException ex)
            {
                SetErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                SetErrorMessage(ex);
            }
            catch (NotSupportedException ex)
            {
                SetErrorMessage(ex);
            }
            catch (UriFormatException ex)
            {
                SetErrorMessage(ex);
            }

            return response;
        }

        public CardSearchResponse GetTokenSearchResults(string query)
        {
            CardSearchResponse response = new();

            try
            {
                (List<SfCard> Cards, string amountFound) = _requester.GetTokensByQuery(query);
                response = CardMapper.MapToSearchResultDto(Cards, amountFound);
            }
            catch (WebException ex)
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
            AddTokensToDeckResponse response = new();

            try
            {
                if (overwriteTokens)
                {
                    deckCards.RemoveAll(c => c.CardFaces.Any(cf => cf.PrimaryCardType == ResponseObjects.Enums.CardType.Token));
                }

                List<SfCard> businessResponse = _requester.GetCardsByIds(CardMapper.GetTokenGuidsOfDeck(deckCards));
                deckCards.AddRange(CardMapper.MapToCardDto(businessResponse));
                response.Deck = deckCards;
            }
            catch (WebException ex)
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