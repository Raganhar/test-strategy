using FluentAssertions;
using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using TestApi.Io.ExternalService;

namespace TestApi.Tests.IoTests;

[Ignore("s")]
public class ThirdPartyTests
{
    private ThirdPartyService _service;
    private ILogger<ThirdPartyService> _logger;

    [Test]
    public async Task HappyPath()
    {
        var response = await _service.CallExternalService();
        response.Should().BeTrue();
    }

    [Test]
    public async Task Timeout()
    {
        using (var httpTest = new HttpTest())
        {
            httpTest.SimulateTimeout();
            var response = await _service.CallExternalService();
            response.Should().BeFalse();
            // _logger.Received().LogError(Arg.Any<TimeoutException>(),"Something went wrong");
        }
    }
    
    
    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<ThirdPartyService>>();
        _service = new ThirdPartyService(_logger);
    }
}