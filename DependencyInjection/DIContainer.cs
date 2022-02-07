using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjection
{
    public class DIContainer
    {
        private List<ServiceInfo> _servicesInfo;
        public DIContainer(List<ServiceInfo> servicesInfo)
        {
            _servicesInfo = servicesInfo;
        }

        public object GetService(Type serviceType)
        {
            var servInfo = _servicesInfo.SingleOrDefault(x => x.ServiceType == serviceType);
            
            if (servInfo == null)
            {
                throw new Exception($"Service of type {serviceType.Name} isn't registered");
            }
            
            if (servInfo.Implementation != null)
            {
                return servInfo.Implementation;
            }
            
            var actualType = servInfo.ImplementationType ?? servInfo.ServiceType;
            var constructorInfo = actualType.GetConstructors().First();
            var parameters = constructorInfo.GetParameters().Select(x => GetService(x.ParameterType)).ToArray();
            
            var instance = Activator.CreateInstance(actualType, parameters);
            
            if (servInfo.Lifetime == ServiceLifetime.Singleton)
            {
                servInfo.Implementation = instance;    
            }
            
            return instance;
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }
    }
}