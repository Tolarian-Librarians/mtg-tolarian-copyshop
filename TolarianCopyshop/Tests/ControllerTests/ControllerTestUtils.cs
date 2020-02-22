using AutoMapper;
using Tolarian.Copyshop.ScreenPresenter.AutoMapper;

namespace Tests.ControllerTests
{
    internal class ControllerTestUtils
    {
        internal static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Add all profiles in current assembly
                cfg.AddProfile(new AutoMapperProfile());
            });

            return new Mapper(config);
        }
    }
}
