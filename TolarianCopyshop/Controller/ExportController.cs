using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.Export;
using Tolarian.Copyshop.Controller.Interfaces;

namespace Tolarian.Copyshop.Controller
{
    public class ExportController : TolarianControllerBase
    {
        private readonly IDeckExporter _deckExporter;

        public ExportController(IDeckExporter deckExporter)
        {
            this._deckExporter = deckExporter;
        }

        public string ExportDeck(List<IFullCard> deck)
        {
            return this._deckExporter.ExportDeck(deck.Select(card => new ExportCard
            {
                CardCount = card.CardCount,
                Name = card.CardFaces.First().Name,
                SetCode = card.SetCode
            }).ToList()
            );
        }
    }
}
