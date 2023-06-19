using System;
using System.Collections.Generic;
using System.Windows.Documents;

using static Tolarian.Copyshop.Business.UseCaseInteractors.PrintInteractor;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface IPrintRequester
    {
        FixedDocument GetPrintPages(PageFormat format, Stack<Uri> deckCards, float scale);
    }
}