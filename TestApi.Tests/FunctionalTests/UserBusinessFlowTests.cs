using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TestApi.Controllers.Weather.V1;
using TestApi.Services;

namespace TestApi.Tests.ContractTests;

public class UserBusinessFlowTests
{
    private UserController _userController;
    private IBusinessLogicImplementation _service;

    [Test]
    public async Task HappyPath()
    {
        var userRequest = new UserRequest
        {
            Age = 9,
            Name = "bob"
        };
        var post = await _userController.Post(_service,userRequest);
        var res = post.AssertIsType<OkObjectResult>();
        res.Value.Should().NotBeNull();
        var createUserResponse = res.Value.AssertIsType<CreateUserResponse>();
        createUserResponse.Id.Should().NotBeNull();
    }

    [SetUp]
    public void Setup()
    {
        var apiWebFactory = new TestWebApplicationFactory<UserResponse>();
        var provider = apiWebFactory.Services.CreateScope().ServiceProvider;
        _userController= provider.GetRequiredService<UserController>();
        _service= provider.GetRequiredService<IBusinessLogicImplementation>();
    }
}