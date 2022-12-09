using DockerWebAPI.DbStuff;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TestApi.Controllers.Weather.V1;
using TestApi.Services;

namespace TestApi.Tests.ContractTests;

[Ignore("s")]
public class UserBusinessFlowTests
{
    private UserController _userController;
    private IBusinessLogicImplementation _service;
    private IServiceProvider _provider;

    public UserBusinessFlowTests()
    {
        ;
    }

    [Test]
    public async Task HappyPath()
    {
        
        
        var apiWebFactory = new TestWebApplicationFactory<UserResponse>();
        _provider = apiWebFactory.Services.CreateScope().ServiceProvider;
        _userController = _provider.GetRequiredService<UserController>();
        _service = _provider.GetRequiredService<IBusinessLogicImplementation>();
        var appContext = _provider.GetRequiredService<RandomDbContext>();
        appContext.Database.Migrate();
        
        
        var userRequest = new UserRequest
        {
            Age = 9,
            Name = "bob"
        };
        var post = await _userController.Post(_service, userRequest);
        var res = post.AssertIsType<OkObjectResult>();
        res.Value.Should().NotBeNull();
        var createUserResponse = res.Value.AssertIsType<CreateUserResponse>();
        createUserResponse.Id.Should().NotBe(0);
    }
    
    
    [SetUp]
    public void Setup()
    {
    }
}