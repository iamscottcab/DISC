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
        /// <exception cref="ArgumentNullException">If any required params are null.</exception>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract or not a class.</exception>
        void RegisterSingleton(Type serviceType);

        /// <summary>
        /// Registers a service with a Singleton lifetime.
        /// </summary>
        /// Registers a service with a Singleton lifetime.
        /// </summary>
        /// <exception cref="ArgumentNullException">If any required params are null.</exception>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract or not a class.</exception>
        /// <exception cref="InvalidCastException">Thrown if the implementation is not assignable to the service type.</exception>
        void RegisterSingleton(Type serviceType, Func<object> factory);

        /// <summary>
        /// Registers a service with a Singleton lifetime.
        /// </summary>
        /// <exception cref="ArgumentNullException">If any required params are null.</exception>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract or not a class.</exception>
        /// <exception cref="InvalidCastException">Thrown if the implementation is not assignable to the service type.</exception> 
        void RegisterSingleton(Type serviceType, Type implementationType);

        /// <summary>
        /// Registers a service with a Singleton lifetime.
        /// </summary>
        /// <exception cref="ArgumentNullException">If any required params are null.</exception>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract or not a class.</exception>
        /// <exception cref="InvalidCastException">Thrown if the implementation is not assignable to the service type.</exception>
        void RegisterSingleton(Type serviceType, Type implementationType, Func<object> factory);

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
        /// <exception cref="ArgumentNullException">If any required params are null.</exception>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract or not a class.</exception>
        void RegisterTransient(Type serviceType);

        /// <summary>
        /// Registers a service with a Transient lifetime.
        /// </summary>
        /// <exception cref="ArgumentNullException">If any required params are null.</exception>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract or not a class.</exception>
        /// <exception cref="InvalidCastException">Thrown if the implementation is not assignable to the service type.</exception>
        void RegisterTransient(Type serviceType, Func<object> factory);

        /// <summary>
        /// Registers a service with a Transient lifetime.
        /// </summary>
        /// <exception cref="ArgumentNullException">If any required params are null.</exception>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract or not a class.</exception>
        /// <exception cref="InvalidCastException">Thrown if the implementation is not assignable to the service type.</exception>
        void RegisterTransient(Type serviceType, Type implementationType);

        /// <summary>
        /// Registers a service with a Transient lifetime.
        /// </summary>
        /// <exception cref="ArgumentNullException">If any required params are null.</exception>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract or not a class.</exception>
        /// <exception cref="InvalidCastException">Thrown if the implementation is not assignable to the service type.</exception>
        void RegisterTransient(Type serviceType, Type implementationType, Func<object> factory);

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

        #region Scoped

        /// <summary>
        /// Registers a service with a Scoped lifetime.
        /// </summary>
        /// <exception cref="ArgumentNullException">If any required params are null.</exception>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract or not a class.</exception>
        void RegisterScoped(Type serviceType);

        /// <summary>
        /// Registers a service with a Scoped lifetime.
        /// </summary>
        /// <exception cref="ArgumentNullException">If any required params are null.</exception>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract or not a class.</exception>
        /// <exception cref="InvalidCastException">Thrown if the implementation is not assignable to the service type.</exception>
        void RegisterScoped(Type serviceType, Func<object> factory);

        /// <summary>
        /// Registers a service with a Scoped lifetime.
        /// </summary>
        /// <exception cref="ArgumentNullException">If any required params are null.</exception>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract or not a class.</exception>
        /// <exception cref="InvalidCastException">Thrown if the implementation is not assignable to the service type.</exception>
        void RegisterScoped(Type serviceType, Type implementationType);

        /// <summary>
        /// Registers a service with a Scoped lifetime.
        /// </summary>
        /// <exception cref="ArgumentNullException">If any required params are null.</exception>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract or not a class.</exception>
        /// <exception cref="InvalidCastException">Thrown if the implementation is not assignable to the service type.</exception>
        void RegisterScoped(Type serviceType, Type implementationType, Func<object> factory);

        /// <summary>
        /// Registers a service with a Scoped lifetime.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract.</exception>
        void RegisterScoped<TService>() where TService : class;

        /// <summary>
        /// Registers a service with a Scoped lifetime using the provided factory.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract.</exception>
        void RegisterScoped<TService>(Func<TService> factory) where TService : class;

        /// <summary>
        /// Registers a service with a Scoped lifetime.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract.</exception>
        void RegisterScoped<TService, TImplementation>() where TImplementation : class, TService;

        /// <summary>
        /// Registers a service with a Scoped lifetime using the provided factory.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the implementation is abstract.</exception>
        void RegisterScoped<TService, TImplementation>(Func<TImplementation> factory) where TImplementation : class, TService;

        #endregion
    }
}
