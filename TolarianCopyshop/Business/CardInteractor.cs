using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public List<SfCard> GetCardsBySearchQuery(string searchQuery, int maxCountOfItems)
        {
            const int minimumQueryLength = 3;
            if (searchQuery.Length < minimumQueryLength)
            {
                return new List<SfCard>();
            }

            List<SfCard> result = _gateway.GetCardsByQuery(searchQuery).Data.ToList();

            result = TruncateListToMaxSize(maxCountOfItems, result);

            return result;
        }

        private List<SfCard> TruncateListToMaxSize(int maxCountOfItems, List<SfCard> targetList)
        {
            int lastIndex = maxCountOfItems - 1;

            if (targetList.Count > maxCountOfItems)
                targetList.RemoveRange(lastIndex, targetList.Count - lastIndex);

            return targetList;
        }
    }
}
