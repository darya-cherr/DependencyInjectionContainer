using System;
using System.Collections.Generic;

namespace DependencyInjection
{
    public class DependencyConfiguration
    {
        private readonly Dictionary<Type, List<ServiceInfo> > _servicesInfo = new ();
        
        public void RegisterSingleton<TService, TImplementation>() where TImplementation : TService
        {
            if (!_servicesInfo.ContainsKey(typeof(TService)))
            { 
                _servicesInfo[typeof(TService)] = new List<ServiceInfo>();
            }
            _servicesInfo[typeof(TService)].Add(new ServiceInfo(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
        }
        
         public void RegisterTransient<TService, TImplementation>() where TImplementation : TService
         { 
             if (!_servicesInfo.ContainsKey(typeof(TService)))
             { 
                 _servicesInfo[typeof(TService)] = new List<ServiceInfo>();
             }
             _servicesInfo[typeof(TService)].Add(new ServiceInfo(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
         }
        
        
        public DIContainer GenerateContainer()
        {
            return new DIContainer(_servicesInfo);
        }

       
    }

}