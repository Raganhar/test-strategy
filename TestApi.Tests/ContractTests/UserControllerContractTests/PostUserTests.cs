using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using TestApi.Controllers.Weather.V1;

namespace TestApi.Tests.ContractTests.UserControllerContractTests;

[Ignore("s")]
public class PostUserTests
{
    private IFlurlRequest _flurlRequest;

    [Test]
    public async Task CreateUser_200()
    {
        var userRequest = new UserRequest
        {
            Age = 9,
            Name = "bob"
        };

        var res = await _flurlRequest.PostJsonAsync(userRequest);
        res.StatusCode.Should().Be(StatusCodes.Status200OK);
        var response = await res.GetJsonAsync<CreateUserResponse>();
        response.Id.Should().NotBe(0);
    }
    
    [Test]
    public async Task CreateUser_204()
    {
        var userRequest = new UserRequest
        {
            Age = 9,
            Name = "bob"
        };

        var res = await _flurlRequest.PostJsonAsync(userRequest);
        res.StatusCode.Should().Be(StatusCodes.Status200OK);
        var response = await res.GetJsonAsync<CreateUserResponse>();
        response.Id.Should().NotBe(0);
    }
    
    
    [Test]
    public async Task CreateUser_400()
    {
        var userRequest = new UserRequest
        {
            Name = "bob"
        };

        var res = await _flurlRequest.PostJsonAsync(userRequest);
        res.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Test]
    public async Task CreateUser_401()
    {
        var userRequest = new UserRequest
        {
            Age = 9,
            Name = "bob"
        };

        var res = await _flurlRequest.PostJsonAsync(userRequest);
        res.StatusCode.Should().Be(StatusCodes.Status200OK);
        var response = await res.GetJsonAsync<CreateUserResponse>();
        response.Id.Should().NotBe(0);
    }

    [Test]
    public async Task CreateUser_403()
    {
        //setup
        
        var userRequest = new UserRequest
        {
            Age = 9,
            Name = "bob"
        };

        var res = await _flurlRequest.PostJsonAsync(userRequest);
        res.StatusCode.Should().Be(StatusCodes.Status200OK);
        var response = await res.GetJsonAsync<CreateUserResponse>();
        response.Id.Should().NotBe(0);
    }

    [SetUp]
    public void Setup()
    {
        var apiWebFactory = new TestWebApplicationFactory<UserResponse>();
        var _flurlClient = new FlurlClient(apiWebFactory.Server.CreateClient());
        _flurlRequest = _flurlClient.Request("api/User").AllowAnyHttpStatus();
    }
}