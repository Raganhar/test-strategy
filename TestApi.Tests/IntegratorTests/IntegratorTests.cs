using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TestApi.Controllers.Weather.V1;
using TestApi.Services;

namespace TestApi.Tests.FunctionalTests;

[Ignore("s")]
public class IntegratorTests
{
    private IFlurlRequest _flurlRequest;

    [Test]
    public async Task CreateUser()
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
    public async Task GetUser()
    {
        //logic for getting a userid from database
        var useridFromDatabase = GetExistingUser();

        var res = await _flurlRequest.SetQueryParam("userId",useridFromDatabase).GetAsync();
        res.StatusCode.Should().Be(StatusCodes.Status200OK);
        var response = await res.GetJsonAsync<UserResponse>();
        response.Age.Should().Be(12);
        response.Name.Should().Be("bob");
    }

    private static string GetExistingUser()
    {
        return Guid.NewGuid().ToString();
    }

    [SetUp]
    public void Setup()
    {
        var apiWebFactory = new TestWebApplicationFactory<UserResponse>();
        var _flurlClient = new FlurlClient(apiWebFactory.Server.CreateClient());
        _flurlRequest = _flurlClient.Request("api/User");
    }
}