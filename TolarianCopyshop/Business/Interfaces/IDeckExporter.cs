using System.Collections.Generic;
using Tolarian.Copyshop.Business.Models.Export;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface IDeckExporter
    {
        public string ExportDeck(List<ExportCard> cards);
    }
}
