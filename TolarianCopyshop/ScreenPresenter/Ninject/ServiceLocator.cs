using Ninject;
using Tolarian.Copyshop.ScreenPresenter.ViewModels;

namespace Tolarian.Copyshop.ScreenPresenter.Ninject
{
    public class ServiceLocator
    {
        private readonly IKernel kernel;

        public ServiceLocator()
        {
            kernel = new StandardKernel(new ServiceModule(), new AutoMapperModule());
        }

        public DeckBuilderViewModel ViewModel
        {
            get => kernel.Get<DeckBuilderViewModel>();
        }
    }
}
