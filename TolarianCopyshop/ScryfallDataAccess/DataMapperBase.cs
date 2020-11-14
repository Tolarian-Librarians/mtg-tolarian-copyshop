using Refit;
using System.Net;
using System.Web;

namespace Tolarian.Copyshop.ScryfallDataAccess
{
    public class DataMapperBase
    {
        protected void HandleUnexpectedStatusCodeForResponse<T>(ApiResponse<T> response)
        {
            throw new WebException($"Unexpected Status Code of Http Request. Status {response.StatusCode}, Error: {response.Error}");
        }
    }
}
