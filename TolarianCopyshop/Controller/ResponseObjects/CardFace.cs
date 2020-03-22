using System;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class CardFace
    {
        public CardType CardType { get; set; }
        public string Name { get; set; }
        public Uri LargeImage { get; set; }
        public Uri SmallImage { get; set; }
        public Uri CroppedImage { get; set; }
        public string Text { get; set; }
    }
}
