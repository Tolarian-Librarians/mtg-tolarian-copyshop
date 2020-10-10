using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Controller.Interfaces;

namespace Tolarian.Copyshop.Controller
{
    public class PrintController : TolarianControllerBase
    {
        private readonly IPrintRequester _requester;

        public PrintController(IPrintRequester requester)
        {
            this._requester = requester;
        }

        public FixedDocument GetPrintPages(Size pageSize, List<IFullCard> deckCards, float scale = 1.05f)
            => this._requester.GetPrintPages(
                pageSize,
                new Stack<Uri>(deckCards.SelectMany(card =>
                {
                    List<Uri> cards = new List<Uri>();
                    for (int i = 0; i < card.CardCount; i++)
                    {
                        cards.AddRange(card.CardFaces.Select(cf => cf.CroppedImage));
                    }

                    return cards;
                })),
                scale);
    }
}
