using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tolarian.Copyshop.Business
{
    public interface IPrintRequester
    {
        void PrintDeck(List<Uri> deckCards);
    }
}
