using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business;
using Tolarian.Copyshop.Controller.ResponseObjects;

namespace Tolarian.Copyshop.Controller
{
    public class PrintController
    {
        private readonly IPrintRequester _requester;

        public PrintController(IPrintRequester requester)
        {
            _requester = requester;
        }

        public void PrintDeck(List<FullCardResponse> deckCards)
        {
            _requester.PrintDeck(deckCards.Select(o => o.PngImage).ToList());
        }
    }
}
