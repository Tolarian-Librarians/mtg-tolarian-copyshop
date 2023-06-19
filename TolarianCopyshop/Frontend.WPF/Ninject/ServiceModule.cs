using MahApps.Metro.Controls.Dialogs;

using Ninject.Modules;

using Tolarian.Copyshop.Business.Entities;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.UseCaseInteractors;
using Tolarian.Copyshop.Fontend.WPF.Communication;
using Tolarian.Copyshop.Fontend.WPF.Model;
using Tolarian.Copyshop.ScryfallDataAccess;

namespace Tolarian.Copyshop.Fontend.WPF.Ninject
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            // Business
            Bind<ICardDataRequester>().To<CardInteractor>().InSingletonScope();
            Bind<IPrintRequester>().To<PrintInteractor>().InSingletonScope();
            Bind<IDeckExporter>().To<DeckExporter>().InSingletonScope();
            Bind<ICardDataGateway>().To<CardDataMapper>().InSingletonScope();
            Bind<ISetDataGateway>().To<SetDataMapper>().InSingletonScope();
            Bind<IDeckInfoInteractor>().To<DeckInfoInteractor>().InSingletonScope();
            Bind<IDeckImportInteractor>().To<DeckImportInteractor>().InSingletonScope();
            Bind<ISetCodeTranslator>().To<SetCodeTranslator>().InSingletonScope();
            Bind<IImportStringParser>().To<ImportStringParser>().InSingletonScope();
            Bind<ISaveAndLoadInteractor>().To<SaveAndLoadInteractor>().InSingletonScope();

            // View
            Bind<IDialogCoordinator>().To<DialogCoordinator>().InSingletonScope();
            Bind<DeckCardModel>().To<DeckCardModel>().InSingletonScope();
            Bind<Dialogs>().To<Dialogs>().InSingletonScope();
        }
    }
}