using Ninject;
using Tolarian.Copyshop.Fontend.WPF.ViewModels;

namespace Tolarian.Copyshop.Fontend.WPF.Ninject
{
    public class ServiceLocator
    {
        private readonly IKernel kernel;

        public ServiceLocator()
        {
            this.kernel = new StandardKernel(new ServiceModule());
        }

        public CopyShopViewModel CopyShopViewModel
            => this.kernel.Get<CopyShopViewModel>();

        public DeckBuilderViewModel DeckBuilderViewModel
            => this.kernel.Get<DeckBuilderViewModel>();

        public DeckPrintViewModel DeckViewerViewModel
            => this.kernel.Get<DeckPrintViewModel>();

        public DeckStatisticsViewModel DeckStatisticsViewModel
            => this.kernel.Get<DeckStatisticsViewModel>();
    }
}
