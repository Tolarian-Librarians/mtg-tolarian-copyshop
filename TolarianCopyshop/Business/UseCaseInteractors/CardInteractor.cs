using System;
using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.DbRequestModels;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

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

        public List<SfCard> GetPrintsOfCard(Guid cardId)
        {
            return _gateway.GetPrintsOfCard(cardId);
        }

        public (List<SfCard>, string) GetTokensByQuery(string searchQuery)
        {
            var result = _gateway.GetTokensByQuery(searchQuery);
            return (result, result.Count.ToString());
        }

        public List<SfCard> GetTokensForDeck(List<Guid> tokenGuids)
        {
            var result = _gateway.GetCardCollectionByIdentifiers(tokenGuids.Select(tg => new GetCardCollectionRequest { Id = tg }).ToList()).Data
                .ToList();
            return result;
        }
    }
}
