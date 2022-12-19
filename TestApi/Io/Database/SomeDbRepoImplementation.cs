using DockerWebAPI.DbStuff;
using Microsoft.EntityFrameworkCore;
using TestApi.Controllers.Weather.V1;
using TestApi.Models;

namespace TestApi.Io;

public interface IDbRepoImplementation
{
    Task<int> SaveData(SomeInternalModel data);
    Task<User?> GetUser(int userId);
    Task<int> GetCount();
}

public class DbRepoImplementation : IDbRepoImplementation
{
    private readonly ILogger _logger;
    private RandomDbContext _db;

    public DbRepoImplementation(ILogger<DbRepoImplementation> logger, RandomDbContext db)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<int> SaveData(SomeInternalModel data)
    {
        var entity = new User
        {
            Name = data.Name
        };
        _db.Users.Add(entity);
        await _db.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<User?> GetUser(int userId)
    {
        return await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<int> GetCount()
    {
        return await _db.Users.CountAsync();
    }
}