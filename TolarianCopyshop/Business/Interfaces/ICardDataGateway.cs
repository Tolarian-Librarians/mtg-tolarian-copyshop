using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface ICardDataGateway
    {
        SfPaginatedCardList GetCardsBySearchQuery(string query);
        SfCard GetCardByPrintId(Guid printId);
        SfPaginatedCardList GetCardsByNameList(List<string> cardNames);
        SfPaginatedCardList GetPrintsOfCard(Guid oracleId);
    }
}
