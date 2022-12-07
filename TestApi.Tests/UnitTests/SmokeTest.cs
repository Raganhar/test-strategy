using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using TestApi.Controllers.Weather.V1;
using TestApi.Io;
using TestApi.Io.ExternalService;
using TestApi.Models;
using TestApi.Services;

namespace TestApi.Tests.UnitTests;

public class SmokeTest
{
    private BusinessLogicImplementation _service;
    private IThirdPartyService _thirdPartyService;
    private IDbRepoImplementation _dbRepoImplementation;

    [Test]
    public async Task ThirdPartyServiceIsUp()
    {
        _thirdPartyService.CallExternalService().ReturnsForAnyArgs(true);
        var userId = Random.Shared.Next(Int32.MaxValue);
        _dbRepoImplementation.SaveData(null).ReturnsForAnyArgs(userId);
        
        var response= await _service.SaveToDbIfConditionsAreRight(new SomeInternalModel());
        response.Id.Should().Be(userId);
    }
    
    [Test]
    public async Task NotPossibleToCreateUserDueToExternalPartyIsDown()
    {
        _thirdPartyService.CallExternalService().ReturnsForAnyArgs(false);
        
        var response= await _service.SaveToDbIfConditionsAreRight(new SomeInternalModel());

        await _thirdPartyService.Received().CallExternalService();
        await _dbRepoImplementation.DidNotReceiveWithAnyArgs().SaveData(null);
    }

    [SetUp]
    public void Setup()
    {
        ILogger<BusinessLogicImplementation> logger = Substitute.For<ILogger<BusinessLogicImplementation>>();
        _dbRepoImplementation = Substitute.For<IDbRepoImplementation>();
        _thirdPartyService = Substitute.For<IThirdPartyService>();
        _service = new BusinessLogicImplementation(logger,_dbRepoImplementation,_thirdPartyService);
    }
}