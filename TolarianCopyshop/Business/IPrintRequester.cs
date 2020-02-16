using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Tolarian.Copyshop.Business
{
    public interface IPrintRequester
    {
        void PrintDeck(PrintDialog printDlg, Stack<Uri> deckCards);
    }
}
