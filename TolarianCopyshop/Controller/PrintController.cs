using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Controller.Interfaces;

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
            _requester.PrintDeck(printDlg, new Stack<Uri>(deckCards.SelectMany(card =>
            {
                var cards = new List<Uri>();
                for(int i = 0; i < card.CardCount; i++)
                    cards.Add(card.LargeImage);

                return cards;
            }
            )));
        }
    }
}
