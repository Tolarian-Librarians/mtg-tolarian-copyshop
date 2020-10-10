using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface IPrintRequester
    {
        FixedDocument GetPrintPages(Size pageSize, Stack<Uri> deckCards, float scale);
    }
}
