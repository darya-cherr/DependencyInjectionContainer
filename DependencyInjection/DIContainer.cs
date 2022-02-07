using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DependencyInjection
{
    public class DIContainer
    {
        private Dictionary<Type, List<ServiceInfo>> _servicesInfo;
        public List<Object> listInstances;
        
        public DIContainer(Dictionary<Type, List<ServiceInfo>> servicesInfo)
        {
            _servicesInfo = servicesInfo;
             listInstances = new List<Object>();
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

            if (serviceType.IsGenericType)
            {
                Type genericTypeDefinition = serviceType.GetGenericTypeDefinition();
                Type genericArgument = serviceType.GetGenericArguments()[0];
                if (_servicesInfo.ContainsKey(genericTypeDefinition) && _servicesInfo.ContainsKey(genericArgument))
                {
                    return  GetServiceOpenGeneric((_servicesInfo[genericTypeDefinition].First().ImplementationType),
                        genericArgument);
                }
            }

            var servInfo =_servicesInfo[serviceType].First(x => x.Implementation == null);
          
            
            if (servInfo == null)
            {
                throw new Exception($"Service of type {serviceType.Name} isn't registered");
            }
            
            if (servInfo.Implementation != null)
            {
                listInstances.Add(servInfo.Implementation);
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
            listInstances.Add(servInfo.Implementation);
            
            return instance;
        }


        public object GetServiceOpenGeneric(Type baseType, Type genericArgumentType)
        {
            Type type = baseType.MakeGenericType(genericArgumentType);
            ConstructorInfo[] constructors = type.GetConstructors();

            foreach (var constructorInfo in constructors)
            {
                ParameterInfo[] parameters = constructorInfo.GetParameters();
                if (parameters.All(param => _servicesInfo.ContainsKey(param.ParameterType)))
                {
                    Object value;

                    if (instanceWasCreate(parameters))
                    {
                        value = constructors[1].Invoke(null);
                        // listPoint.Add(value);
                    }
                    else
                    {
                        value = constructorInfo.Invoke(parameters.Select(param =>
                            GetService(_servicesInfo[param.ParameterType].First().ServiceType)).ToArray());
                    }

                    return value;
                }
            }

            return default;
        }


        private bool instanceWasCreate(ParameterInfo[] parameters)
        {
            foreach (var param in parameters)
            {
                Type type = _servicesInfo[param.ParameterType].First().ServiceType;
                if (listInstances.Contains(type))
                {
                    return true;
                }
            }
            return false;
        }
        
        public object GetService<Object>()
        {
            return (object)GetService(typeof(Object));
        }
    }
}