using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface IDeckImportInteractor
    {
        (List<SfCard> Cards, string NotFound) GetCardsForImport(List<string> importLines);
        (List<SfCard>, string notFound) ImportFromTappedOut(Uri deckUrl);
    }
}
