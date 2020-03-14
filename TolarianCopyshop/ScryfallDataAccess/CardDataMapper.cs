using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tolarian.Copyshop.ScryfallDataAccess
{
    public class CardDataMapper : ICardDataGateway
    {
        IScryfallApi _service;

        public CardDataMapper()
        {
            _service = RestService.For<IScryfallApi>("https://api.scryfall.com");
        }

        public SfCard GetCardByPrintId(Guid printId)
        {
            ApiResponse<SfCard> response = _service.GetCardByPrintId(printId).Result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content;
                case HttpStatusCode.NotFound:
                    return SfCard.GetEmpty();
                default:
                    HandleUnexpectedStatusCodeForResponse(response);
                    break;
            }

            return null;
        }

        public SfPaginatedCardList GetPrintsOfCard(Guid oracleId)
        {
            ApiResponse<SfPaginatedCardList> response = _service.GetPrintsBySearchQuery(oracleId).Result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content;
                case HttpStatusCode.NotFound:
                    return SfPaginatedCardList.GetEmpty();
                default:
                    HandleUnexpectedStatusCodeForResponse(response);
                    break;
            }

            return null;
        }

        public SfPaginatedCardList GetCardsByNameList(List<string> cardNames)
        {
            SfIdentifierContainer container = new SfIdentifierContainer { Identifiers = cardNames.Select(n => new SfIdentifier { Name = n}).ToList() };

            ApiResponse<SfPaginatedCardList> response = _service.GetCardsByCollection(container).Result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content;
                case HttpStatusCode.NotFound:
                    return SfPaginatedCardList.GetEmpty();
                default:
                    HandleUnexpectedStatusCodeForResponse(response);
                    break;
            }

            return null;
        }

        public SfPaginatedCardList GetCardsBySearchQuery(string query)
        {
            ApiResponse<SfPaginatedCardList> response = _service.GetCardsBySearchQuery(query).Result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content;
                case HttpStatusCode.NotFound:
                    return SfPaginatedCardList.GetEmpty();
                default:
                    HandleUnexpectedStatusCodeForResponse(response);
                    break;
            }

            return null;
        }

        public void HandleUnexpectedStatusCodeForResponse<T>(ApiResponse<T> response)
        {
            throw new HttpException($"Unexpected Status Code of Http Request. Status {response.StatusCode}, Error: {response.Error}");
        }
    }
}
