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
    private TestWebApplicationFactory<UserResponse> _apiWebFactory;

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

    [Test]
    public async Task GetTask()
    {
        var count = await _repo.GetCount();
        count.Should().Be(100);
    }

    [OneTimeSetUp]
    public void onetime()
    {
        _apiWebFactory = new TestWebApplicationFactory<UserResponse>();
    }

    [SetUp]
    public void Setup()
    {
        _repo = _apiWebFactory.Services.CreateScope().ServiceProvider.GetRequiredService<IDbRepoImplementation>();
    }

    [TearDown]
    public void reset()
    {
        _apiWebFactory.ResetDatabase();
    }
}