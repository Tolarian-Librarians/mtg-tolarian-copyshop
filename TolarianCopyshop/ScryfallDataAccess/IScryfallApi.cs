using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business.Models;

namespace Tolarian.Copyshop.ScryfallDataAccess
{
    public interface IScryfallApi
    {
        [Get("/cards/search?q=\"{searchQuery}\"")]
        Task<SfPaginatedCardList> GetCardsBySearchQuery(string searchQuery);
        
        [Get("/cards/search?q=!\"{searchQuery}\"&unique=art")]
        Task<SfPaginatedCardList> GetCardsBySearchQueryUniqueArt(string searchQuery);

    }
}
