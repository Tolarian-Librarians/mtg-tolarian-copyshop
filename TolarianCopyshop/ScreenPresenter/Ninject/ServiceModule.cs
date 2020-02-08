using Ninject.Modules;
using Tolarian.Copyshop.Business;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.ScreenPresenter.Models;
using Tolarian.Copyshop.ScryfallDataAccess;

namespace Tolarian.Copyshop.ScreenPresenter.Ninject
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ICardDataPresenter>().To<CardDataPresenter>().InSingletonScope();
            this.Bind<ICardDataRequester>().To<CardInteractor>().InSingletonScope();
            this.Bind<ICardDataGateway>().To<CardDataMapper>().InSingletonScope();
        }
    }
}
