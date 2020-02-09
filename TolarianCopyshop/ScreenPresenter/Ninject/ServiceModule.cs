using MahApps.Metro.Controls.Dialogs;
using Ninject.Modules;
using Tolarian.Copyshop.Business;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.ScreenPresenter.Model;
using Tolarian.Copyshop.ScryfallDataAccess;

namespace Tolarian.Copyshop.ScreenPresenter.Ninject
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ICardDataRequester>().To<CardInteractor>().InSingletonScope();
            this.Bind<ICardDataGateway>().To<CardDataMapper>().InSingletonScope();
            this.Bind<IDialogCoordinator>().To<DialogCoordinator>().InSingletonScope();
            this.Bind<DeckCardModel>().To<DeckCardModel>().InSingletonScope();
        }
    }
}
