using Ninject;

using Tolarian.Copyshop.Fontend.WPF.ViewModels;

namespace Tolarian.Copyshop.Fontend.WPF.Ninject
{
    public class ServiceLocator
    {
        private readonly IKernel kernel;

        public ServiceLocator()
        {
            kernel = new StandardKernel(new ServiceModule());
        }

        public CopyShopViewModel CopyShopViewModel
            => kernel.Get<CopyShopViewModel>();

        public DeckBuilderViewModel DeckBuilderViewModel
            => kernel.Get<DeckBuilderViewModel>();

        public DeckPrintViewModel DeckViewerViewModel
            => kernel.Get<DeckPrintViewModel>();

        public DeckStatisticsViewModel DeckStatisticsViewModel
            => kernel.Get<DeckStatisticsViewModel>();
    }
}