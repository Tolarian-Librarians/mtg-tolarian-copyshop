using AutoMapper;
using Ninject.Modules;

namespace Tolarian.Copyshop.ScreenPresenter.Ninject
{
    internal class AutoMapperModule : NinjectModule
    {
        //public override void Load()
        //{
        //    Bind<IMapper>().ToMethod(AutoMapper).InSingletonScope();
        //}

        //private IMapper AutoMapper(Ninject.Activation.IContext context)
        //{
        //    Mapper (config =>
        //    {
        //        config.ConstructServicesUsing(type => context.Kernel.Get(type));

        //        config.CreateMap<MySource, MyDest>();
        //        // .... other mappings, Profiles, etc.              

        //    });

        //    Mapper.AssertConfigurationIsValid(); // optional
        //    return Mapper.Instance;
        //}
    }
}