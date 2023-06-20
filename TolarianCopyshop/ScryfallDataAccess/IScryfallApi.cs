using System;
using System.Threading.Tasks;

using Refit;

using Tolarian.Copyshop.Business.Models.SfCardInfo;
using Tolarian.Copyshop.Business.Models.SfSetInfo;

namespace Tolarian.Copyshop.ScryfallDataAccess
{
    public interface IScryfallApi
    {
        [Get("/cards/{printId}")]
        Task<ApiResponse<SfCard>> GetCardByPrintId(Guid printId);

        [Get("/cards/autocomplete?q=\"{searchQuery}\"&include_extras=false")]
        Task<ApiResponse<SfCatalog>> GetCardsByAutoCompleteQuery(string searchQuery);

        [Get("/cards/search?q={searchQuery}+(layout:token+or+layout:emblem)&include_extras=true")]
        Task<ApiResponse<SfPaginatedCardList>> GetTokensByQuery(string searchQuery);

        [Get("/cards/named?exact=\"{cardName}\"&set={setCode}")]
        Task<ApiResponse<SfCard>> GetCardByExactName(string cardName, string setCode);

        [Get("/cards/search?q=oracleid:{oracleId}&unique=prints&include_extras=true&page={page}")]
        Task<ApiResponse<SfPaginatedCardList>> GetPrintsBySearchQuery(Guid oracleId, int page);

        [Post("/cards/collection")]
        Task<ApiResponse<SfCardCollection>> GetCardsByCollection([Body] SfIdentifierContainer identifiers);

        [Get("/sets")]
        Task<ApiResponse<SfPaginatedSetList>> GetAllSets();

    }
}