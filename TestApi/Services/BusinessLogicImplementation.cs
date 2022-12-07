using TestApi.Controllers.Weather.V1;
using TestApi.Io;
using TestApi.Io.ExternalService;
using TestApi.Models;

namespace TestApi.Services;

public interface IBusinessLogicImplementation
{
    Task<CreateUserResponse?> SaveToDbIfConditionsAreRight(SomeInternalModel model);
    Task<UserResponse?> User(int userId);
}

public class BusinessLogicImplementation : IBusinessLogicImplementation
{
    private readonly ILogger _logger;
    private readonly IDbRepoImplementation _repoImplementation;
    private readonly IThirdPartyService _thirdPartyService;

    public BusinessLogicImplementation(ILogger<BusinessLogicImplementation> logger,
        IDbRepoImplementation repoImplementation, IThirdPartyService thirdPartyService)
    {
        _logger = logger;
        _repoImplementation = repoImplementation;
        _thirdPartyService = thirdPartyService;
    }

    public async Task<CreateUserResponse?> SaveToDbIfConditionsAreRight(SomeInternalModel model)
    {
        var callExternalService = await _thirdPartyService.CallExternalService();
        if (callExternalService)
        {
            var newUserId = await _repoImplementation.SaveData(model);
            return new CreateUserResponse
            {
                Id = newUserId
            };
        }
        else
        {
            // not really sure what to do here
            return null;
        }
    }

    public async Task<UserResponse?> User(int userId)
    {
        var user = await _repoImplementation.GetUser(userId);
        return user?.ToResponseModel();
    }
}