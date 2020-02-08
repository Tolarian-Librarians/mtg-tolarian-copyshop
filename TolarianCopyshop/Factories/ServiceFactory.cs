using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business;
using Tolarian.Copyshop.ScryfallDataAccess;

namespace Tolarian.Copyshop.Factories
{
    public static class ServiceFactory
    {
        public static ICardDataGateway CreateCardDataGateway()
        {
            return new DataMapper();
        }
    }
}
