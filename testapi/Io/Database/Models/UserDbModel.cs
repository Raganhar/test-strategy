using TestApi.Controllers.Weather.V1;

namespace TestApi.Io.Models;

public class UserDbModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

    public UserResponse ToResponseModel()
    {
        return new UserResponse
        {
            Age = Age,
            Name = Name
        };
    }
}