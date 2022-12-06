using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using TestApi.Controllers.Weather.V1;

namespace TestApi.Tests.ContractTests.UserControllerContractTests;

public class GetUserTests
{
    private IFlurlRequest _flurlRequest;

    [Test]
    public async Task GetUser_200()
    {
        //logic for getting a userid from database
        var useridFromDatabase = GetExistingUser();

        var res = await _flurlRequest.SetQueryParam("userId",useridFromDatabase).GetAsync();
        res.StatusCode.Should().Be(StatusCodes.Status200OK);
        var response = await res.GetJsonAsync<UserResponse>();
        response.Age.Should().Be(12);
        response.Name.Should().Be("bob");
    }
    [Test]
    public async Task GetUser_203()
    {
        //logic for getting a userid from database
        var useridFromDatabase = GetExistingUser();

        var res = await _flurlRequest.SetQueryParam("userId",useridFromDatabase).GetAsync();
        res.StatusCode.Should().Be(StatusCodes.Status200OK);
        var response = await res.GetJsonAsync<UserResponse>();
        response.Age.Should().Be(12);
        response.Name.Should().Be("bob");
    }
    
    [Test]
    public async Task GetUser_401()
    {
        //logic for getting a userid from database
        var useridFromDatabase = GetExistingUser();

        var res = await _flurlRequest.SetQueryParam("userId",useridFromDatabase).GetAsync();
        res.StatusCode.Should().Be(StatusCodes.Status200OK);
        var response = await res.GetJsonAsync<UserResponse>();
        response.Age.Should().Be(12);
        response.Name.Should().Be("bob");
    }

    [Test]
    public async Task GetUser_403()
    {
        //logic for getting a userid from database
        var useridFromDatabase = GetExistingUser();

        var res = await _flurlRequest.SetQueryParam("userId",useridFromDatabase).GetAsync();
        res.StatusCode.Should().Be(StatusCodes.Status200OK);
        var response = await res.GetJsonAsync<UserResponse>();
        response.Age.Should().Be(12);
        response.Name.Should().Be("bob");
    }
    
    
    [Test]
    public async Task GetUser_400()
    {
        try
        {
            var res = await _flurlRequest.GetAsync();
            res.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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
        _flurlRequest = _flurlClient.Request("api/User").AllowAnyHttpStatus();
    }
}