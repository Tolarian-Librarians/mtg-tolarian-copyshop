using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Business.Models.Enums;

namespace Tolarian.Copyshop.Business.Models.DeckInfo
{
    public class DeckInfoCard
    {
        public int Copies { get; set; }
        public Guid PrintId { get; set; }
        public List<DeckInfoCardFace> cardFaces { get; set; }
        public string ManaCostLine { get; set; }
        public float ConvertedManaCost { get; set; }
        public List<MtgColor> ProducedMana { get; set; }
        public List<MtgColor> ColorIdentity { get; set; }
    }
}
