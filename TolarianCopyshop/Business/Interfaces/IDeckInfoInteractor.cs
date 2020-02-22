using System.Collections.Generic;
using Tolarian.Copyshop.Business.Models.DeckInfo;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface IDeckInfoInteractor
    {
        int GetTotalCardCountOfDeck(List<DeckInfoCard> deck);
    }
}
