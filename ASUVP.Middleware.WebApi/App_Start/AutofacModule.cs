using ASUVP.Middleware.WebApi.OAuth;
using Autofac;
using AutoMapper;

namespace ASUVP.Middleware.WebApi
{
    public class AutofacMiddlewareModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var configuration = new MapperConfiguration(x =>
            {
                x.AddProfile<DomainToODataModelProfile>();
                x.AddProfile<DomainToDtoProfile>();
                x.AddProfile<DtoToDomainProfile>();
            });

            builder.Register(config => configuration.CreateMapper()).As<IMapper>().SingleInstance();

            builder.RegisterType<OAuthAuthorizationProvider>();
        }
    }
}