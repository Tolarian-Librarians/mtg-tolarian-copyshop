using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface IPrintRequester
    {
        void PrintDeck(PrintDialog printDlg, Stack<Uri> deckCards);
    }
}
