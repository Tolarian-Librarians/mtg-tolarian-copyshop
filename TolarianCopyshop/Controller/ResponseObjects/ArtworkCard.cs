using System;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class ArtworkCard
    {
        public string SetName { get; set; }

        public string SetCode { get; set; }

        public Uri Image { get; set; }

        public Guid PrintId { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}