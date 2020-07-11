using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Controller.Interfaces;

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
                Legalities1 = card.Legalities1,
                Legalities2 = card.Legalities2,
                SetCode = card.SetCode,
                CardFaces = card.CardFaces,
                IsTransformable = card.IsTransformable,
                FormattedCardName = card.FormattedCardName,
                RelatedCards = card.RelatedCards,
            };
        }

        public int CardCount { get; set; }
        public Guid CardId { get; set; }
        public Guid PrintId { get; set; }
        public Dictionary<string, string> Legalities1 { get; set; }
        public Dictionary<string, string> Legalities2 { get; set; }
        public string SetCode { get; set; }
        public ICollection<CardFace> CardFaces { get; set; }
        public ICollection<RelatedCard> RelatedCards { get; set; }
        public bool IsTransformable { get; set; }
        public string FormattedCardName { get; set; }
    }
}
