using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface ICardDataRequester
    {
        (List<SfCard>, int) GetCardsBySearchQuery(string searchQuery, int maxCountOfItems);
        SfCard GetCardByPrintId(Guid printId);
        List<SfCard> GetCardsByNameList(List<string> cardNames);
        List<SfCard> GetPrintsOfCard(Guid cardId);
    }
}
