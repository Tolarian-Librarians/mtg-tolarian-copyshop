using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

using Refit;

using Tolarian.Copyshop.Business.DbRequestModels;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tolarian.Copyshop.ScryfallDataAccess
{
    public class CardDataMapper : DataMapperBase, ICardDataGateway
    {
        //Scryfall will return a maximum of 75 Cards per request
        private const int _scryfallApiReturnCountMaximum = 75;

        private readonly IScryfallApi _service;

        public CardDataMapper()
        {
            _service = RestService.For<IScryfallApi>(_httpClient, new RefitSettings() { ContentSerializer = new NewtonsoftJsonContentSerializer() });
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

        public List<SfCard> GetPrintsOfCard(Guid oracleId)
        {
            ApiResponse<SfPaginatedCardList> firstPageResponse = _service.GetPrintsBySearchQuery(oracleId, 1).Result;

            switch (firstPageResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    SfPaginatedCardList currentPage = firstPageResponse.Content;
                    var result = currentPage.Data.ToList();

                    if (currentPage.MorePagesAvailable)
                        result.AddRange(ReadNextPagesOf(currentPage, oracleId));

                    return result;
                case HttpStatusCode.NotFound:
                    return new List<SfCard>();
                default:
                    HandleUnexpectedStatusCodeForResponse(firstPageResponse);
                    break;
            }

            return new List<SfCard>();
        }

        private List<SfCard> ReadNextPagesOf(SfPaginatedCardList firstPage, Guid oracleId)
        {
            SfPaginatedCardList currentPage = firstPage;
            int currentPageNumber = 1;
            var result = new List<SfCard>();

            while (currentPage.MorePagesAvailable)
            {
                currentPageNumber++;
                var nextPageResponse = _service.GetPrintsBySearchQuery(oracleId, currentPageNumber)
                                               .Result;

                switch (nextPageResponse.StatusCode)
                {
                    case HttpStatusCode.OK:
                        currentPage = nextPageResponse.Content;
                        result.AddRange(currentPage.Data);
                        break;
                    default:
                        HandleUnexpectedStatusCodeForResponse(nextPageResponse);
                        break;
                }
            }

            return result;
        }

        public SfCardCollection GetCardCollectionByIdentifiers(List<GetCardCollectionRequest> cardNames)
        {
            if (cardNames.Count == 0)
            {
                return SfCardCollection.GetEmpty();
            }

            //List needs to be chunked into lists of max 75 items because SF will return a maximum of 75 cards
            List<List<GetCardCollectionRequest>> chunkedRequests = ChunkListBySize(cardNames, _scryfallApiReturnCountMaximum);

            var containers = chunkedRequests.Select(cr => new SfIdentifierContainer
            {
                Identifiers = cr.Select(r => new SfIdentifier
                {
                    // For splitcards is only one cardname required
                    Name = r.Name.Split("//").First(),
                    SetCode = r.SetCode,
                    Id = r.Id,
                }).ToList()
            });

            var response = containers.Select(c => IssueGetCardCollectionRequest(c)).ToList();
            SfCardCollection result = MergeCardCollections(response);

            return result;
        }

        private SfCardCollection IssueGetCardCollectionRequest(SfIdentifierContainer container)
        {
            ApiResponse<SfCardCollection> response = _service.GetCardsByCollection(container).Result;

            // Give Scryfall time to breath
            Thread.Sleep(100);

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

        private static SfCardCollection MergeCardCollections(IEnumerable<SfCardCollection> response)
        {
            SfCardCollection result = new()
            {
                Data = response.SelectMany(r => r.Data).ToArray(),
                NotFound = response.SelectMany(r => r.NotFound).ToArray()
            };
            return result;
        }

        private static List<List<T>> ChunkListBySize<T>(List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
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

        public List<SfCard> GetTokensByQuery(string query)
        {
            ApiResponse<SfPaginatedCardList> response = _service.GetTokensByQuery(query).Result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content.Data.ToList();
                case HttpStatusCode.NotFound:
                    return new List<SfCard>();
                default:
                    HandleUnexpectedStatusCodeForResponse(response);
                    break;
            }

            return new List<SfCard>();
        }
    }
}