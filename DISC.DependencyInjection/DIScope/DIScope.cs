using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DISC
{
    internal class DIScope : IDIScope
    {
        internal static RootDIScope RootScope { get; set; }

        protected Dictionary<Type, ServiceDescriptor> services = new();

        public DIScope()
        {
            CheckInitialization();
        }

        public DIScope(Dictionary<Type, ServiceDescriptor> services) : this ()
        {
            this.services = services;
        }

        protected virtual void CheckInitialization()
        {
            if (RootScope == null)
            {
                throw new NotSupportedException("Cannot create a scope without a root scope");
            }
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("You must specify a service type to resolve");
            }

            var descriptor = GetServiceDescriptor(serviceType);

            if (descriptor == null)
            {
                throw new NullReferenceException($"A service of type {serviceType.Name} was not registered.");
            }

            // If we have a Singleton make sure we always get it from the root scope to prevent accidentally loading the Singleton twice
            if (descriptor.Lifetime == ServiceLifetime.Singleton && RootScope != this)
            {
                return RootScope.GetService(serviceType);
            }

            var implementation = descriptor.Implementation;

            if (implementation == null)
            {
                if (descriptor.ImplementationFactory != null)
                {
                    implementation = descriptor.ImplementationFactory();
                }
                else
                {
                    var implementationType = descriptor.ImplementationType;

                    var constructorArgs = GetConstructorInjections(implementationType);
                    implementation = Activator.CreateInstance(implementationType, constructorArgs);
                }

                if (descriptor.Lifetime == ServiceLifetime.Singleton)
                {
                    descriptor.Implementation = implementation;
                }
            }

            return implementation;
        }

        private ServiceDescriptor GetServiceDescriptor(Type serviceType)
        {
            services.TryGetValue(serviceType, out var descriptor);

            if (descriptor != null || !serviceType.IsGenericType)
            {
                return descriptor;
            }

            services.TryGetValue(serviceType.GetGenericTypeDefinition(), out var genericDescriptor);

            if (genericDescriptor != null)
            {
                TryUpdateOpenGenericServiceDescriptor(genericDescriptor, serviceType);
                services.TryGetValue(serviceType, out descriptor);
            }

            return descriptor;
        }

        private void TryUpdateOpenGenericServiceDescriptor(ServiceDescriptor genericDescriptor, Type serviceType)
        {
            // Create the new implementation type for the concrete generic
            // e.g. IThing<> might now be an IThing<A>
            var implementationType = genericDescriptor.ImplementationType.MakeGenericType(serviceType.GetGenericArguments());

            // Try and fetch the implementation from the root also...
            ServiceDescriptor rootDescriptor = null;

            if (RootScope != this)
            {
                rootDescriptor = RootScope.GetServiceDescriptor(serviceType);
            }

            // If we got a match check if the implementation types match (perhaps another scope created this concrete generic in the meantime)
            if (rootDescriptor != null && rootDescriptor.ImplementationType == implementationType)
            {
                AddToServices(serviceType, rootDescriptor.ImplementationType, rootDescriptor.ImplementationFactory, rootDescriptor.Lifetime);
            }
            // If we didn't get a match on type then add this to both the local scope (and the root if we are not the root)
            // so that other scopes can resolve the concrete generic the same way.
            else
            {
                AddToServices(serviceType, implementationType, genericDescriptor.ImplementationFactory, genericDescriptor.Lifetime);

                if (RootScope != this)
                {
                    RootScope.AddToServices(serviceType, implementationType, genericDescriptor.ImplementationFactory, genericDescriptor.Lifetime);
                }
            }
        }

        private object[] GetConstructorInjections(Type serviceType)
        {
            ConstructorInfo constructorInfo = null;

            try
            {
                 constructorInfo = serviceType.GetConstructors().SingleOrDefault();
            } 
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Service of type {serviceType.Name} has an ambiguous amount of constructors, it should have exactly 0 or 1.", ex);
            }

            var parameters = constructorInfo?.GetParameters() ?? new System.Reflection.ParameterInfo[0];
            return parameters.Select(p => GetService(p.ParameterType)).ToArray();
        }

        protected void AddToServices(Type serviceType, Type implementationType, Func<object> implementationFactory, ServiceLifetime lifetime)
        {
            if (implementationType.IsAbstract)
            {
                throw new ArgumentException($"A service of type {implementationType.Name} cannot be instantiated as it is abstract.");
            }

            services[serviceType] = new ServiceDescriptor(implementationType, implementationFactory, lifetime);
        }
    }
}
