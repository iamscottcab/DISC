using System;
using System.Collections.Generic;
using System.Linq;

namespace DISC
{
    internal class RootDIScope : DIScope, IRootDIScope, IScopeProvider
    {
        internal static Type entryPointType = null;

        public RootDIScope()
        {
            RegisterSingleton<IScopeProvider, RootDIScope>();
        }

        protected override void CheckInitialization()
        {
            // Noop, we want to be able to create this without checking if this has been created already unlike other scopes...
        }

        #region Bootstrapping

        public void RegisterEntryPoint<TService>() where TService : class
        {
            if (entryPointType != null)
            {
                throw new NotSupportedException($"Entrypoint of type {entryPointType.Name} is already registered.");
            }

            RegisterSingleton<TService>();
            entryPointType = typeof(TService);
        }

        public TService ResolveEntryPoint<TService>() where TService : class
        {
            if (entryPointType == null)
            {
                throw new NullReferenceException("No entry point has been registered.");
            }

            if (typeof(TService) != entryPointType)
            {
                throw new ArgumentException($"Registered entry point {entryPointType.Name} does not match the requested {typeof(TService).Name}.");
            }

            return GetService<TService>();
        }

        #endregion

        #region Singletons

        public void RegisterSingleton<TService>() where TService : class
        {
            RegisterSingleton<TService, TService>();
        }

        public void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService
        {
            AddToServices(typeof(TService), typeof(TImplementation), null, ServiceLifetime.Singleton);
        }

        #endregion

        #region Transients

        public void RegisterTransient<TService>() where TService : class
        {
            RegisterTransient<TService, TService>();
        }

        public void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService
        {
            AddToServices(typeof(TService), typeof(TImplementation), null, ServiceLifetime.Transient);
        }

        #endregion

        public IDIScope CreateScope()
        {
            var servicesCopy = services.Select(entry =>
            {
                var rootDescriptor = entry.Value;

                var serviceDescriptor = new ServiceDescriptor(
                    rootDescriptor.ImplementationType,
                    rootDescriptor.ImplementationFactory,
                    rootDescriptor.Lifetime);

                return new KeyValuePair<Type, ServiceDescriptor>(entry.Key, serviceDescriptor);
            }).ToDictionary(s => s.Key, s => s.Value);

            return new DIScope(servicesCopy);
        }
    }
}
