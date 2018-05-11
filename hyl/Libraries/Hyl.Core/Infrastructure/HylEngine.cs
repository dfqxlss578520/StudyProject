using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Hyl.Core.Configuration;
using Hyl.Core.DependencyRegistrar;
using Hyl.Core.Helpers.TypeFinder;

namespace Hyl.Core.Infrastructure
{
    public class HylEngine : IEngine
    {
        #region Fields

        private ContainerManager _containerManager;

        #endregion


        #region Utilities

        /// <summary>
        /// Register dependencies
        /// </summary>
        /// <param name="config">Config</param>
        protected virtual void RegisterDependencies(HylWebConfig config)
        {
            var builder = new ContainerBuilder();

            ////dependencies
            var typeFinder = new WebAppTypeFinder();
            builder.RegisterInstance(config).As<HylWebConfig>().SingleInstance();
            builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();

            //register dependencies provided by other assemblies
            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var drInstances = new List<IDependencyRegistrar>();
            foreach (var drType in drTypes)
                drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            //sort
            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in drInstances)
                dependencyRegistrar.Register(builder, typeFinder, config);

            var container = builder.Build();
            this._containerManager = new ContainerManager(container);

            //set dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize components and plugins in the nop environment.
        /// </summary>
        /// <param name="config">Config</param>
        /// 
        public void Initialize(HylWebConfig config)
        {

            //register dependencies
            RegisterDependencies(config);
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T Resolve<T>(string key = "") where T : class
        {
            return ContainerManager.Resolve<T>(key);
        }

        /// <summary>
        ///  Resolve dependency
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        /// <summary>
        /// Resolve dependencies
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Container manager
        /// </summary>
        public virtual ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion
    }
}
