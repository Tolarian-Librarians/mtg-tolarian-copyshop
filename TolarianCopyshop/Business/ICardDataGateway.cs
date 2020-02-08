using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business.Models;

namespace Tolarian.Copyshop.Business
{
    public interface ICardDataGateway
    {
        SfPaginatedCardList GetCardsByQuery(string query);
        SfCard GetCardById(Guid id);
    }
}
