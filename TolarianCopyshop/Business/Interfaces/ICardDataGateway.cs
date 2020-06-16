using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Business.DbRequestModels;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface ICardDataGateway
    {
        SfCatalog GetCardNamesByAutoCompleteQuery(string query);
        SfCard GetCardByPrintId(Guid printId);
        SfCardCollection GetCardCollectionByIdentifiers(List<GetCardCollectionRequest> cardNames);
        List<SfCard> GetPrintsOfCard(Guid oracleId);
    }
}
