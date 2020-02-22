using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface ICardDataGateway
    {
        SfPaginatedCardList GetCardsByQuery(string query);
        SfCard GetCardById(Guid id);
        SfPaginatedCardList GetCardsByNameList(List<string> cardNames);
    }
}
