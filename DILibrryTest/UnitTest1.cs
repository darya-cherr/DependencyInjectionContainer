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
    
    
    
}