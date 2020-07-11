using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.Models.SaveAndLoad;
using Tolarian.Copyshop.Controller.Interfaces;

namespace Tolarian.Copyshop.Controller.Mappers
{
    public static class SaveAndLoadMapper
    {
        public static List<SaveCard> ConvertToBusiness(List<IFullCard> deck)
        {
            return deck.Select(dc => new SaveCard { CardCount = dc.CardCount, PrintId = dc.PrintId}).ToList();
        }
            
    }
}
