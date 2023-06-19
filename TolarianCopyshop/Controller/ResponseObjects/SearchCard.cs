using System;

using Tolarian.Copyshop.Controller.ResponseObjects.Enums;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class SearchCard
    {
        public string Name { get; set; }

        public CardType PrimaryCardType { get; set; }

        public Guid PrintId { get; set; }

        public Uri Image { get; set; }

        public string PowerToughness { get; set; }
    }
}