using System;

namespace DependencyInjection
{
    public class ServiceInfo
    {
        public Type ServiceType { get; }
        
        public Type ImplementationType { get; }

        public object Implementation { get; internal set; }
        

        public ServiceInfo(Type serviceType, Type implementationType)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
        }
    }
}