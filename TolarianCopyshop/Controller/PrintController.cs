using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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

        public void PrintDeck(PrintDialog printDlg, List<FullCardResponse> deckCards)
        {
            _requester.PrintDeck(printDlg, deckCards.Select(o => o.PngImage).ToList());
        }
    }
}
