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
                CardType = card.CardType,
                CardCount = card.CardCount,
                CardId = card.CardId,
                PrintId = card.PrintId,
                Legalities1 = card.Legalities1,
                Legalities2 = card.Legalities2,
                Name = card.Name,
                LargeImage = card.LargeImage,
                SmallImage = card.SmallImage,
                Text = card.Text,
                SetCode = card.SetCode,
                CroppedImage = card.CroppedImage,
            };
        }

        public string Name { get; set; }

        public Guid CardId { get; set; }

        public string Text { get; set; }

        public CardType CardType { get; set; }

        public int CardCount { get; set; }

        public Uri SmallImage { get; set; }

        public Uri LargeImage { get; set; }

        public Dictionary<string, string> Legalities1 { get; set; }

        public Dictionary<string, string> Legalities2 { get; set; }

        public string SetCode { get; set; }

        public Guid PrintId { get; set; }

        public Uri CroppedImage { get; set; }
    }
}
