using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Tolarian.Copyshop.Business;
using Tolarian.Copyshop.Controller.Interfaces;
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

        public void PrintDeck(PrintDialog printDlg, List<IFullCard> deckCards)
        {
            _requester.PrintDeck(printDlg, new Stack<Uri>(deckCards.Select(o => o.LargeImage)));
        }
    }
}
