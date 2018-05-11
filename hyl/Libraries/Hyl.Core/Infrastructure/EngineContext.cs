using System.Configuration;
using System.Runtime.CompilerServices;
using Hyl.Core.Caching;
using Hyl.Core.Configuration;

namespace Hyl.Core.Infrastructure
{
    public class EngineContext
    {
        #region Methods

        /// <summary>
        /// Initializes a static instance of the Nop factory.
        /// </summary>
        /// <param name="forceRecreate">Creates a new factory instance even though the factory has been previously initialized.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                Singleton<IEngine>.Instance = new HylEngine();
                var config = ConfigurationManager.GetSection("HylWebConfig") as HylWebConfig;
                Singleton<IEngine>.Instance.Initialize(config);
            }
            return Singleton<IEngine>.Instance;
        }

        /// <summary>
        /// Sets the static engine instance to the supplied engine. Use this method to supply your own engine implementation.
        /// </summary>
        /// <param name="engine">The engine to use.</param>
        /// <remarks>Only use this method if you know what you're doing.</remarks>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the singleton Nop engine used to access Nop services.
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize(false);
                }
                return Singleton<IEngine>.Instance;
            }
        }

        public static ContainerManager ContainerManager
        {
            get { return Current.ContainerManager; }
        }

        public static ICacheManager CacheManager
        {
            get
            {
                if (Singleton<ICacheManager>.Instance == null)
                {
                    Singleton<ICacheManager>.Instance = Current.ContainerManager.Resolve<ICacheManager>("nop_cache_static");
                }
                return Singleton<ICacheManager>.Instance;
            }
        }
        #endregion
    }
}
