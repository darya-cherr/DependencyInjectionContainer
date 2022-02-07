using System.Collections.Generic;

namespace DependencyInjection
{
    public class DependencyConfiguration
    {
        private List<ServiceInfo> _servicesInfo = new List<ServiceInfo>();
        
        public void RegisterSingleton<TService, TImplementation>()
        {
            _servicesInfo.Add(new ServiceInfo(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
        }
        
        
        public DIContainer GenerateContainer()
        {
            throw new System.NotImplementedException();
        }
    }

}