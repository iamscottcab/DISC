using System;

namespace DISC
{
    public interface IRootDIScope : IDIScope
    {
        #region Bootstrapping

        /// <summary>
        /// Registers the entry point or startup class of the application as the root of the dependency tree.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Thrown if an entry point is already registered.</exception>
        void RegisterEntryPoint<TService>() where TService : class;

        /// <summary>
        /// Resolves the registered entry point to bootstrap the program.
        /// </summary>
        /// <returns>The type of TService provided by the generic constraint.</returns>
        /// <exception cref="System.NullReferenceException">Thrown if no entry point has been registered.</exception>
        /// <exception cref="System.ArgumentException">Thrown if the requested type does not match the registered type.</exception>
        TService ResolveEntryPoint<TService>() where TService : class;

        #endregion

        #region Singletons

        /// <summary>
        /// Registers a service with a Singleton lifetime.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract.</exception>
        void RegisterSingleton<TService>() where TService : class;

        /// <summary>
        /// Registers a service with a Singleton lifetime using the provided factory.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract.</exception>
        void RegisterSingleton<TService>(Func<TService> factory) where TService : class;

        /// <summary>
        /// Registers a service with a Singleton lifetime.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract.</exception>
        void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService;

        /// <summary>
        /// Registers a service with a Singleton lifetime using the provided factory.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract.</exception>
        void RegisterSingleton<TService, TImplementation>(Func<TImplementation> factory) where TImplementation : class, TService;

        #endregion

        #region Transients

        /// <summary>
        /// Registers a service with a Transient lifetime.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract.</exception>
        void RegisterTransient<TService>() where TService : class;

        /// <summary>
        /// Registers a service with a Transient lifetime using the provided factory.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract.</exception>
        void RegisterTransient<TService>(Func<TService> factory) where TService : class;

        /// <summary>
        /// Registers a service with a Transient lifetime.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract.</exception>
        void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService;

        /// <summary>
        /// Registers a service with a Transient lifetime using the provided factory.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract.</exception>
        void RegisterTransient<TService, TImplementation>(Func<TImplementation> factory) where TImplementation : class, TService;

        #endregion
    }
}
