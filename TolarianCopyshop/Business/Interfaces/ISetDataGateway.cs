using Tolarian.Copyshop.Business.Models.SfSetInfo;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface ISetDataGateway
    {
        public SfPaginatedSetList GetAllSets();
    }
}