using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class FullCard : IFullCard
    {
        public static FullCard Create(IFullCard card)
        {
            if (card is null)
            {
                return new FullCard();
            }

            return new FullCard
            {
                CardCount = card.CardCount,
                CardId = card.CardId,
                PrintId = card.PrintId,
                Legalities = card.Legalities,
                SetCode = card.SetCode,
                CardFaces = card.CardFaces,
                IsTransformable = card.IsTransformable,
                FormattedCardName = card.FormattedCardName,
                RelatedCards = card.RelatedCards,
                ConvertedManaCost = card.ConvertedManaCost,
                ColorIdentity = card.ColorIdentity,
                Colors = card.Colors,
                ManaCostLine = card.ManaCostLine,
                ProducedMana = card.ProducedMana,
            };
        }

        public int CardCount { get; set; }
        public Guid CardId { get; set; }
        public Guid PrintId { get; set; }
        public Dictionary<string, string> Legalities { get; set; }
        public string SetCode { get; set; }
        public string ManaCostLine { get; set; }
        public ICollection<CardFace> CardFaces { get; set; }
        public ICollection<RelatedCard> RelatedCards { get; set; }
        public bool IsTransformable { get; set; }
        public string FormattedCardName { get; set; }
        public float ConvertedManaCost { get; set; }
        public List<MtgColor> ColorIdentity { get; set; }
        public List<MtgColor> Colors { get; set; }
        public List<MtgColor> ProducedMana { get; set; }
    }
}
