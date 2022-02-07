using System;
using System.Collections.Generic;
using DependencyInjection;
using NUnit.Framework;
using static DILibrryTest.TestClasses;

public class Tests
{
    private DependencyConfiguration _configuration;
    
    [SetUp]
    public void Setup()
    {
        _configuration = new DependencyConfiguration();
    }

    [Test]
    public void ResolveBasicTypes()
    {
        _configuration.RegisterSingleton<IService, Service1>();
        _configuration.RegisterTransient<AbstractService, AbstractServiceImpl>();
            
        var container = _configuration.GenerateContainer();
            
        var service = container.GetService<IService>();
        var serviceFirst = container.GetService<AbstractService>();
        Assert.AreEqual(typeof(Service1), service.GetType());
        Assert.AreEqual(typeof(AbstractServiceImpl), serviceFirst.GetType());
    }
    
    [Test]
    public void ResolveRecursiveDependency()
    {
        _configuration.RegisterSingleton<IService, Service3>();
        _configuration.RegisterSingleton<IRepository, Repository1>();
        
        var container = _configuration.GenerateContainer();
        IService service = (IService)container.GetService<IService>();

        Assert.AreEqual( typeof(Service3), service.GetType());
    }
    
    [Test]
    public void ResolveTransientCreating()
    {
        _configuration.RegisterTransient<IService, Service1>();
            
        var container = _configuration.GenerateContainer();
        object service1 = container.GetService<IService>();
        object service2 = container.GetService<IService>();

        Assert.AreNotEqual( service1, service2);
    }
    
    [Test]
    public void ResolveEnumerable()
    {
        _configuration.RegisterSingleton<IService, Service1>();
        _configuration.RegisterSingleton<IService, Service2>();
        _configuration.RegisterSingleton<IService, Service3>();
        _configuration.RegisterSingleton<IRepository, Repository1>();
            
        var container = _configuration.GenerateContainer();
        List<object> services = container.GetService<IEnumerable<IService>>() as List<object>;
            
        Assert.AreEqual(3, services?.Count);
        Assert.AreEqual(typeof(Service1), services[0].GetType());
        Assert.AreEqual(typeof(Service2), services[1].GetType());
        Assert.AreEqual(typeof(Service3), services[2].GetType());
    }
    
    [Test]
    public void ResolveGenericDependency()
    {
        _configuration.RegisterSingleton<IRepository, Repository1>();
        _configuration.RegisterSingleton<IService<IRepository>, Service4<IRepository>>();
            
        var container = _configuration.GenerateContainer();
        object service = container.GetService<IService<IRepository>>();
        
        Assert.AreEqual(typeof(Service4<IRepository>), service.GetType());
        Assert.NotNull((service as Service4<IRepository>)?.Repository);    
    }
    
    [Test]
    public void ResolveOpenGenericsDependency()
    {
        _configuration.RegisterSingleton<IRepository, Repository1>();
        _configuration.RegisterSingleton(typeof(IService<>), typeof(Service4<>));
            
        var container = _configuration.GenerateContainer();
        object service =  container.GetService<IService<IRepository>>();
        
        Assert.AreEqual(typeof(Service4<IRepository>), service.GetType());
        Assert.NotNull((service as Service4<IRepository>)?.Repository);
    }
    
    
    
    
    
    
}