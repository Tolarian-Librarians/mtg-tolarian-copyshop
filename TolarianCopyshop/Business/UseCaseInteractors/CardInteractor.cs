using System;
using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.DbRequestModels;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.SfCardInfo;
using Tolarian.Copyshop.Business.Utility;

namespace Tolarian.Copyshop.Business.UseCaseInteractors
{
    public class CardInteractor : ICardDataRequester
    {
        private readonly ICardDataGateway _gateway;

        public CardInteractor(ICardDataGateway gateway)
        {
            _gateway = gateway;
        }

        public SfCard GetCardByPrintId(Guid printId)
        {
            SfCard result = _gateway.GetCardByPrintId(printId);
            return result;
        }

        public (List<SfCard>, string) GetCardsByImport(List<string> importLines)
        {
            //Scryfall will return a maximum of 75 Cards per request
            int scryfallApiReturnCountMaximum = 75;

            List<GetCardCollectionRequest> resolvedRequests = DeckImportHelper.ResolveCardNamesFromImportString(importLines);

            List<List<GetCardCollectionRequest>> requestLists = ChunkListBySize(resolvedRequests, scryfallApiReturnCountMaximum);

            var result = requestLists.Select(r => _gateway.GetCardCollectionByIdentifiers(r));

            string notFound = string.Join(Environment.NewLine
                , result.SelectMany(cc => cc.NotFound.Select(nf => string.Join(" ", nf.Name, nf.SetCode))).ToArray());

            return (result.SelectMany(cc => cc.Data).ToList(), notFound);
        }

        public (List<SfCard>, string) GetCardsBySearchQuery(string searchQuery, int maxCountOfItems)
        {
            const int minimumQueryLength = 3;
            if (searchQuery.Length < minimumQueryLength || maxCountOfItems <= 0)
            {
                return (new List<SfCard>(), "0");
            }

            List<string> cardNames = _gateway.GetCardNamesByAutoCompleteQuery(searchQuery).Data.ToList();

            List<SfCard> result = _gateway.GetCardCollectionByIdentifiers(
                cardNames.Select(name => new GetCardCollectionRequest { Name = name}).ToList()
                ).Data.ToList();

            return (TruncateListToMaxSize(maxCountOfItems, result), cardNames.Count >= 20 ? "20+" : cardNames.Count.ToString());
        }

        private List<SfCard> TruncateListToMaxSize(int maxCountOfItems, List<SfCard> targetList)
        {
            List<SfCard> resultList = new List<SfCard>(targetList);
            int firstInvalidIndex = maxCountOfItems;

            if (resultList.Count > maxCountOfItems)
                resultList.RemoveRange(firstInvalidIndex, resultList.Count - firstInvalidIndex);

            return resultList;
        }

        private List<List<T>> ChunkListBySize<T>(List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public List<SfCard> GetPrintsOfCard(Guid cardId)
        {
            return _gateway.GetPrintsOfCard(cardId).Data.ToList();
        }
    }
}
