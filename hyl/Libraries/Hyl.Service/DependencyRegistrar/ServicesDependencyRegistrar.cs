using System;
using System.Linq;
using Autofac;
using Hyl.Core;
using Hyl.Core.Configuration;
using Hyl.Core.DependencyRegistrar;
using Hyl.Core.Helpers.TypeFinder;

namespace Hyl.Service.DependencyRegistrar
{
    public class ServicesDependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, HylWebConfig config)
        {
            //register all services that inherit interface IServicesDependency
            var baseType = typeof(IServicesDependency);

            //var assembly = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();
            var assembly = AppDomain.CurrentDomain.GetAssemblies().ToList();
            builder.RegisterAssemblyTypes(assembly.ToArray()).Where(t => baseType.IsAssignableFrom(t) && t != baseType).AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        public int Order { get { return 0; } }
    }
}
