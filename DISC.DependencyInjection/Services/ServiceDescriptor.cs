using System;

namespace DISC
{
    internal class ServiceDescriptor
    {
        public Type ImplementationType { get; }

        public object Implementation { get; internal set; }

        public Func<object> ImplementationFactory { get; }

        public ServiceLifetime Lifetime { get; }

        public ServiceDescriptor(Type implementationType, Func<object> implementationFactory, ServiceLifetime lifetime)
        {
            ImplementationType = implementationType;
            ImplementationFactory = implementationFactory;
            Lifetime = lifetime;
        }
    }
}
