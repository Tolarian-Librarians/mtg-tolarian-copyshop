using System;
using System.Collections.Generic;

using Tolarian.Copyshop.Controller.ResponseObjects.Enums;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class CardFace
    {
        public List<CardType> CardTypes { get; set; }
        public CardType PrimaryCardType { get; set; }
        public string Name { get; set; }
        public Uri LargeImage { get; set; }
        public Uri SmallImage { get; set; }
        public Uri CroppedImage { get; set; }
        public string Text { get; set; }
        public List<MtgColor> Colors { get; set; }
    }
}