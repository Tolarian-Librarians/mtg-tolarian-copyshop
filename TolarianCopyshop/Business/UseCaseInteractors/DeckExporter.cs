using System.Collections.Generic;
using System.Text;

using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.Export;

namespace Tolarian.Copyshop.Business.UseCaseInteractors
{
    public class DeckExporter : IDeckExporter
    {
        public string ExportDeck(List<ExportCard> cards)
        {
            StringBuilder builder = new();

            foreach (ExportCard card in cards)
            {
                builder.AppendLine(GetExportLine(card));
            }

            return builder.ToString();
        }

        private string GetExportLine(ExportCard card) => $"{card.CardCount} {card.Name} ({card.SetCode})";
    }
}