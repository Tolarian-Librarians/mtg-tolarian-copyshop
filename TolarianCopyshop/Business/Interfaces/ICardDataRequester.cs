using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface ICardDataRequester
    {
        (List<SfCard>, int) GetCardsBySearchQuery(string searchQuery, int maxCountOfItems);
        SfCard GetCardById(Guid id);
        List<SfCard> GetCardsByNameList(List<string> cardNames);
    }
}
