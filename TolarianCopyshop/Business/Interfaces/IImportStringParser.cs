using System.Collections.Generic;
using Tolarian.Copyshop.Business.EntitiesModels;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface IImportStringParser
    {
        Dictionary<PreImportCard, int> ResolvePreImportCardsFromImportString(List<string> importLines);
    }
}
