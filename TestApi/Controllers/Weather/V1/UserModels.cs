using System.ComponentModel.DataAnnotations;
using TestApi.Models;

namespace TestApi.Controllers.Weather.V1;

public class UserRequest
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    [Range(0,12)]
    public int? Age { get; set; }
    
    public SomeInternalModel ToInternalModel()
    {
        return new SomeInternalModel
        {
            Age = Age.Value,
            Name = Name
        };
    }
    
}

public class CreateUserResponse
{
    public int Id { get; set; }
}

public class UserResponse
{
    public string Name { get; set; }
    public int Age { get; set; }
}