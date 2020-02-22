using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models;

namespace Tolarian.Copyshop.Business
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

            List<List<string>> requestLists = ChunkListBySize<string>(resolvedNames, scryfallApiReturnCountMaximum);

            List<SfCard> result = requestLists.SelectMany(r => _gateway.GetCardsByNameList(r).Data.ToList()).ToList();
            return result;
        }

        public List<SfCard> GetCardsBySearchQuery(string searchQuery, int maxCountOfItems, out int maxResults)
        {
            const int minimumQueryLength = 3;
            if (searchQuery.Length < minimumQueryLength)
            {
                maxResults = 0;
                return new List<SfCard>();
            }

            List<SfCard> result = _gateway.GetCardsByQuery(searchQuery).Data.ToList();
            maxResults = result.Count;
            return TruncateListToMaxSize(maxCountOfItems, result);
        }

        private List<SfCard> TruncateListToMaxSize(int maxCountOfItems, List<SfCard> targetList)
        {
            int firstInvalidIndex = maxCountOfItems;

            if (targetList.Count > maxCountOfItems)
                targetList.RemoveRange(firstInvalidIndex, targetList.Count - firstInvalidIndex);

            return targetList;
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
