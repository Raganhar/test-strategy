using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NSubstitute;
using TestApi.Controllers.Weather.V1;
using TestApi.Services;

namespace TestApi.Tests;

public class TestWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup: class
{
    private Dictionary<Type, object> _servicesToReplace = new Dictionary<Type, object>();

    public TestWebApplicationFactory()
    {
        
    }
    public TestWebApplicationFactory(Dictionary<Type,object> servicesToReplace)
    {
        _servicesToReplace = servicesToReplace;
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ReplaceServiceWithMock(services,_servicesToReplace);
        });
    }

    public static void ReplaceServiceWithMock(IServiceCollection services, Dictionary<Type, object> servicesToReplace)
    {
        foreach (var servicePair in servicesToReplace)
        {
            var businessLogicImplementation = services.Single(x => x.ServiceType == servicePair.Key);
            services.Remove(businessLogicImplementation);
            services.AddScoped(_ =>servicePair.Value);
        }
    }
}