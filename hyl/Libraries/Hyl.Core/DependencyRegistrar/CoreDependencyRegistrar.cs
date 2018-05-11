using System.Reflection;
using Autofac;
using Hyl.Core.Caching;
using Hyl.Core.Configuration;
using Hyl.Core.Helpers.TypeFinder;
using Hyl.Core.Job;
using Hyl.Core.Logs;
using Quartz;
using Quartz.Impl;

namespace Hyl.Core.DependencyRegistrar
{
    public class CoreDependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return -1; }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, HylWebConfig config)
        {
            //cache managers
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("hyl_cache_per_request").InstancePerLifetimeScope();
            if (config.RedisCachingEnabled)
            {
                builder.Register(wp => new RedisConnectionWrapper(config)).As<IRedisConnectionWrapper>().SingleInstance();
                builder.RegisterType<RedisCacheManager>().As<ICacheManager>().InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance();
            }
            builder.RegisterGeneric(typeof(DefaultLog<>)).WithParameter(new NamedParameter("config", config)).As(typeof(ILogs<>));
            builder.RegisterType<QuartzManager>().As<IJobManager>();

            // Schedule
            builder.Register(x => new StdSchedulerFactory().GetScheduler()).As<IScheduler>();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(x => typeof(IJob).IsAssignableFrom(x));

        }
    }
}
