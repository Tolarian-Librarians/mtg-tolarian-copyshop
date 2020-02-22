using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Business.Models;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface ICardDataRequester
    {
        List<SfCard> GetCardsBySearchQuery(string searchQuery, int maxCountOfItems, out int maxResults);
        SfCard GetCardById(Guid id);
        List<SfCard> GetCardsByNameList(List<string> cardNames);
    }
}
