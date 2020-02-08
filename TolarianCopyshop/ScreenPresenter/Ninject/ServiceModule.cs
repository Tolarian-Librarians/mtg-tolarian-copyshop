using Ninject.Modules;
using Tolarian.Copyshop.Business;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.ScryfallDataAccess;

namespace Tolarian.Copyshop.ScreenPresenter.Ninject
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ICardDataRequester>().To<CardInteractor>().InSingletonScope();
            this.Bind<ICardDataGateway>().To<CardDataMapper>().InSingletonScope();
        }
    }
}
