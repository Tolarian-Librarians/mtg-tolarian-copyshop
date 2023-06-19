using System.Net;

using Refit;

namespace Tolarian.Copyshop.ScryfallDataAccess
{
    public class DataMapperBase
    {
        protected DataMapperBase()
        {

        }

        protected static void HandleUnexpectedStatusCodeForResponse<T>(ApiResponse<T> response)
        {
            throw new WebException($"Unexpected Status Code of Http Request. Status {response.StatusCode}, Error: {response.Error}");
        }
    }
}