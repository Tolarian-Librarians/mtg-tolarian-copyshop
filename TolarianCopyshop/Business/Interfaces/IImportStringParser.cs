using System.Collections.Generic;
using Tolarian.Copyshop.Business.EntitiesModels;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface IImportStringParser
    {
        List<KeyValuePair<PreImportCard, int>> ResolvePreImportCardsFromImportString(List<string> importLines);
    }
}
