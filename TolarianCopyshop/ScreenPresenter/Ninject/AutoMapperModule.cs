using AutoMapper;
using Ninject.Modules;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.ScreenPresenter.AutoMapper;

namespace Tolarian.Copyshop.ScreenPresenter.Ninject
{
    internal class AutoMapperModule : NinjectModule
    {
        public override void Load()
        {
            var mapperConfiguration = CreateConfiguration();
            this.Bind<IMapper>().ToConstructor(c => new Mapper(mapperConfiguration)).InSingletonScope();
            this.Bind<CardController>().ToSelf().InSingletonScope();
        }

        private MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Add all profiles in current assembly
                cfg.AddProfile(new AutoMapperProfile());
            });

            return config;
        }
    }
}