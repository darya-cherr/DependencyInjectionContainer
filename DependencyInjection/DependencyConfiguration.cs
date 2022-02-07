using System.Collections.Generic;

namespace DependencyInjection
{
    public class DependencyConfiguration
    {
        private List<ServiceInfo> _servicesInfo = new List<ServiceInfo>();
        
        public void RegisterSingleton<TService, TImplementation>() where TImplementation : TService
        {
            _servicesInfo.Add(new ServiceInfo(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
        }
        
         public void RegisterTransient<TService, TImplementation>() where TImplementation : TService
         { 
             _servicesInfo.Add(new ServiceInfo(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
         }
        
        
        public DIContainer GenerateContainer()
        {
            return new DIContainer(_servicesInfo);
        }

       
    }

}