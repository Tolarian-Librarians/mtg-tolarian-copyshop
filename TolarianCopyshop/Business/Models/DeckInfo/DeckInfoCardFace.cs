using System.Collections.Generic;

using Tolarian.Copyshop.Business.Models.Enums;

namespace Tolarian.Copyshop.Business.Models.DeckInfo
{
    public class DeckInfoCardFace
    {
        public CardType PrimaryCardType { get; set; }
        public List<MtgColor> Colors { get; set; }
    }
}