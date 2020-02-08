using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business.Models;

namespace Tolarian.Copyshop.Business
{
    public interface ICardDataRequester
    {
        List<SfCard> GetCardsBySearchQuery(string searchQuery, int maxCountOfItems);

        SfCard GetCardById(Guid id);
    }
}
