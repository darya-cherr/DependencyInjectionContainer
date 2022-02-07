using System;

namespace DependencyInjection
{
    public class ServiceInfo
    {
        public Type ServiceType { get; }
        
        public Type ImplementationType { get; }

        public object Implementation { get; internal set; }

        public ServiceLifetime Lifetime { get; }

        public ServiceInfo(Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            ServiceType = serviceType;
            Lifetime = lifetime;
            ImplementationType = implementationType;
        }
    }
}