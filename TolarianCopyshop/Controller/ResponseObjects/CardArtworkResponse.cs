using System;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class CardArtworkResponse
    {
        public string SetName { get; set; }
        public string SetCode { get; set; }
        public Uri Image { get; set; }
    }
}
