using System.Collections.Generic;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class CardArtworkResponse
    {
        public List<ArtworkCard> Artworks { get; set; } = new List<ArtworkCard>();
    }
}