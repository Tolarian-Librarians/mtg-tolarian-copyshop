using MahApps.Metro.Controls.Dialogs;
using Ninject.Modules;
using Tolarian.Copyshop.Business.Entities;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.UseCaseInteractors;
using Tolarian.Copyshop.ScreenPresenter.Communication;
using Tolarian.Copyshop.ScreenPresenter.Model;
using Tolarian.Copyshop.ScryfallDataAccess;

namespace Tolarian.Copyshop.ScreenPresenter.Ninject
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            // Business
            this.Bind<ICardDataRequester>().To<CardInteractor>().InSingletonScope();
            this.Bind<IPrintRequester>().To<PrintInteractor>().InSingletonScope();
            this.Bind<ICardDataGateway>().To<CardDataMapper>().InSingletonScope();
            this.Bind<ISetDataGateway>().To<SetDataMapper>().InSingletonScope();
            this.Bind<IDeckInfoInteractor>().To<DeckInfoInteractor>().InSingletonScope();
            this.Bind<IDeckImportInteractor>().To<DeckImportInteractor>().InSingletonScope();
            this.Bind<ISetCodeTranslator>().To<SetCodeTranslator>().InSingletonScope();
            this.Bind<IImportStringParser>().To<ImportStringParser>().InSingletonScope();

            // View
            this.Bind<IDialogCoordinator>().To<DialogCoordinator>().InSingletonScope();
            this.Bind<DeckCardModel>().To<DeckCardModel>().InSingletonScope();
            this.Bind<Dialogs>().To<Dialogs>().InSingletonScope();
        }
    }
}
