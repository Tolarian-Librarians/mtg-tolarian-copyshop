using Refit;
using System;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tolarian.Copyshop.ScryfallDataAccess
{
    public interface IScryfallApi
    {
        [Get("/cards/{id}")]
        Task<ApiResponse<SfCard>> GetCardById(Guid id);

        [Get("/cards/search?q=\"{searchQuery}\"")]
        Task<ApiResponse<SfPaginatedCardList>> GetCardsBySearchQuery(string searchQuery);
        
        [Get("/cards/search?q=!\"{searchQuery}\"&unique=art")]
        Task<ApiResponse<SfPaginatedCardList>> GetCardsBySearchQueryUniqueArt(string searchQuery);

        [Post("/cards/collection")]
        Task<ApiResponse<SfPaginatedCardList>> GetCardsByCollection([Body] SfIdentifierContainer identifiers);

    }
}
