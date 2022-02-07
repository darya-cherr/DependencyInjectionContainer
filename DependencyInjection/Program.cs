using System;
using DependencyInjection;

namespace DependencyInjectionContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new DependencyConfiguration();
            
            configuration.RegisterSingleton<TService>();
            
            var container = configuration.GenerateContainer();
            
            var service = container.GetService<Type>();
        }
    }
}