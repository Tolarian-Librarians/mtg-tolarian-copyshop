using System;
using System.Collections.Generic;

namespace Tolarian.Copyshop.Business.Models.DeckInfo
{
    public class DeckInfoCard
    {
        public int Copies { get; set; }
        public Guid PrintId { get; set; }
        public List<DeckInfoCardFace> cardFaces { get; set; }
    }
}
