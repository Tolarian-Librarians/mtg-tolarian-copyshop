using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Tolarian.Copyshop.Business.DbRequestModels;
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

        public SfCardCollection GetCardCollectionByIdentifiers(List<GetCardCollectionRequest> request)
        {
            if (request.Count == 0)
            {
                return SfCardCollection.GetEmpty();
            }

            SfIdentifierContainer container = new SfIdentifierContainer { Identifiers = request.Select(
                r => new SfIdentifier { Name = r.Name, SetCode = r.SetCode}).ToList()
            };

            ApiResponse<SfCardCollection> response = _service.GetCardsByCollection(container).Result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content;
                case HttpStatusCode.NotFound:
                    return SfCardCollection.GetEmpty();
                default:
                    HandleUnexpectedStatusCodeForResponse(response);
                    break;
            }

            return null;
        }

        public SfCatalog GetCardNamesByAutoCompleteQuery(string query)
        {
            ApiResponse<SfCatalog> response = _service.GetCardsByAutoCompleteQuery(query).Result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content;
                case HttpStatusCode.NotFound:
                    return SfCatalog.GetEmpty();
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
