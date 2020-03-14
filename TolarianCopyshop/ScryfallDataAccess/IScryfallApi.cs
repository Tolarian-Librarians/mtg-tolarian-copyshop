using Refit;
using System;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tolarian.Copyshop.ScryfallDataAccess
{
    public interface IScryfallApi
    {
        [Get("/cards/{printId}")]
        Task<ApiResponse<SfCard>> GetCardByPrintId(Guid printId);

        [Get("/cards/autocomplete?q=\"{searchQuery}\"&include_extras={includeExtras}")]
        Task<ApiResponse<SfCatalog>> GetCardsByAutoCompleteQuery(string searchQuery, bool includeExtras = false);

        [Get("/cards/named?exact=\"{cardName}\"")]
        Task<ApiResponse<SfPaginatedCardList>> GetCardByExactName(string cardName);

        [Get("/cards/search?q=oracleid:{oracleId}&unique=prints")]
        Task<ApiResponse<SfPaginatedCardList>> GetPrintsBySearchQuery(Guid oracleId);

        [Post("/cards/collection")]
        Task<ApiResponse<SfPaginatedCardList>> GetCardsByCollection([Body] SfIdentifierContainer identifiers);

    }
}
