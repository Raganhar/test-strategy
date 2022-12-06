using TestApi.Controllers.Weather.V1;
using TestApi.Models;

namespace TestApi.Io;

public interface IDbRepoImplementation
{
    Task<string> SaveData(SomeInternalModel data);
    Task<UserResponse> GetUser(string userId);
}

public class DbRepoImplementation : IDbRepoImplementation
{
    private readonly ILogger _logger;

    public DbRepoImplementation(ILogger<DbRepoImplementation> logger)
    {
        _logger = logger;
    }

    public async Task<string> SaveData(SomeInternalModel data)
    {
        //DB operation
        return await Task.FromResult(Guid.NewGuid().ToString());
    }

    public async Task<UserResponse> GetUser(string userId)
    {
        //too lazy to db related stuff
        return await Task.FromResult( new UserResponse
        {
            Age = 12,
            Name = "bob"
        });
    }
}