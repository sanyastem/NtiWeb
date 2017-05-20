using ASUVP.Core.Logging;
using ASUVP.Online.OData;
using ASUVP.Online.Services;
//using ASUVP.Online.Web.Models.SupplementaryAgreement.ModelBinder;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;

namespace ASUVP.Online.Web
{
    public class AutofacContainer
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof (AutofacContainer).Assembly);

            builder.RegisterType<EventLogger>().As<IEventLogger>().WithParameter("name", "WebEventLogger");

            builder.RegisterModule(new AutofacODataModule());
            builder.RegisterModule(new AutofacRestModule());

            //builder.RegisterModelBinders(typeof(StepViewModelBinder).Assembly);
            //builder.RegisterModelBinderProvider();

            return builder.Build();
        }
    }
}