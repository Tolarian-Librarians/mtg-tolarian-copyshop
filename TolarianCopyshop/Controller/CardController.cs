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
        public List<IFullCard> GetCardById(Guid id, string setCode = "")
        {
            List<IFullCard> response = null;
            try
            {
                SfCard card;
                if (string.IsNullOrWhiteSpace(setCode))
                {
                    card = _requester.GetCardById(id);
                }
                else
                {
                    card = _requester.GetCardById(id);
                }

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
            CardSearchResponse response = null;
            try
            {
                (List<SfCard>, int) businessResponse = _requester.GetCardsBySearchQuery(query, maxCountOfItems);
                response = CardMapper.MapToSearchResultDto(businessResponse.Item1, businessResponse.Item2);
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
        public List<CardArtworkResponse> GetArtworksOfCard(Guid id)
        {
            var response = new List<CardArtworkResponse>();
            try
            {
                response = new List<CardArtworkResponse>
                {
                    new CardArtworkResponse{ Image = new Uri("https://img.scryfall.com/cards/normal/front/1/8/18a44b2a-afda-452d-9d84-f67bc97620b0.jpg?1581630450"), SetCode = "KLD", SetName = "Kaladesh" },
                    new CardArtworkResponse{ Image = new Uri("https://img.scryfall.com/cards/normal/front/5/4/54aabd14-7fc7-4ba0-9fde-0aac3edb077d.jpg?1581630519"), SetCode = "ELD", SetName = "Throne of Eldraine" },
                    new CardArtworkResponse{ Image = new Uri("https://img.scryfall.com/cards/normal/front/d/a/da57b900-5f1f-42a7-8182-b5ff22e8d65f.jpg?1581630453"), SetCode = "WAR", SetName = "War of the Spark" },
                    new CardArtworkResponse{ Image = new Uri("https://img.scryfall.com/cards/normal/front/6/5/6579b2ae-bf13-432c-8d41-1c89c8f8568e.jpg?1581630524"), SetCode = "TBD", SetName = "Theros beyond Death" },
                };
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
