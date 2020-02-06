using CardInfoAccess.Model;
using Refit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardInfoAccess
{
    public interface IScryfallApi
    {
        [Get("/cards/{cardId}")]
        Task<MtgCard> GetCardById(Guid cardId);
    }
}
