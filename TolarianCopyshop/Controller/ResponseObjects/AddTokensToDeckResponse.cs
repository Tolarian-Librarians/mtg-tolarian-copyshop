using System.Collections.Generic;

using Tolarian.Copyshop.Controller.Interfaces;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class AddTokensToDeckResponse
    {
        public List<IFullCard> Deck { get; set; }
    }
}