using Microsoft.EntityFrameworkCore;
using TestApi.Controllers.Weather.V1;

namespace DockerWebAPI.DbStuff;

public class RandomDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public string DbPath { get; }

    public RandomDbContext(DbContextOptions<RandomDbContext> context) : base(context)
    {
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }

    public UserResponse? ToResponseModel()
    {
        return new UserResponse
        {
            Name = Name
        };
    }
}