using System.Net;

using Refit;

using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.SfSetInfo;

namespace Tolarian.Copyshop.ScryfallDataAccess
{
    public class SetDataMapper : DataMapperBase, ISetDataGateway
    {
        SfPaginatedSetList allSetsCached;

        IScryfallApi _service;

        public SetDataMapper()
        {
            _service = RestService.For<IScryfallApi>(_httpClient);
        }

        public SfPaginatedSetList GetAllSets()
        {
            if (allSetsCached != null)
            {
                return allSetsCached;
            }

            ApiResponse<SfPaginatedSetList> response = _service.GetAllSets().Result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    allSetsCached = response.Content;
                    break;
                default:
                    HandleUnexpectedStatusCodeForResponse(response);
                    break;
            }

            return allSetsCached;
        }
    }
}