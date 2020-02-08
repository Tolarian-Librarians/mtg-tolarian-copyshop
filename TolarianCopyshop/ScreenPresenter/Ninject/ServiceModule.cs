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
            Bind<ICardDataPresenter>().To<CardDataPresenter>();
            Bind<ICardDataRequester>().To<CardInteractor>();
            Bind<ICardDataGateway>().To<CardDataMapper>();
        }
    }
}
