using System.Collections.Generic;
using Tolarian.Copyshop.Business.Models;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface IDeckInfoInteractor
    {
        int GetTotalCardCountOfDeck(List<SfCard> deck);
    }
}
