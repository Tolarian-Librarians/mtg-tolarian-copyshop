using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface ICardDataRequester
    {
        (List<SfCard> Cards, string AmountFound) GetCardsBySearchQuery(string searchQuery, int maxCountOfItems);
        SfCard GetCardByPrintId(Guid printId);
        List<SfCard> GetPrintsOfCard(Guid cardId);
    }
}
