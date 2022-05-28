using System;
using System.Collections.Generic;
using System.Linq;

namespace DISC
{
    internal class RootDIScope : DIScope, IRootDIScope, IScopeProvider
    {
        internal static Type entryPointType = null;

        public DISettings Settings { get; private set; }


        public RootDIScope(DISettings settings)
        {
            settings ??= new();
            Settings = settings;

            RegisterSingleton<IScopeProvider, RootDIScope>(() => this);
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

        public void RegisterSingleton(Type serviceType)
        {
            RegisterSingleton(serviceType, serviceType, null);
        }

        public void RegisterSingleton(Type serviceType, Func<object> factory)
        {
            RegisterSingleton(serviceType, serviceType, factory);
        }

        public void RegisterSingleton(Type serviceType, Type implementationType)
        {
            RegisterSingleton(serviceType, implementationType, null);
        }

        public void RegisterSingleton(Type serviceType, Type implementationType, Func<object> factory)
        {
            ValidateTypes(serviceType, implementationType);
            AddToServices(serviceType, implementationType, factory, ServiceLifetime.Singleton);
        }

        public void RegisterSingleton<TService>() where TService : class
        {
            RegisterSingleton<TService, TService>(null);
        }

        public void RegisterSingleton<TService>(Func<TService> factory) where TService : class
        {
            RegisterSingleton<TService, TService>(factory);
        }

        public void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService
        {
            RegisterSingleton<TService, TImplementation>(null);
        }

        public void RegisterSingleton<TService, TImplementation>(Func<TImplementation> factory) where TImplementation : class, TService
        {
            AddToServices(typeof(TService), typeof(TImplementation), factory, ServiceLifetime.Singleton);
        }

        #endregion

        #region Transients

        public void RegisterTransient(Type serviceType)
        {
            RegisterTransient(serviceType, serviceType, null);
        }

        public void RegisterTransient(Type serviceType, Func<object> factory)
        {
            RegisterTransient(serviceType, serviceType, factory);
        }

        public void RegisterTransient(Type serviceType, Type implementationType)
        {
            RegisterTransient(serviceType, implementationType, null);
        }

        public void RegisterTransient(Type serviceType, Type implementationType, Func<object> factory)
        {
            ValidateTypes(serviceType, implementationType);
            AddToServices(serviceType, implementationType, factory, ServiceLifetime.Transient);
        }

        public void RegisterTransient<TService>() where TService : class
        {
            RegisterTransient<TService, TService>(null);
        }

        public void RegisterTransient<TService>(Func<TService> factory) where TService : class
        {
            RegisterTransient<TService, TService>(factory);
        }

        public void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService
        {
            RegisterTransient<TService, TImplementation>(null);
        }

        public void RegisterTransient<TService, TImplementation>(Func<TImplementation> factory) where TImplementation : class, TService
        {
            AddToServices(typeof(TService), typeof(TImplementation), factory, ServiceLifetime.Transient);
        }

        #endregion

        #region Scoped

        public void RegisterScoped(Type serviceType)
        {
            RegisterScoped(serviceType, serviceType, null);
        }

        public void RegisterScoped(Type serviceType, Func<object> factory)
        {
            RegisterScoped(serviceType, serviceType, factory);
        }

        public void RegisterScoped(Type serviceType, Type implementationType)
        {
            RegisterScoped(serviceType, implementationType, null);
        }

        public void RegisterScoped(Type serviceType, Type implementationType, Func<object> factory)
        {
            ValidateTypes(serviceType, implementationType);
            AddToServices(serviceType, implementationType, factory, ServiceLifetime.Scoped);
        }

        public void RegisterScoped<TService>() where TService : class
        {
            RegisterScoped<TService, TService>(null);
        }

        public void RegisterScoped<TService>(Func<TService> factory) where TService : class
        {
            RegisterScoped<TService, TService>(factory);
        }

        public void RegisterScoped<TService, TImplementation>() where TImplementation : class, TService
        {
            RegisterScoped<TService, TImplementation>(null);
        }

        public void RegisterScoped<TService, TImplementation>(Func<TImplementation> factory) where TImplementation : class, TService
        {
            AddToServices(typeof(TService), typeof(TImplementation), factory, ServiceLifetime.Scoped);
        }

        #endregion

        public IDIScope CreateScope()
        {
            var servicesCopy = services.Select(entry => 
                new ServiceDescriptor(entry.ServiceType,
                    entry.ImplementationType,
                    entry.ImplementationFactory,
                    entry.Lifetime)).ToList();

            return new DIScope(servicesCopy);
        }

        private void ValidateTypes(Type serviceType, Type implementationType)
        {
            if (serviceType == null || implementationType == null)
            {
                throw new ArgumentNullException("Unable to pass null types to Register.");
            }

            if (!implementationType.IsClass)
            {
                throw new ArgumentException($"Type {implementationType.Name} must be both a class and not abstract.");
            }

            var assignableFrom = serviceType.IsAssignableFrom(implementationType);
            var assignableToGeneric = implementationType.IsAssignableToOpenGenericType(serviceType);

            if (!assignableFrom && !assignableToGeneric)
            {
                throw new InvalidCastException($"Type {implementationType.Name} must be assignable to {serviceType.Name}.");
            }
        }
    }
}
