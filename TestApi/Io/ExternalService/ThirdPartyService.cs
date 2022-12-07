using Flurl.Http;

namespace TestApi.Io.ExternalService;

public interface IThirdPartyService
{
    Task<bool> CallExternalService();
}

public class ThirdPartyService : IThirdPartyService
{
    private readonly ILogger _logger;

    public ThirdPartyService(ILogger<ThirdPartyService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> CallExternalService()
    {
        try
        {
            return !string.IsNullOrEmpty((await "https://google.dk".GetStringAsync()));
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Something went wrong");
            return false;
        }
    }
}