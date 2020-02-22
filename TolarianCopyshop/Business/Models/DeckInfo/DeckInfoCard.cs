using System;
using System.Collections.Generic;

namespace Tolarian.Copyshop.Business.Models.DeckInfo
{
    public class DeckInfoCard
    {
        public string CardType { get; set; }
        public int Copies { get; set; }
        public Guid Id { get; set; }
        public List<DeckInfoCardFace> cardFaces { get; set; }
    }
}
