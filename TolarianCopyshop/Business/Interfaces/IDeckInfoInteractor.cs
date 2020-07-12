using System.Collections.Generic;
using Tolarian.Copyshop.Business.Models.DeckInfo;
using Tolarian.Copyshop.Business.Models.Enums;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface IDeckInfoInteractor
    {
        int GetTotalCardCountOfDeck(List<DeckInfoCard> deck);
        Dictionary<float, int> GetManaCurve(List<DeckInfoCard> deck);
        Dictionary<MtgColor, int> GetColorSymbolCounts(List<DeckInfoCard> deck);
        Dictionary<MtgColor, int> GetManaSourcesCounts(List<DeckInfoCard> deck);
        Dictionary<CardType, int> GetCardTypeCounts(List<DeckInfoCard> deck);
    }
}
