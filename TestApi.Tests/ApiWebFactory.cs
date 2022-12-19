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
using MySql.Data.MySqlClient;
using NSubstitute;
using NUnit.Framework;
using Respawn;

namespace TestApi.Tests;

public class TestWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    public MySqlTestcontainer _db = new TestcontainersBuilder<MySqlTestcontainer>()
        .WithDatabase(new MySqlTestcontainerConfiguration
        {
            Database = "somedb",
            Password = "root",
            Username = "root",
        }).Build();

    private Respawner _respawner;
    private MySqlConnection _mySqlConnection;

    public TestWebApplicationFactory()
    {
        _db.StartAsync().Wait();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // services.RemoveAll(typeof(RandomDbContext));
            
            services.AddDbContext<RandomDbContext>(x =>
                x.UseMySql(_db.ConnectionString, ServerVersion.AutoDetect(_db.ConnectionString)));
            var serviceProvider = services.BuildServiceProvider().CreateScope().ServiceProvider;    
            var db = serviceProvider.GetRequiredService<RandomDbContext>();
            _mySqlConnection = new MySql.Data.MySqlClient.MySqlConnection(_db.ConnectionString);
            _mySqlConnection.Open();
            var respawner = Respawn.Respawner.CreateAsync(_mySqlConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.MySql,
                SchemasToInclude = new []{"somedb"},
                
            });
            respawner.Wait();
            _respawner = respawner.Result;
            SetupDatabase(db);
        });
    }

    private static void SetupDatabase(RandomDbContext db)
    {
        db.Database.Migrate();
        SeedDatabase(db);
    }

    public void ResetDatabase()
    {
        _respawner.ResetAsync(_mySqlConnection).Wait();
        SetupDatabase(Services.CreateScope().ServiceProvider.GetRequiredService<RandomDbContext>());   
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
}