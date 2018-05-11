using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hyl.Web.Infrastructure;
using Hyl.Web.Framework.Middleware;
using Hyl.Repository;
using Hyl.Core;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;

namespace Hyl.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            //services.AddMvc(config =>
            //{
            //    config.Filters.Add(new AuthorizeFilter(BaseConfig.AuthorizationPolicy));
            //});
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.CookieName = "HylCookie";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
            });

            return RegisterAutofac(services);
        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseMiddleware<OAuthTokenInHeaderMiddleware>(); // OAuth Authorize

            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller}/{action=Index}/{id?}"); // area注册
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }



        #region Autofac IOC
        public IContainer ApplicationContainer { get; private set; }
        private IServiceProvider RegisterAutofac(IServiceCollection services)
        {
            // ASP.NET Core docs for Autofac are here:
            // http://autofac.readthedocs.io/en/latest/integration/aspnetcore.html
            //
            // Add framework services.
            services.AddMvc();

            // Create the Autofac container builder.
            var builder = new ContainerBuilder();

            // Add any Autofac modules or registrations.
            builder.RegisterModule(new AutofacModule());
            // Populate the services.
            builder.Populate(services);

            builder.RegisterType<DbQuery>().As<IDbQuery>();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));


            var baseType = typeof(IServicesDependency);

            var assemblys = GetReferencingAssemblies(baseType.GetTypeInfo().Assembly.GetName().Name).ToList();
            builder.RegisterAssemblyTypes(assemblys.ToArray());
            builder.RegisterAssemblyTypes(assemblys.ToArray())
                .Where(t => baseType.IsAssignableFrom(t) && t != baseType)
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            // Build the container.
            this.ApplicationContainer = builder.Build();

            // Create and return the service provider.
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        public static IEnumerable<Assembly> GetReferencingAssemblies(string assemblyName)
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {
                if (IsCandidateLibrary(library, assemblyName))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }
            return assemblies;
        }

        private static bool IsCandidateLibrary(RuntimeLibrary library, string assemblyName)
        {
            return library.Name == (assemblyName)
                || library.Dependencies.Any(d => d.Name.StartsWith(assemblyName));
        }
        #endregion

    }
}
