using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TestApi.Controllers.Weather.V1;
using TestApi.Io;
using TestApi.Models;
using TestApi.Services;

namespace TestApi.Tests.IoTests;

public class IoTest
{
    private IDbRepoImplementation _repo;

    [Test]
    public async Task CreatesUserCorrectly()
    {
        await _repo.SaveData(new SomeInternalModel
        {
            Age = 12,
            Name = "bob"
        });
        // connect directly to db and validate that data is correct in database
    }

    [SetUp]
    public void Setup()
    {
        var apiWebFactory = new TestWebApplicationFactory<UserResponse>();
        _repo = apiWebFactory.Services.CreateScope().ServiceProvider.GetRequiredService<IDbRepoImplementation>();
        
    }
}