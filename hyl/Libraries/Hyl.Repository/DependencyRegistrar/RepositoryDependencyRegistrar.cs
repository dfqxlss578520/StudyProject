using System.Data;
using Autofac;
using Hyl.Core.Configuration;
using Hyl.Core.DependencyRegistrar;
using Hyl.Core.Helpers.TypeFinder;

namespace Hyl.Repository.DependencyRegistrar
{
    public class RepositoryDependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, HylWebConfig config)
        {
            builder.Register(db => new DbConnectionFactory(config).CreateDbConnection()).As<IDbConnection>();
            builder.RegisterType<DbQuery>().As<IDbQuery>();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            //builder.RegisterGeneric(typeof(NhibernateRepository<>)).Named<string>("NH").As(typeof(IRepository<>));
        }

        public int Order
        {
            get { return 0; }
        }
    }
}
