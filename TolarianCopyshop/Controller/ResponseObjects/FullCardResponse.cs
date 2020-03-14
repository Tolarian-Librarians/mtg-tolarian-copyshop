using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class FullCardResponse : IFullCard
    {
        public FullCardResponse()
        {

        }

        public FullCardResponse(IFullCard card)
        {
            if (card != null)
            {
                this.CardType = card.CardType;
                this.CardCount = card.CardCount;
                this.Id = card.Id;
                this.Legalities1 = card.Legalities1;
                this.Legalities2 = card.Legalities2;
                this.Name = card.Name;
                this.LargeImage = card.LargeImage;
                this.SmallImage = card.SmallImage;
                this.Text = card.Text;
                this.SetCode = card.SetCode;
            }
        }

        public string Name { get; set; }

        public Guid Id { get; set; }

        public string Text { get; set; }

        public CardType CardType { get; set; }

        public int CardCount { get; set; }

        public Uri SmallImage { get; set; }

        public Uri LargeImage { get; set; }

        public Dictionary<string, string> Legalities1 { get; set; }

        public Dictionary<string, string> Legalities2 { get; set; }
        public string SetCode { get; set; }
    }
}
