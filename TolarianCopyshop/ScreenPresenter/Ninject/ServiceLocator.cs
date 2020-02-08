using Ninject;
using Tolarian.Copyshop.ScreenPresenter.ViewModels;

namespace Tolarian.Copyshop.ScreenPresenter.Ninject
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
    }
}
