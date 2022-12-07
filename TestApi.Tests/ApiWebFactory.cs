using System.Diagnostics;
using DockerWebAPI.DbStuff;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NSubstitute;
using NUnit.Framework;

namespace TestApi.Tests;

public class TestWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    private Dictionary<Type, object> _servicesToReplace = new Dictionary<Type, object>();

    public MySqlTestcontainer _db = new TestcontainersBuilder<MySqlTestcontainer>()
        .WithDatabase(new MySqlTestcontainerConfiguration
        {
            Database = "somedb",
            Password = "root",
            Username = "root",
        }).Build();

    public TestWebApplicationFactory()
    {
        var startNew = Stopwatch.StartNew();
        _db.StartAsync().Wait();
        Console.WriteLine(startNew.Elapsed);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // services.RemoveAll(typeof(RandomDbContext));
            
            services.AddDbContext<RandomDbContext>(x =>
                x.UseMySql(_db.ConnectionString, ServerVersion.AutoDetect(_db.ConnectionString)));
            // // ReplaceServiceWithMock(services,_servicesToReplace);
        });
    }

    private static void SeedDatabase(RandomDbContext appContext)
    {
        var enumerable = Enumerable.Range(0, 100).Select(x => new User
        {
            Name = $"Name_{x}"
        });
        appContext.Users.AddRange(enumerable);
        appContext.SaveChanges();
    }

    protected override void Dispose(bool disposing)
    {
        _db.StopAsync().Wait();
        base.Dispose(disposing);
    }

    public static void ReplaceServiceWithMock(IServiceCollection services, Dictionary<Type, object> servicesToReplace)
    {
        foreach (var servicePair in servicesToReplace)
        {
            var businessLogicImplementation = services.Single(x => x.ServiceType == servicePair.Key);
            services.Remove(businessLogicImplementation);
            services.AddScoped(_ => servicePair.Value);
        }
    }
}