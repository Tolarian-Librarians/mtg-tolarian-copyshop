using System;
using System.Collections.Generic;
using System.Linq;
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

        public SfCard GetCardById(Guid id)
        {
            SfCard result = _gateway.GetCardById(id);
            return result;
        }

        public List<SfCard> GetCardsByNameList(List<string> cardNames)
        {
            //Scryfall will return a maximum of 75 Cards per request
            int scryfallApiReturnCountMaximum = 75;

            List<string> resolvedNames = DeckImportHelper.ResolveCardNamesFromList(cardNames);

            List<List<string>> requestLists = ChunkListBySize(resolvedNames, scryfallApiReturnCountMaximum);

            List<SfCard> result = requestLists.SelectMany(r => _gateway.GetCardsByNameList(r).Data.ToList()).ToList();
            return result;
        }

        public (List<SfCard>, int) GetCardsBySearchQuery(string searchQuery, int maxCountOfItems)
        {
            const int minimumQueryLength = 3;
            if (searchQuery.Length < minimumQueryLength)
            {
                return (new List<SfCard>(), 0);
            }

            List<SfCard> result = _gateway.GetCardsByQuery(searchQuery).Data.ToList();
            return (TruncateListToMaxSize(maxCountOfItems, result), result.Count);
        }

        private List<SfCard> TruncateListToMaxSize(int maxCountOfItems, List<SfCard> targetList)
        {
            List<SfCard> resultList = new List<SfCard>(targetList);
            int firstInvalidIndex = maxCountOfItems;

            if (resultList.Count > maxCountOfItems)
                resultList.RemoveRange(firstInvalidIndex, resultList.Count - firstInvalidIndex);

            return resultList;
        }

        public static List<List<T>> ChunkListBySize<T>(List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
