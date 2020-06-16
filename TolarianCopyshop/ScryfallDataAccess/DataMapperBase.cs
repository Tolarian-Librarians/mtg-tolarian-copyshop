using Refit;
using System.Web;

namespace Tolarian.Copyshop.ScryfallDataAccess
{
    public class DataMapperBase
    {
        protected void HandleUnexpectedStatusCodeForResponse<T>(ApiResponse<T> response)
        {
            throw new HttpException($"Unexpected Status Code of Http Request. Status {response.StatusCode}, Error: {response.Error}");
        }
    }
}
