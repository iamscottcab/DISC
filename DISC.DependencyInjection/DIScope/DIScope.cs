using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DISC
{
    internal class DIScope : IDIScope
    {
        internal static RootDIScope RootScope { get; set; }

        protected List<ServiceDescriptor> services = new();

        public DIScope()
        {
            CheckInitialization();
        }

        public DIScope(List<ServiceDescriptor> services) : this ()
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

        public IEnumerable<TService> GetServices<TService>()
        {
            return GetService<IEnumerable<TService>>();
        }

        public TService GetService<TService>()
        {
            return (TService)GetService(typeof(TService), null);
        }

        public object GetService(Type serviceType)
        {
            return GetService(serviceType, null);
        }

        private object GetService(Type serviceType, ServiceLifetime? parentLifetime)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("You must specify a service type to resolve");
            }

            // If we have a colletion we don't want to search for the collection but rather the service it is implementing
            bool isCollectionType = serviceType.IsGenericType && serviceType.IsAssignableToOpenGenericType(typeof(IEnumerable<>));

            // If it's a collection type get the underlying implementation rather than the IEnumerable
            if (isCollectionType)
            {
                serviceType = serviceType.GetGenericArguments().First();
            }

            // Get all descriptors
            var descriptors = GetServiceDescriptors(serviceType);

            if (descriptors.Count == 0)
            {
                throw new NullReferenceException($"A service of type {serviceType.Name} was not registered.");
            }

            // If we don't want a collection just get the last element
            if (!isCollectionType)
            {
                descriptors = descriptors.GetRange(descriptors.Count - 1, 1);
            }

            // Create an array of the correct type for the given implementations (to fix casting and covariance issues later)
            var implementations = Array.CreateInstance(serviceType, descriptors.Count);

            for (var i = 0; i < descriptors.Count; i++)
            {
                var descriptor = descriptors[i];

                // Validate lifetimes
                if (RootScope.Settings.ValidateServiceLifetime && parentLifetime.HasValue && parentLifetime.Value > descriptor.Lifetime)
                {
                    throw new NotSupportedException($"You are creating a captive dependency on type {serviceType.Name}, this is not supoorted by default.");
                }

                // Get Singletons from the root
                if (descriptor.Lifetime == ServiceLifetime.Singleton && RootScope != this)
                {
                    return RootScope.GetService(serviceType, descriptor.Lifetime);
                }

                // Generate the implementation either via the relevant factory, or via the activator
                var implementation = descriptor.Implementation;

                if (implementation == null)
                {
                    if (descriptor.ImplementationFactory != null)
                    {
                        implementation = descriptor.ImplementationFactory();

                        if (!serviceType.IsAssignableFrom(implementation.GetType()))
                        {
                            throw new InvalidCastException($"Type {serviceType.Name} is not assignable from {descriptor.ImplementationType.Name}");
                        }
                    }
                    else
                    {
                        var implementationType = descriptor.ImplementationType;
                        var constructorArgs = GetConstructorInjections(implementationType, descriptor.Lifetime);
                        implementation = Activator.CreateInstance(implementationType, constructorArgs);
                    }

                    // Save any non transient lifetimes (with some validation)
                    if (descriptor.Lifetime == ServiceLifetime.Singleton)
                    {
                        descriptor.Implementation = implementation;
                    }
                    else if (descriptor.Lifetime == ServiceLifetime.Scoped)
                    {
                        if (RootScope == this && !RootScope.Settings.AllowScopedInRoot)
                        {
                            throw new NotSupportedException($"You are unable to inject a Scoped lifetime for {descriptor.ImplementationType.Name} in the root as they effectively become Singletons. This can be overridden in Settings.");
                        }

                        descriptor.Implementation = implementation;
                    }
                }

                implementations.SetValue(implementation, i);
            }

            // If we are returning a collection make sure we create an IEnumerable of the matching type
            // and set the constructor args accordingly. If we aren't a collection we can just return object itself
            // from the array of length 1
            if (isCollectionType)
            {
                return Activator.CreateInstance(typeof(List<>).MakeGenericType(serviceType), new object[] { implementations });
            }
            else
            {
                return implementations.GetValue(0);
            }
        }

        private List<ServiceDescriptor> GetServiceDescriptors(Type serviceType)
        {
            // Get all service descriptors
            var descriptors = services.Where(d => d.ServiceType == serviceType).ToList();

            // If we have results, or we have no results but the type we are requesting is a concrete type then we can return
            // because we can't do any more processing...
            if (descriptors.Count > 0 || !serviceType.IsGenericType)
            {
                return descriptors;
            }

            // If the service is generic try and find the specific implementation for this type.
            var genericDescriptors = services.Where(d => d.ServiceType == serviceType.GetGenericTypeDefinition()).ToList();
            // If we get results internally add some new descriptors (to save doing this code path later) and refetch
            if (genericDescriptors.Count > 0)
            {
                TryUpdateOpenGenericServiceDescriptors(genericDescriptors, serviceType);
                descriptors = services.Where(d => d.ServiceType == serviceType).ToList();
            }

            return descriptors;
        }

        private void TryUpdateOpenGenericServiceDescriptors(List<ServiceDescriptor> genericDescriptors, Type serviceType)
        {
            foreach (var genericDescriptor in genericDescriptors)
            {
                // Create the new implementation type for the concrete generic
                // e.g. IThing<> might now be an IThing<A>
                var implementationType = genericDescriptor.ImplementationType.MakeGenericType(serviceType.GetGenericArguments());

                // Try and fetch the implementation from the root also...
                ServiceDescriptor rootDescriptor = null;

                if (RootScope != this)
                {
                    rootDescriptor = RootScope.GetServiceDescriptors(serviceType).LastOrDefault();
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
        }

        private object[] GetConstructorInjections(Type serviceType, ServiceLifetime parentLifetime)
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

            var parameters = constructorInfo?.GetParameters() ?? new ParameterInfo[0];
            return parameters.Select(p => GetService(p.ParameterType, parentLifetime)).ToArray();
        }

        protected void AddToServices(Type serviceType, Type implementationType, Func<object> implementationFactory, ServiceLifetime lifetime)
        {
            if (implementationType.IsAbstract)
            {
                throw new ArgumentException($"A service of type {implementationType.Name} cannot be instantiated as it is abstract.");
            }

            services.Add(new ServiceDescriptor(serviceType, implementationType, implementationFactory, lifetime));
        }
    }
}
