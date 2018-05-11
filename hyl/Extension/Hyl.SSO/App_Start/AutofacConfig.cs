using Autofac;
using Autofac.Integration.Mvc;
using System.Web;
using Hyl.Core.Configuration;
using Hyl.Core.DependencyRegistrar;
using Hyl.Core.Helpers.TypeFinder;
using Hyl.Core.Infrastructure;
using Hyl.SSO;

namespace Hyl.WebMvc.App_Start
{
    public class AutofacConfig : IDependencyRegistrar
    {
        private ContainerManager _containerManager;
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, HylWebConfig config)
        {
            // MVC - Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            // MVC - OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();
            // MVC - OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();


            //// MVC - OPTIONAL: Register web abstractions like HttpContextBase. 
            //builder.RegisterModule<AutofacWebTypesModule>(); //下面手动注册
            //HTTP context and other related stuff
            builder.Register(c => new HttpContextWrapper(HttpContext.Current) as HttpContextBase).As<HttpContextBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request).As<HttpRequestBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response).As<HttpResponseBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server).As<HttpServerUtilityBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session).As<HttpSessionStateBase>().InstancePerLifetimeScope();
            
        }

        public int Order
        {
            get { return 0; }
        }
    }
}