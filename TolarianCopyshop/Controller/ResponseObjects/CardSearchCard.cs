using System;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class CardSearchCard
    {
        public string Name { get; set; }

        public CardType CardType { get; set; }

        public Guid Id { get; set; }

        public Uri Image { get; set; }
    }
}
