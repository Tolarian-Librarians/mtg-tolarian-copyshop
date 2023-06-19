using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Controller.Interfaces;

using static Tolarian.Copyshop.Business.UseCaseInteractors.PrintInteractor;

namespace Tolarian.Copyshop.Controller
{
    public class PrintController : TolarianControllerBase
    {
        private readonly IPrintRequester _requester;

        public PrintController(IPrintRequester requester)
        {
            _requester = requester;
        }

        public FixedDocument GetPrintPages(PageFormat selectedPageFormat, List<IFullCard> deckCards, float scale = 1.05f)
            => _requester.GetPrintPages(
                selectedPageFormat,
                new Stack<Uri>(deckCards.OrderByDescending(card => card.FormattedCardName).SelectMany(card =>
                {
                    List<Uri> cards = new();
                    for (int i = 0; i < card.CardCount; i++)
                    {
                        cards.AddRange(card.CardFaces.Select(cf => cf.CroppedImage));
                    }

                    return cards;
                })),
                scale);
    }
}