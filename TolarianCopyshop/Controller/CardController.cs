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
        private string errorMessage;

        public CardController(ICardDataRequester requester, IMapper mapper)
        {
            _requester = requester;
            _mapper = mapper;
        }

        public string ErrorMessage
        {
            get
            {
                string returnValue = this.errorMessage;
                this.errorMessage = string.Empty;
                return returnValue;
            }
            set => this.errorMessage = value;
        }


        /// <summary>
        /// Gets the information for one Card by ID. This returns a List because the target card may be multifaced.
        /// </summary>
        public List<FullCardResponse> GetCardById(Guid id)
        {
            List<FullCardResponse> response = null;
            try
            {
                SfCard card = _requester.GetCardById(id);

                response = MapCardToFullCardResponse(card);
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

        public List<CardNameResponse> GetCardNamesAndIdsBySearchQuery(string query, int maxCountOfItems)
        {
            var response = new List<CardNameResponse>();

            try
            {
                response = _mapper.Map<List<CardNameResponse>>(_requester.GetCardsBySearchQuery(query, maxCountOfItems));
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

        private static string BuildErrorMessage(Exception ex)
            => ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");

        public List<FullCardResponse> GetCardsByNameList(string importString)
        {
            List<FullCardResponse> response = new List<FullCardResponse>();

            try
            {
                List<string> lines = importString.Split(
                                new[] { "\r\n", "\r", "\n" },
                                StringSplitOptions.None).ToList();
                return _requester.GetCardsByNameList(lines).SelectMany(c => MapCardToFullCardResponse(c)).ToList();
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

        private List<FullCardResponse> MapCardToFullCardResponse(SfCard card)
        {
            List<FullCardResponse> response;
            if (IsDoubleFacedCard(card))
                response = _mapper.Map<List<FullCardResponse>>(card);
            else
                response = new List<FullCardResponse> { _mapper.Map<FullCardResponse>(card) };
            return response;
        }

        private bool IsDoubleFacedCard(SfCard card)
        {
            return card.CardFaces != null && card.ImageUris == null;
        }

        public List<FullCardResponse> OpenFrom(string fileName)
            => throw new NotImplementedException();

        public bool SaveTo(string fileName, List<FullCardResponse> deckCards)
            => throw new NotImplementedException();
    }
}
