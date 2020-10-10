using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            this._requester = requester;
            this._importInteractor = importInteractor;
        }

        /// <summary>
        /// Gets the information for one Card by ID. This returns a List because the target card may be multifaced.
        /// </summary>
        public FullCardResponse GetCardByPrintId(Guid printId)
        {
            FullCardResponse response = new FullCardResponse();

            try
            {
                SfCard card = this._requester.GetCardByPrintId(printId);
                response.Card = CardMapper.MapToCardDto(card);
            }
            catch (HttpException ex)
            {
                this.SetErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                this.SetErrorMessage(ex);
            }

            return response;
        }

        public CardSearchResponse GetSearchResults(string query, int maxCountOfItems)
        {
            CardSearchResponse response = new CardSearchResponse();

            try
            {
                (List<SfCard> Cards, string amountFound) businessResponse = this._requester.GetCardsBySearchQuery(query, maxCountOfItems);
                response = CardMapper.MapToSearchResultDto(businessResponse.Cards, businessResponse.amountFound);
            }
            catch (HttpException ex)
            {
                this.SetErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                this.SetErrorMessage(ex);
            }

            return response;
        }

        public CardArtworkResponse GetArtworksOfCard(Guid cardId)
        {
            CardArtworkResponse response = new CardArtworkResponse();

            try
            {
                List<ArtworkCard> artworks = CardMapper.MapToArtworkDto(this._requester.GetPrintsOfCard(cardId));
                artworks = artworks.OrderByDescending(card => card.ReleaseDate).ToList();
                response.Artworks = artworks;
            }
            catch (HttpException ex)
            {
                this.SetErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                this.SetErrorMessage(ex);
            }

            return response;
        }

        public CardImportResponse GetCardsByImportString(string importString)
        {
            CardImportResponse response = new CardImportResponse();

            try
            {
                List<string> lines = CardMapper.PrepareImportStringForBusiness(importString);
                (List<SfCard> Cards, string NotFound) = this._importInteractor.GetCardsForImport(lines);
                response.Cards = CardMapper.MapToCardDto(Cards);
                response.NotFound = NotFound;
            }
            catch (HttpException ex)
            {
                this.SetErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                this.SetErrorMessage(ex);
            }

            return response;
        }

        public CardImportResponse GetCardsFromUri(string deckUrl)
        {
            CardImportResponse response = new CardImportResponse();

            try
            {
                Uri uri = new Uri(deckUrl);

                if (uri.Host.IndexOf("tappedout", StringComparison.InvariantCultureIgnoreCase) == -1)
                {
                    throw new NotSupportedException("Currently, we only support importing decks from tappedout.net.");
                }

                (List<SfCard> Cards, string NotFound) = this._importInteractor.ImportFromTappedOut(uri);

                response.Cards = CardMapper.MapToCardDto(Cards);
                response.NotFound = NotFound;
            }
            catch (HttpException ex)
            {
                this.SetErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                this.SetErrorMessage(ex);
            }
            catch (NotSupportedException ex)
            {
                this.SetErrorMessage(ex);
            }

            return response;
        }

        public CardSearchResponse GetTokenSearchResults(string query)
        {
            CardSearchResponse response = new CardSearchResponse();

            try
            {
                (List<SfCard> Cards, string amountFound) businessResponse = this._requester.GetTokensByQuery(query);
                response = CardMapper.MapToSearchResultDto(businessResponse.Cards, businessResponse.amountFound);
            }
            catch (HttpException ex)
            {
                this.SetErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                this.SetErrorMessage(ex);
            }

            return response;
        }

        public AddTokensToDeckResponse AddTokensToDeck(List<IFullCard> deckCards, bool overwriteTokens)
        {
            AddTokensToDeckResponse response = new AddTokensToDeckResponse();

            try
            {
                if (overwriteTokens)
                {
                    deckCards.RemoveAll(c => c.CardFaces.Any(cf => cf.PrimaryCardType == ResponseObjects.Enums.CardType.Token));
                }

                List<SfCard> businessResponse = this._requester.GetCardsByIds(CardMapper.GetTokenGuidsOfDeck(deckCards));
                deckCards.AddRange(CardMapper.MapToCardDto(businessResponse));
                response.Deck = deckCards;
            }
            catch (HttpException ex)
            {
                this.SetErrorMessage(ex);
            }
            catch (AggregateException ex)
            {
                this.SetErrorMessage(ex);
            }

            return response;
        }
    }
}
