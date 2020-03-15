using System.Collections.Generic;
using Tolarian.Copyshop.Controller.Interfaces;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class FullCardResponse
    {
        public string NotFound { get; set; }

        public List<IFullCard> Cards { get; set; } = new List<IFullCard>();
    }
}
