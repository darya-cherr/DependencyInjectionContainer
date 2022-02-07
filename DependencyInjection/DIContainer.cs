using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjection
{
    public class DIContainer
    {
        private Dictionary<Type, List<ServiceInfo>> _servicesInfo;
        
        public DIContainer(Dictionary<Type, List<ServiceInfo>> servicesInfo)
        {
            _servicesInfo = servicesInfo;
        }

        public object GetService(Type serviceType)
        {
            if (typeof(IEnumerable).IsAssignableFrom(serviceType))
            { 
                Type genericType = serviceType.GetGenericArguments()[0];
                if (_servicesInfo.ContainsKey(genericType))
                {
                    List<Object> list = new List<object>();
                    foreach (var implementation in _servicesInfo[genericType])
                    {
                        list.Add(GetService(implementation.ServiceType));
                    }
                    return (Object)list.AsEnumerable();
                }
            }

            
            var servInfo =_servicesInfo[serviceType].First(x => x.Implementation == null);
          
            
            if (servInfo == null)
            {
                throw new Exception($"Service of type {serviceType.Name} isn't registered");
            }
            
            if (servInfo.Implementation != null)
            {
                return servInfo.Implementation;
            }
            
            var actualType = servInfo.ImplementationType ?? servInfo.ServiceType;
            
            if (actualType.IsAbstract || actualType.IsInterface)
            {
                throw new Exception("Cannot instantiate abstract classes or interfaces");
            }
            
            var constructorInfo = actualType.GetConstructors().First();
            var parameters = constructorInfo.GetParameters().Select(x => GetService(x.ParameterType)).ToArray();
            
            var instance = Activator.CreateInstance(actualType, parameters);
            
            if (servInfo.Lifetime == ServiceLifetime.Singleton)
            {
                servInfo.Implementation = instance;    
            }
            
            return instance;
        }
        
        public object GetService<Object>()
        {
            return (object)GetService(typeof(Object));
        }
    }
}