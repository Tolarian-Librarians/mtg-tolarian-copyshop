using System;
using System.Net;
using System.Net.Http;

using Refit;

namespace Tolarian.Copyshop.ScryfallDataAccess
{
    public class DataMapperBase
    {
        protected HttpClient _httpClient;

        protected DataMapperBase()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(Constants.SCRYFALL_BASE_URI)
            };

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "TolarianCopyshop/1.0");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        protected static void HandleUnexpectedStatusCodeForResponse<T>(ApiResponse<T> response)
        {
            throw new WebException($"Unexpected Status Code of Http Request. Status {response.StatusCode}, Error: {response.Error}");
        }
    }
}